'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
    
    var autocompletes = null;

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        // if no autocomplete go to next
        autocompletes = $rootScope.myTakeDoc.Metadatas.filter(function (item) {
            return item.get("htmlType") === "autocomplete";
        });
        if (autocompletes.length == 0) $scope.doSelect();
        $scope.current = autocompletes[0].attributes;
    });

    $scope.doSelect = function () {
        $location.path($scope.nextUrl.replace("#/", ""));
    };

    $scope.onType = function () {
        var success = function () {
            $scope.items = arguments[0];
        };

        var error = function () {
            $rootScope.PopupHelper.show(arguments[0]);
        };

        autocomplete.get(this.value, $scope.current.autoCompleteUrl, success, error);
    };

}]);

