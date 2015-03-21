'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
    
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
        debugger;
        var toAutocomplete = $rootScope.myTakeDoc.Metadatas.where({ type: "0" });
        if (toAutocomplete.length == 0) $scope.doSelect();
    });

    $scope.doSelect = function () {
        $location.path($scope.nextUrl.replace("#/", ""));
        $scope.$broadcast('autocomplete$refreshPage');
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

