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

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        angular.element("#inputTextValue").val("");
    });

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.items = null;
        
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        // if no autocomplete go to next
        autocompletes = $rootScope.myTakeDoc.Metadatas.filter(function (item) {
            return item.get("htmlType") === "autocomplete";
        });

        if (autocompletes.length == 0) $location.path($scope.nextUrl.replace("#/", ""));
        else $scope.current = autocompletes[currentIndex].attributes;
    });

    $scope.doSelect = function (key, value) {
        if (key != null) {
            $rootScope.myTakeDoc.Metadatas.where({ name: autocompletes[currentIndex].get("name") })[0].set("value", key);
            var fn = function () {
                if (arguments[0] == "Ok") $location.path($scope.nextUrl.replace("#/", ""));
            };
            $rootScope.PopupHelper.show("Client", "Référence : " + value, "OkCancel", fn);
        }
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

