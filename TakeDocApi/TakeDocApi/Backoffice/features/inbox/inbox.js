'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, documentDisplay, documentsDirectory) {

    var myDocuments = new DocumentsExtended();
    var myMetas = new MetaDatas();
    var myDashBoards = new Dashboards();
    
    var displayInbox = function () {
        $("#viewRight").css("width", "0%");
        $("#viewRight").hide();
        $("#viewLeft").css("width", "98%");
        
        documentDisplay.data.metadatas = [];
        documentDisplay.data.document = null;
        documentDisplay.data.viewType = null;
        documentDisplay.data.calls = documentDisplay.data.calls + 1;
        if (!$scope.$$phase) $scope.$apply();
    }

    var displayDocument = function (document, metas, viewType) {
        $("#viewRight").css("width", "49%");
        $("#viewRight").show();
        $("#viewLeft").css("width", "49%");

        documentDisplay.data.metadatas = metas;
        documentDisplay.data.document = document;
        documentDisplay.data.viewType = viewType;
        documentDisplay.data.calls = documentDisplay.data.calls + 1;
        if (!$scope.$$phase) $scope.$apply();
    }
    
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
           { name: 'Titre', field: 'attributes.label', cellClass: "cell-inbox-item", width: 280 },
           { name: 'Type', field: 'attributes.typeLabel', cellClass: "cell-inbox-item", width: 250 },
           { name: 'Entité', field: 'attributes.entityLabel', cellClass: "cell-inbox-item", width: 100 },
           { name: 'Proprietaire', field: 'attributes.ownerFullName', cellClass: "cell-inbox-item", width: 110 },
           { name: 'Status', field: 'attributes.statusLabel', cellClass: "cell-inbox-item", width: 110 },
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
                displayDocument(toShow, arguments[0], $scope.selectedDirectory);
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
        var success = function () {
            var badges = $(".badge.badge-inbox");
            $.each(badges, function (index, value) {
                var status = value.id.replace("badge-", "");
                if (status === "all") $("#badge-all").html(myDashBoards.count());
                else {
                    $("#badge-" + status).html(myDashBoards.countStatus(status));
                }
            });
        };
        var error = function () {
            $rootScope.showError("Impossible d'obtenir les indicateurs.")
        };
        myDashBoards.load($rootScope.getUser().Id, success, error);
    };

    var loadDocument = function () {
        // load documents
        var param = {
            ownerId: $rootScope.getUser().Id,
            success: function () {
                documentsDirectory.data.documents = arguments[0];
                $scope.gridDocuments.data = documentsDirectory.data.documents.models;
                if (!$scope.$$phase) $scope.$apply();
                resizeGridInbox();
            },
            error: function () {
                $rootScope.showError(arguments[0]);
            }
        };

        if ($scope.selectedDirectory === "MYDOC")
            myDocuments.loadAll(param);
        else if ($scope.selectedDirectory === "INCOMPLETE")
            myDocuments.loadIncomplete(param);
        else if ($scope.selectedDirectory === "COMPLETE")
            myDocuments.loadComplete(param);
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
        displayInbox();
        $scope.selectedDirectory = id;
        loadDocument(id);
    };

    $scope.setSelectedDirectory("MYDOC");
    setDashBoard();
}]);