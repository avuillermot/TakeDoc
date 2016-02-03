'use strict';
takeDoc.controller('autocompleteController', ['$scope', '$rootScope', '$location', '$ionicPlatform', '$timeout', function ($scope, $rootScope, $location, $ionicPlatform, $timeout) {

    var id = null;

    // delay before search
    var delay = 1500;
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
        id = states.stateParams.id;
        var current = $rootScope.CurrentDocument.metadatas.where({ id: id });
        if (current.length > 0) $scope.current = current[0].attributes;
        $scope.$broadcast("autocomplete$refreshPage");
    });

    $scope.doSelect = function (key, value) {
        if (key != null) {
            $scope.current.value = key;
            $scope.current.text = value;
            var versionId = $rootScope.CurrentDocument.document.get("versionId");
            var entityId = $rootScope.CurrentDocument.document.get("entityId");
            $location.path("takePicture/mode/BACK/versionId/" + versionId + "/entity/" + entityId);
        }
    };

    $scope.doReset = function () {
        var versionId = $rootScope.CurrentDocument.document.get("versionId");
        var entityId = $rootScope.CurrentDocument.document.get("entityId");
        $location.path("takePicture/mode/BACK/versionId/" + versionId + "/entity/" + entityId);
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

