'use strict';
takeDoc.controller('loginController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $location, $ionicLoading) {

    $scope.identity = {
        login: (sessionStorage.getItem('login') != null) ? sessionStorage.getItem('login') : "avuillermot@gmail.com",
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
                alert(arguments[0].responseJSON.Message);
            }
            catch (ex) {
                alert("Une erreur est survenue lors de l'authentification.");
            }
        };
        var success = function () {
            $rootScope.User = new userTk(arguments[0], true);
            $location.path("menu");
            if (!$scope.$$phase) $scope.$apply();
        };
        userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }
}]);
