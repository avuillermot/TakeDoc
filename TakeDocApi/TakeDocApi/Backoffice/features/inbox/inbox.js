'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', function ($scope, $rootScope, $stateParams, documentDisplay) {

    var myDocuments = new DocumentsExtended();
    var cellTitle = '<div ng-click="grid.appScope.showMe(row)"><div class="cell-inbox-item-show-me">{{row.entity.attributes.label}}<div id="divStatus" class="inbox-item-{{row.entity.attributes.statusReference}}">{{row.entity.attributes.statusLabel}}</div></div><div class="cell-inbox-item-show-entity">({{row.entity.attributes.entityLabel}} - {{row.entity.attributes.typeLabel}})</div></div>';
    var cellDate = '<div>{{row.entity.attributes.formatDate}}</div>';

    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: '', cellTemplate: cellTitle, cellClass: "cell-inbox-item" },
           { name: 'Date', field: 'attributes.formatDate', cellTemplate: cellDate, cellClass: "cell-inbox-item" }
        ],
        data: []
    };

    var param = {
        userId: $rootScope.getUser().Id,
        success: function () {
            $scope.gridDocuments.data = arguments[0].models;
            $scope.$apply();
            resizeGridInbox();

        },
        error: function () {
            $rootScope.showError(arguments[0]);
        }
    };

    var resizeGridInbox = function () {
        var h = ($(document).height() - 200);
        $("#inbox-items").css('height', h + 'px');
    };
    
    $scope.showMe = function () {
        var toShow = arguments[0].entity;

        var success = function () {
            documentDisplay.data.metadatas = arguments[0];
            documentDisplay.data.document = toShow;
            documentDisplay.data.calls = documentDisplay.data.calls + 1;
            if(!$scope.$$phase) $scope.$apply();
        };
        var error = function() {
            $rootScope.showError(arguments[0]);
        };

        var metas = new Metadatas();
        var param = {
            versionId: toShow.get("versionId"),
            entityId: toShow.get("entityId")
        };
        metas = new Metadatas("byVersion", toShow.get("versionId"), toShow.get("entityId"));
        metas.fetch({ success: success, error: error });
    };

    myDocuments.loadAll(param);
}]);