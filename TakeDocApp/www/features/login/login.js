'use strict';
takeDoc.controller('loginController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    $scope.nextUrl = $rootScope.Scenario.start("login").to;

    $scope.doCheck = function () {

        var ok = true;
        if (ok) {
            $rootScope.User = userTkService.logon("", "");
            return true;
        }
        return false;
    }
}]);
