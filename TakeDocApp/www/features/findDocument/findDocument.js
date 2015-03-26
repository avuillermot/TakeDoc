﻿'use strict';
takeDoc.controller('findDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', '$ionicModal', function ($scope, $rootScope, $location, $ionicLoading, $ionicModal) {

    var mode = null;
    var extDocuments = null
    var detailModal = new modalHelper($ionicModal, $scope, 'read-only-metadata');
    var readOnlyMetadata = new ReadOnlyMetadatas();

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
            ownerId: $rootScope.User.Id,
            typeDocumentReference: (($rootScope.User.CurrentTypeDocument.TypeDocumentReference == "") ? null : $rootScope.User.CurrentTypeDocument.TypeDocumentReference),
            success: onSuccess,
            error: onError
        }
        mode = $rootScope.urlParam("search");
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
                var success = function () {
                    $scope.readOnlyMetadatas = arguments[0].models;
                    detailModal.show("Détail", current[0].get("entityLabel") + " / " + current[0].get("typeLabel"));
                };
                var error = function() {
                    $rootScope.PopupHelper.show("Détail", "Le détail n'est pas disponible.");
                };
                readOnlyMetadata.loadBy({ success: success, error: error, versionId: current[0].get("versionId"), entityId: current[0].get("entityId") })
            }
        }
    };
}]);