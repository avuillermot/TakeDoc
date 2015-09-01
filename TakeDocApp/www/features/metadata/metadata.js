'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {
    
    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };

    $scope.$on("metadata$refreshPage", fRefresh);
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
        $scope.mode = states.stateParams.mode;
        debugger;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        if (metas.length == 0) $scope.doSave(true);
    });

    $scope.doAutocompleteOnFocus = function (id) {
        $location.path("autocomplete/id/" + id);
        if (!$scope.$$phase) $scope.$apply();
    };

    $scope.doMetadataFileOnFocus = function (id) {
        $location.path("upload/id/" + id);
        if (!$scope.$$phase) $scope.$apply();
    };

    $scope.doCancelDocument = function (id) {
        var current = $rootScope.myTakeDoc.Metadatas.where({ id: id });
        if (current.length > 0) {
            current[0].set("value", "");
            var metaFile = current[0].get("file");
            metaFile.set("name", "");
            metaFile.set("path", "");
            metaFile.set("data", "");
        }
    };

    $scope.doOpenDocument = function (id) {
        $ionicLoading.show({
            template: 'Chargement en cours...'
        });

        var success = function () {
            $ionicLoading.hide();
        };
        var error = function () {
            $ionicLoading.hide();
            $rootScope.PopupHelper.show("Une erreur est survenue lors du chargement du document.");
        };
        var current = $rootScope.myTakeDoc.Metadatas.where({ id: id });
        if (current.length > 0) {
            if ($rootScope.isApp == false) fileHelper.readFileUrl(current[0].get("id"), current[0].get("entityId"), success, error);
            else fileHelper.download(current[0].get("id"), current[0].get("entityId"), "medatafile", current[0].get("file").get("mimeType"), success, error);
        }
    };

    $scope.doSave = function (startWorkflow) {
        $scope.$on("metadata$refreshPage", fRefresh);

        var success = function () {
            $ionicLoading.hide();
            $location.path($scope.nextUrl.replace("#/", ""));
            $scope.$broadcast("metadata$refreshPage");
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].message != null) ? arguments[0].message : arguments[0].responseJSON.Message;
            $rootScope.PopupHelper.show("Informations", msg);
        };

        var fn = function () {
            if (arguments[0] === "Ok") {
                $ionicLoading.show({
                    template: 'Enregistrement...'
                });

                $rootScope.myTakeDoc.Metadatas.save({
                    userId: $rootScope.User.Id,
                    entityId: $rootScope.myTakeDoc.get("EntityId"),
                    versionId: $rootScope.myTakeDoc.get("DocumentCurrentVersionId"),
                    startWorkflow: startWorkflow
                }, success, error);
            }
        };

        if (startWorkflow) {
            $rootScope.PopupHelper.show("Workflow", "Confirmer l'envoi du document au back-office.", "OkCancel", fn);
        }
        else fn("Ok");
        return false;
    };
}]);
