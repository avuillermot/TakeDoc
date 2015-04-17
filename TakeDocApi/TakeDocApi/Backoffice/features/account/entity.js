'use strict';
backOffice.controller('entityController', ['$scope', '$rootScope', '$stateParams', 'refreshDetail', function ($scope, $rootScope, $stateParams, refreshDetail) {

    var userToDisplay = $stateParams.user;
    var userEntitys = null;

    var fetchUser = function () {

        var success = function () {
            var userEntityToDisplay = new Array();
            var data = userEntitys.where({ enable: true });
            $.each(data, function (index, value) {
                userEntityToDisplay.push(value);
            });
            $scope.gridUserEntity.data = userEntityToDisplay;
            $scope.$apply();
        };

        var error = function () {
            $rootScope.showModal("Erreur", "Une erreur est survenue lors de l'obtention des entités.")
        };

        var param = {
            userId: (userToDisplay == "current") ? $rootScope.getUser().Id : userToDisplay,
            success: success,
            error: error
        };
        userEntitys = new UserEntitys();
        userEntitys.loadByUser(param);
    };

    $scope.$watch(function () { return refreshDetail.data.calls; }, function () {
        fetchUser();
    });

    $scope.entityToDelete = function () {
        var data = arguments[0].entity;

        var success = function () {
            fetchUser();
        };

        var error = function () {
            $rootScope.showError(arguments[0]);
        };

        var param = {
            userId: data.get("userId"),
            entityId: data.get("id"),
            success: success,
            error: error
        };
        userEntitys = new UserEntitys();
        userEntitys.removeEntityToUser(param);
    };

    $scope.gridUserEntity = {
        enableSorting: true,
        columnDefs: [
          { name: 'Entité', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-remove" ng-click="grid.appScope.entityToDelete(row)"></button>&#160;{{row.entity.attributes.label}}' },
        ],
        data: [ ]
    };
}]);