'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', '$ionicPlatform', '$timeout', function ($scope, $rootScope, $location, $ionicPlatform, $timeout) {

    // delay before search
    var delay = 2000;
    var timeLastKeyPress = null;


    // if there are some autocomplete datafield, index of the current
    var currentIndex = 0;
    var autocompletes = null;

    var fRefresh = function () {
        if (!$rootScope.$$phase) {
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

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        if ($scope.current != null && $scope.current.autoCompleteTitle != null)
            $(".title.title-center.header-item").html($scope.current.autoCompleteTitle);
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

        var that = this;
        var oldValue = this.value;
        var fnSearch = function () {
            if (oldValue === that.value) autocomplete.get($rootScope.User.CurrentEntity.Id, $rootScope.User.Id, that.value, $scope.current.autoCompleteUrl, success, error);
        };
        if (this.value.length >= 3) $timeout(fnSearch, delay);
        else success(null);
    };


}]);

