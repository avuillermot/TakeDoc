'use strict';
takeDoc.controller('loginController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
    $scope.identity = {
        login: "eleonore",
        password: "password"
    };

    $scope.doCheck = function () {
        var error = function () {
            $rootScope.User = null;
            $rootScope.ErrorHelper.show("Authentification", "Utilisateur inconnu.");
        };
        var success = function () {
            $rootScope.User = new userTk(arguments[0]);
            $location.path("menu");
            $scope.$apply();
        };
        userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }
}]);
