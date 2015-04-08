'use strict';
backOffice.controller('loginController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.identity = {
        login: "eleonore",
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
            var groups = new GroupTks();
            var ok = function () {
                $rootScope.hideLoader();
                groups = arguments[0];
                var gid = $rootScope.getUser().GroupUserId;
                var current = groups.where({ id: gid });
                if (current != null && current.length > 0) {
                    $rootScope.setGroup(current[0]);
                    $location.path("/home");
                    $scope.$apply();
                }
                else $rootScope.showModal("Erreur", "Votre niveau d'accès est inconnu.");
            };

            var nok = function () {
                $rootScope.hideLoader();
                $rootScope.showModal("Erreur", "Une erreur est survenue lors de l'authentification.");
            };
            $rootScope.setGroup(null);
            $rootScope.setUser(new userTk(arguments[0]));
            groups.loadAll({ success: ok, error: nok });


        };
        userTkService.logon($scope.identity.login, $scope.identity.password, success, error);
        return false;
    }

}]);
