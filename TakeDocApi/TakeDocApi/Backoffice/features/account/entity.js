'use strict';
backOffice.controller('entityController', ['$scope', '$rootScope', '$stateParams', function ($scope, $rootScope, $stateParams) {

    var userToDisplay = $stateParams.user;

    var userEntitys = new UserEntitys();

    $scope.entityToDelete = function () {
        alert("delete");
    };

    $scope.gridUserEntity = {
        enableSorting: true,
        columnDefs: [
          { name: 'Entité', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-remove" ng-click="grid.appScope.entityToDelete(row)"></button>&#160;{{row.entity.attributes.label}}' },
        ],
        data: [ ]
    };

    var success = function () {
        var userEntityToDisplay = userEntitys.where({ enable: true });
        $scope.gridUserEntity.data = userEntityToDisplay;
    };

    var error = function () {
        $rootScope.showModal("Erreur", "Une erreur est survenue lors de l'obtention des entités.")
    };

    var param = {
        userId: (userToDisplay == "current") ? $rootScope.getUser().Id : userToDisplay,
        success: success,
        error: error
    };

    userEntitys.loadByUser(param);

}]);