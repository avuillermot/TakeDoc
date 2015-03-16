'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', '$location', function ($scope, $rootScope, $stateParams, $route, $location) {
    var metas = null;
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        metas = new Metadatas("byVersion", $rootScope.documentToAdd.DocumentCurrentVersionId, $rootScope.documentToAdd.EntityId);
        var fn = function (collection) {
            var dico = new Dictionary();
            $scope.Metadatas = collection.models;
            debugger;
            // initialise dans les scopes les données pour les dropdownlist
            /*var metaWithList = collection.where({ isList: true });
            for (var i = 0; i < metaWithList.length; i++) {
                var data = metaWithList[i].get("valueList").models;
                dico.addKeyValue(metaWithList[i].get("id"), metaWithList[i].get("valueList"));
            }
            debugger;
            $scope.listValue = dico;*/
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
        var retour = metas.save({
            userId: $rootScope.documentToAdd.UserCreateData,
            entityId: $rootScope.documentToAdd.EntityId,
            versionId: $rootScope.documentToAdd.DocumentCurrentVersionId
        });
        if (retour.valid) success();
        else error(retour.message);
        return false;
    };

}]);
