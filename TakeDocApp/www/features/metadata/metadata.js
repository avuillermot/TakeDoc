'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {

    $scope.filterType = function () {
        return (arguments[0].attributes.htmlType !== "autocomplete");
    };

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("metadata$refreshPage", fRefresh);
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
        // if no metadata go to next
        var metas = $rootScope.myTakeDoc.Metadatas.filter(function (item) {
            return item.get("htmlType") !== "autocomplete";
        });
        if (metas.length == 0) $scope.doSave();
    });

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });

        var success = function () {
            $location.path($scope.nextUrl.replace("#/", ""));
            $scope.$broadcast("metadata$refreshPage");
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].message != null) ? arguments[0].message : arguments[0].responseJSON.Message;
            $rootScope.PopupHelper.show("Informations", msg);
        };

        $rootScope.myTakeDoc.Metadatas.save({
            userId: $rootScope.User.Id,
            entityId: $rootScope.myTakeDoc.get("EntityId"),
            versionId: $rootScope.myTakeDoc.get("DocumentCurrentVersionId")
        }, success, error);
        return false;
    };

}]);
