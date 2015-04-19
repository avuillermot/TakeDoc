'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', function ($scope, $rootScope, $stateParams) {

    var myDocuments = new DocumentsExtended();

    var param = {
        userId: $rootScope.getUser().Id,
        success: function () {
            $scope.gridDocuments.data = arguments[0].models;
        },
        error: function () {
            $rootScope.showError(arguments[0]);
        }
    };
    myDocuments.loadAll(param);

    var cellTitle = '<div style="padding-top: 5px; height:40px">{{row.entity.attributes.label}}<br/>({{row.entity.attributes.entityLabel}})</div>';

    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: '', cellTemplate: cellTitle, cellClass: "cell-inbox-item" },
           { name: 'Type', field: 'attributes.typeLabel', cellClass: "cell-inbox-item" },
           { name: 'Status', field: 'attributes.statusLabel', cellClass: "cell-inbox-item" }
        ]
    };
}]);