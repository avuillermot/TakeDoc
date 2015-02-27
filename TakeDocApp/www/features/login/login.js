'use strict';
takeDoc.controller('loginController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
    //$scope.nextUrl = $rootScope.Scenario.start("login").to;
    $scope.identity = {
        login: "eleonore",
        password: "password"
    }
    $scope.doCheck = function () {
        var error = function () {
            $rootScope.User = null;
            $rootScope.ErrorHelper.show("Authentification", "Utilisateur inconnu.");
        };
        var success = function () {
            $rootScope.User = arguments[0];

            $location.path("#/menu");
        };
        $rootScope.User = userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }
}]);
