'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', '$ionicPlatform', function ($scope, $rootScope, $location, $ionicPlatform) {

    // if there are some autocomplete datafield, index of the current
    var currentIndex = 0;
    var autocompletes = null;

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $rootScope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("autocomplete$refreshPage", fRefresh);

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        // if no autocomplete go to next
        autocompletes = $rootScope.myTakeDoc.Metadatas.filter(function (item) {
            return item.get("htmlType") === "autocomplete";
        });
        if (autocompletes.length == 0) $scope.doSelect(null);
        else $scope.current = autocompletes[currentIndex].attributes;
    });

    $scope.doSelect = function (key) {
        if (key !=  null) $rootScope.myTakeDoc.Metadatas.where({ name: autocompletes[currentIndex].get("name") })[0].set("value", key);
        $location.path($scope.nextUrl.replace("#/", ""));
    };

    $scope.onType = function () {
        var success = function () {
            $scope.items = arguments[0];
            $scope.$broadcast("autocomplete$refreshPage");
        };

        var error = function () {
            $rootScope.PopupHelper.show(arguments[0]);
        };
        autocomplete.get($rootScope.User.CurrentEntityId, $rootScope.User.Id, this.value, $scope.current.autoCompleteUrl, success, error);
    };

}]);

