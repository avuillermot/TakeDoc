'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', function ($scope, $rootScope, $stateParams, $route) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var metas = new Metadatas("byVersion", $rootScope.documentToAdd.DocumentCurrentVersionId, $rootScope.documentToAdd.EntityId);
        var fn = function (collection) {
            $scope.Metadatas = collection.models;
        };
        metas.fetch({ success: fn } );

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

}]);
