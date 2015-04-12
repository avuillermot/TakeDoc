'use strict';
backOffice.controller('searchUserController', ['$scope', '$rootScope', 'usersResult', function ($scope, $rootScope, usersResult) {
    $scope.search = null;

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
    };
        
    $scope.doSearch = function () {
        var success = function () {
            usersResult.data.users = arguments[0];
            usersResult.data.calls = usersResult.data.calls + 1;
            $scope.$apply();
        };
        var error = function () {
            var data = arguments[0].responseJSON;
            $rootScope.showModal("Erreur", data.Message);
        };
        userTkService.search($scope.search, success, error)
    }

    // add entity all for search cross entity
    $scope.entitys = $rootScope.getUser().Entitys;
    var all = new entity();
    all.Id = null;
    all.Label = "(Toutes)";
    all.Reference = "ALL";
    $scope.entitys.push(all);
    $scope.selectedEntity = all;
}]);