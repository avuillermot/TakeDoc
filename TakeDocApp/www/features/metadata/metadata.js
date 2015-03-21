'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $stateParams, $route, $location, $ionicLoading) {

    $scope.filterType = function () {
        return (arguments[0].attributes.htmlType !== "autocomplete");
    };
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        // if no metadata go to next
        var metas = $rootScope.myTakeDoc.Metadatas.filter(function (item) {
            return item.get("htmlType") !== "autocomplete";
        });
        if (metas.length == 0) $location.path($scope.nextUrl.replace("#/", ""));
    });

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });

        var success = function () {
            $location.path($scope.nextUrl.replace("#/", ""));
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].message != null) ? arguments[0].message : arguments[0].responseJSON.Message;
            $rootScope.PopupHelper.show("Informations", msg);
        };

        $rootScope.myTakeDoc.Metadatas.save({
            userId: $rootScope.myTakeDoc.UserCreateData,
            entityId: $rootScope.myTakeDoc.EntityId,
            versionId: $rootScope.myTakeDoc.DocumentCurrentVersionId
        }, success, error);
        return false;
    };

}]);
