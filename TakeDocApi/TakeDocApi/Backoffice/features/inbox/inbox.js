'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, documentDisplay, documentsDirectory) {

    var myDocuments = new DocumentsExtended();
    var myMetas = new Metadatas();

    // subscribe to displayController that it can update list currently display
    $scope.$watch(function () { return documentsDirectory.data.calls; }, function () {
        if (documentsDirectory.data.documents != null)
            $scope.gridDocuments.data = documentsDirectory.data.documents.models;
    });

    var resizeGridInbox = function () {
        var h = ($(document).height() - 200);
        $("#inbox-items").css('height', h + 'px');
    };

    // document grid display
    var cellTitle = '<div ng-click="grid.appScope.showMe(row)"><div class="cell-inbox-item-title">{{row.entity.attributes.label}}<div id="divStatus" class="inbox-item-{{row.entity.attributes.statusReference}}">{{row.entity.attributes.statusLabel}}</div></div><div class="cell-inbox-item-entity">({{row.entity.attributes.entityLabel}} - {{row.entity.attributes.typeLabel}})</div></div>';
    var cellDate = '<div ng-click="grid.appScope.showMe(row)">{{row.entity.attributes.formatDate}}</div>';

    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: '', cellTemplate: cellTitle, cellClass: "cell-inbox-item" },
           { name: 'Date', field: 'attributes.formatDate', cellTemplate: cellDate, cellClass: "cell-inbox-item" }
        ],
        paginationPageSizes:  [20, 50, 100, 500],
        data: []
    };
    
    // display detail of this document in the display module
    $scope.showMe = function () {
        var toShow = arguments[0].entity;

        var success = function () {
            documentDisplay.data.metadatas = arguments[0];
            documentDisplay.data.document = toShow;
            documentDisplay.data.calls = documentDisplay.data.calls + 1;
            if(!$scope.$$phase) $scope.$apply();
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
        myMetas = new Metadatas();
        myMetas.load(param);
    };

    // load documents
    var param = {
        userId: $rootScope.getUser().Id,
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
    myDocuments.loadAll(param);
}]);