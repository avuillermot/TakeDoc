'use strict';
takeDoc.controller('loginController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $location, $ionicLoading) {
    $scope.identity = {
        login: "eleonore",
        password: "password"
    };

    $scope.doCheck = function () {

        $ionicLoading.show({
            template: 'Connexion...'
        });

        var error = function () {
            $ionicLoading.hide();
            $rootScope.User = null;
            try {
                $rootScope.PopupHelper.show("Authentification", arguments[0].responseJSON.Message);
            }
            catch (ex) {
                $rootScope.PopupHelper.show("Authentification", "Une erreur est survenue lors de l'authentification.");
            }
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
