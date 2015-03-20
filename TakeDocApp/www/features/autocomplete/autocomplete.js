'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $stateParams, $route, $location, $ionicLoading) {
    
    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("autocomplete$refreshPage", fRefresh);

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.items = null;

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

    $scope.doSave = function () {
        return false;
    };

    $scope.doSelect = function () {
        $rootScope.PopupHelper.show(arguments[0]);
    };

    $scope.onType = function () {
        var success = function () {
            $scope.items = arguments[0];
            $scope.$broadcast('autocomplete$refreshPage');
        };

        var error = function () {
            $rootScope.PopupHelper.show(arguments[0]);
        };

        autocomplete.get(this.value, "Client/Cegid/55C72E33-8864-4E0E-9BC8-C82378B2BF8C/", success, error);
    };

}]);

