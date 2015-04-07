'use strict';
backOffice.controller('loginController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.identity = {
        login: "",
        password: ""
    };

    $scope.doCheck = function () {

        $rootScope.showLoader('Connexion...');

        var error = function () {
            $rootScope.hideLoader();
            $rootScope.User = null;
            /*try {
                $rootScope.PopupHelper.show("Authentification", arguments[0].responseJSON.Message);
            }
            catch (ex) {
                $rootScope.PopupHelper.show("Authentification", "Une erreur est survenue lors de l'authentification.");
            }*/
        };
        var success = function () {
            $rootScope.User = new userTk(arguments[0]);
             $location.path("/test");
            $scope.$apply();
        };
        userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }

}]);
