'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $stateParams, $route, $location, $ionicLoading) {

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("metadata$refreshPage", fRefresh);
    
    var metas = null;
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        metas = new Metadatas("byVersion", $rootScope.documentToAdd.DocumentCurrentVersionId, $rootScope.documentToAdd.EntityId);
        var fn = function (collection) {
            $scope.Metadatas = collection.models;
            try { $scope.$apply(); } catch (ex) { }
        };
        metas.fetch({ success: fn } );

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        $scope.$broadcast('metadata$refreshPage');
    });

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });

        var success = function () {
            $location.path($scope.nextUrl.replace("#/", ""));
            $scope.$apply();
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].message != null) ? arguments[0].message : arguments[0].responseJSON.Message;
            $rootScope.PopupHelper.show("Saisies", msg);
        };

        metas.save({
            userId: $rootScope.documentToAdd.UserCreateData,
            entityId: $rootScope.documentToAdd.EntityId,
            versionId: $rootScope.documentToAdd.DocumentCurrentVersionId
        }, success, error);
        return false;
    };

}]);
