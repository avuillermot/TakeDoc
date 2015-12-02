'use strict';
backOffice.controller('loginController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.identity = {
        login: (sessionStorage.getItem('login') != null)?sessionStorage.getItem('login'):"",
        password: ""
    };

    $scope.onKeyPress = function (event) {
        if (event.keyCode == 13) this.doCheck();
    };

    $scope.doCheck = function () {

        var error = function () {
            var data = arguments[0].responseJSON;
            $rootScope.hideLoader();
            $rootScope.setUser(null);
            $rootScope.showModal("Erreur", data.Message);
        };
        var success = function () {
            $rootScope.setUser(new userTk(arguments[0], true));
            $location.path("/home");
            sessionStorage.setItem('login', $scope.identity.login);
            $scope.$apply();
        };

        if ($scope.identity != null && $scope.identity.login != "" && $scope.identity.password != "") {
            $rootScope.showLoader('Connexion...');
            userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        }
        return false;
    }

}]);
