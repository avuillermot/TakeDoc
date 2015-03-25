'use strict';
takeDoc.controller('findDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $location, $ionicLoading) {

    var mode = null;
    var extDocuments = null

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("findDocument$refreshPage", fRefresh);

    var onSuccess = function (collection) {
        extDocuments = collection;
        $scope.documents = extDocuments.models;
        $ionicLoading.hide();
    };
    var onError = function () {
        $ionicLoading.hide();
        $rootScope.PopupHelper.show("Erreur", "Une erreur est survenue lors de la recherche");
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        extDocuments = new DocumentsExtended();
        $scope.documents = null;
    });
    
    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        $ionicLoading.show({
            template: 'Recherche...'
        });
        var params = {
            entityReference: $rootScope.User.CurrentEntity.Reference,
            typeDocumentReference: $rootScope.User.CurrentTypeDocument.TypeDocumentReference,
            success: onSuccess,
            error: onError
        }

        var mode = $rootScope.urlParam("search");
        if (mode === "complete") extDocuments.loadComplete(params);
        else if (mode === "incomplete") extDocuments.loadIncomplete(params);
        else if (mode === "last") extDocuments.loadLast(params);

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

    $scope.openDocument = function (docRef, entityRef) {
        var current = extDocuments.where({ reference: docRef, entityReference: entityRef });
        if (current.length > 0) {
            if (current[0].get("statusReference") == "DATA_SEND") {
                var onSuccess = function () {
                    var step = $rootScope.Scenario.start("detailIncomplet");
                    $location.path(step.to.substr(2));
                    $scope.$broadcast("findDocument$refreshPage");
                };
                var onError = function() {

                };
                $rootScope.myTakeDoc = new TkDocument();
                $rootScope.myTakeDoc.set("DocumentCurrentVersionId", current[0].get("versionId"));
                $rootScope.myTakeDoc.set("EntityId", current[0].get("entityId"));
                $rootScope.myTakeDoc.set("UserUpdateData", $rootScope.User.Id);
                documentService.getMetaData($rootScope.myTakeDoc, onSuccess, onError);
            }
            else {
                alert("lock");
            }
        }
    };
}]);