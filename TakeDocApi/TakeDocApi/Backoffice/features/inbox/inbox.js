'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, documentDisplay, documentsDirectory) {

    var myDocuments = new DocumentsExtended();
    var myMetas = new MetaDatas();

    // subscribe to displayController that it can update list currently display
    $scope.$watch(function () { return documentsDirectory.data.calls; }, function () {
        if (documentsDirectory.data.documents != null)
            $scope.gridDocuments.data = documentsDirectory.data.documents.models;
    });

    var resizeGridInbox = function () {
        var h = ($(document).height() - 100);
        $("#inbox-items").css('height', h + 'px');
    };

    /**************************************************
    ***************************************************
    display/use item list and item to display
    ***************************************************
    ***************************************************/
    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: 'attributes.label', cellClass: "cell-inbox-item", width: 180 },
           { name: 'Type', field: 'attributes.typeLabel', cellClass: "cell-inbox-item", width: 150 },
           { name: 'Entité', field: 'attributes.entityLabel', cellClass: "cell-inbox-item", width: 100 },
           { name: 'Propriétaire', field: 'attributes.ownerFullName', cellClass: "cell-inbox-item", width: 80 },
           { name: 'Date', field: 'attributes.formatDate', cellClass: "cell-inbox-item", width: 80 }
        ],
        enableRowHeaderSelection: false,
        enableRowSelection: true,
        multiSelect: false,
        modifierKeysToMultiSelect: false,
        noUnselect : true,
        paginationPageSizes:  [20, 50, 100, 500],
        data: []
    };

    // display detail of this document in the display module
    $scope.gridDocuments.onRegisterApi = function (gridApi) {
        //set gridApi on scope
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function () {
            var toShow = arguments[0].entity;
            var success = function () {
                documentDisplay.data.metadatas = arguments[0];
                documentDisplay.data.document = toShow;
                documentDisplay.data.calls = documentDisplay.data.calls + 1;
                if (!$scope.$$phase) $scope.$apply();
            };
            var error = function () {
                $rootScope.showError(arguments[0]);
            };

            var param = {
                versionId: toShow.get("versionId"),
                entityId: toShow.get("entityId"),
                success: success,
                error: error
            };
            myMetas = new MetaDatas();
            myMetas.load(param);
        });
    };

    var setDashBoard = function () {
        var all = documentsDirectory.data.documents.length;
        var toValidate = documentsDirectory.data.documents.where({ statusReference: "TO_VALIDATE" }).length;
        var approve = documentsDirectory.data.documents.where({ statusReference: "APPROVE" }).length;
        var archive = documentsDirectory.data.documents.where({ statusReference: "ARCHIVE" }).length;
        $("#badge-all").html(all);
        $("#badge-approve").html(approve);
        $("#badge-to-validate").html(toValidate);
        $("#badge-archive").html(archive);

    };

    var loadDocument = function () {
        // load documents
        var param = {
            userId: $rootScope.getUser().Id,
            success: function () {
                documentsDirectory.data.documents = arguments[0];
                $scope.gridDocuments.data = documentsDirectory.data.documents.models;
                if (!$scope.$$phase) $scope.$apply();
                resizeGridInbox();
                setDashBoard();
            },
            error: function () {
                $rootScope.showError(arguments[0]);
            }
        };

        if ($scope.selectedDirectory === "MYDOC")
            myDocuments.loadAll(param);
        else if ($scope.selectedDirectory === "TO_VALIDATE") {
             myDocuments.loadWaitValidate(param);
        }
        else if ($scope.selectedDirectory === "APPROVE") {
            myDocuments.loadApprove(param);
        }
        else if ($scope.selectedDirectory === "ARCHIVE") {
            myDocuments.loadArchive(param);
        }
        else if ($scope.selectedDirectory === "REFUSE") {
            myDocuments.loadRefuse(param);
        }
        else if ($scope.selectedDirectory === "TO_VALIDATE_MANAGER") {
            myDocuments.loadToValidateAsManager(param);
        }
        else {
            documentsDirectory.data.documents = [];
            $scope.gridDocuments.data = [];
        }
    }

    /**************************************************
    ***************************************************
    display/use directory list and directory to display
    ***************************************************
    ***************************************************/
    // set css for selected item
    $scope.isSelectedDirectory = function (id) {
        return $scope.selectedDirectory === id;
    };

    $scope.setSelectedDirectory = function (id) {
        $scope.selectedDirectory = id;
        loadDocument(id);
    };

    $scope.setSelectedDirectory("MYDOC");
}]);