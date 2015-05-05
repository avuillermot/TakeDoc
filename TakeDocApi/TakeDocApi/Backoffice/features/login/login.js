'use strict';
backOffice.controller('loginController', ['$scope', '$rootScope', '$location', 'inputTypes', function ($scope, $rootScope, $location, inputTypes) {

    $scope.identity = {
        login: "avuillermot@gmail.com",
        password: "password"
    };

    $scope.onKeyPress = function (event) {
        if (event.keyCode == 13) this.doCheck();
    };

    $scope.doCheck = function () {

        $rootScope.showLoader('Connexion...');

        var error = function () {
            var data = arguments[0].responseJSON;
            $rootScope.hideLoader();
            $rootScope.setUser(null);
            $rootScope.showModal("Erreur", data.Message);
        };
        var success = function () {
            $rootScope.setUser(new userTk(arguments[0]));
            $location.path("/home");
            $scope.$apply();
        };
        userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }

}]);
