'use strict';
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

        var reference = "";
        if ($rootScope.User.CurrentTypeDocument != null) reference = (($rootScope.User.CurrentTypeDocument.get("reference") == "") ? null : $rootScope.User.CurrentTypeDocument.get("reference"));
        var params = {
            entityReference: $rootScope.User.CurrentEntity.Reference,
            ownerId: $rootScope.User.Id,
            typeDocumentReference: reference,
            success: onSuccess,
            error: onError
        }
        mode = states.stateParams.search;

        if (mode === "COMPLETE") extDocuments.loadComplete(params);
        else if (mode === "INCOMPLETE") extDocuments.loadIncomplete(params);
        else if (mode === "TO_VALIDATE") extDocuments.loadWaitValidate(params);
        else if (mode === "REFUSE") extDocuments.loadRefuse(params);
        else if (mode === "APPROVE") extDocuments.loadApprove(params);
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

    $scope.openMetadata = function (docRef, entityRef) {
        $ionicLoading.show({
            template: 'Chargement en cours...'
        });
        var current = extDocuments.where({ reference: docRef, entityReference: entityRef });
        if (current.length > 0) {
            var onSuccess = function () {
                    var starter = (mode == "INCOMPLETE") ? "detailMetadataUpdate" : "detailMetadataReadOnly";
                    var step = $rootScope.Scenario.start("");
                    step = $rootScope.Scenario.start(starter);
                    $location.path(step.to.substr(2));
                    $scope.$broadcast("findDocument$refreshPage");
                };
                var onError = function() {
                    $rootScope.PopupHelper.show("Une erreur est survenue lors de l'obtention des informations");
                };
                $rootScope.myTakeDoc = new TkDocument();
                $rootScope.myTakeDoc.set("DocumentCurrentVersionId", current[0].get("versionId"));
                $rootScope.myTakeDoc.set("EntityId", current[0].get("entityId"));
                $rootScope.myTakeDoc.set("UserUpdateData", $rootScope.User.Id);
                documentService.getMetaData($rootScope.myTakeDoc, onSuccess, onError);
        }
    };

    $scope.viewDocument = function (docRef, entityRef) {
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
        var current = extDocuments.where({ reference: docRef, entityReference: entityRef });
        if (current.length > 0) {
            if ($rootScope.isApp == false) fileHelper.readDocumentUrl(current[0].get("versionId"), current[0].get("entityId"), success, error);
            else fileHelper.download(current[0].get("versionId"), current[0].get("entityId"), success, error);
        }
    };
}]);