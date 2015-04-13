'use strict';
backOffice.controller('searchUserController', ['$scope', '$rootScope', 'usersResult', function ($scope, $rootScope, usersResult) {
    $scope.search = null;

    $scope.doReset = function () {
        $scope.search = null;
        usersResult.data.users = new Array();
        usersResult.data.calls = usersResult.data.calls + 1;

        $.each($scope.entitys, function (index, value) {
            if (value.Reference == "ALL") $scope.selectedEntity = value;
        });
    }

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
    };
        
    $scope.doSearch = function () {
        var success = function () {
            usersResult.data.users = new Array();
            $.each(arguments[0].value, function (index, value) {
                var user = new userTk(value);
                usersResult.data.users.push(user);
            });

            usersResult.data.calls = usersResult.data.calls + 1;
            $scope.$apply();
        };
        var error = function () {
            $rootScope.showError(arguments[0]);
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