'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', '$location', function ($scope, $rootScope, $stateParams, $route, $location) {
    var metas = null;

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        metas = new Metadatas("byVersion", $rootScope.documentToAdd.DocumentCurrentVersionId, $rootScope.documentToAdd.EntityId);
        var fn = function (collection) {
            $scope.Metadatas = collection.models;
        };
        metas.fetch({ success: fn } );

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

    $scope.doSave = function () {

        var success = function () {
            $location.path($scope.nextUrl.replace("#/", ""));
        };

        var error = function () {
            $rootScope.ErrorHelper.show("Saisies", arguments[0]);
        };

        var element = angular.element(".metadata-field");
        var retour = metas.save();
        if (retour.valid) success();
        else error(retour.message);
        return false;
    };

}]);
