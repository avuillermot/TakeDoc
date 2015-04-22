﻿'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, documentDisplay, documentsDirectory) {

    var pages = new Pages();

    // subscribe to event for display the current document
    $scope.$watch(function () { return documentDisplay.data.calls; }, function () {
        $rootScope.hideLoader();
        if (documentDisplay.data.metadatas.length > 0 && documentDisplay.data.document != null) {
            $scope.document = documentDisplay.data.document;
            $scope.metadatas = documentDisplay.data.metadatas.models;
            $scope.title = $scope.document.get("label");

            loadImage();
        }
        else {
            $scope.document = {};
            $scope.metadatas = [];
            $scope.title = null;

        }
    });

    // display pdf of this document
    $scope.doOpenDocument = function () {
        var success = function () {

        };
        var error = function () {
            $rootScope.showError("Impossible d'ouvrir le document.")
        };

        fileHelper.readUrl(documentDisplay.data.document.get("versionId"), documentDisplay.data.document.get("entityId"), success, error);
    };

    // remove this document
    $scope.doRemove = function () {
        var documentsExt = new DocumentsExtended();
        var param = {
            documentId: $scope.document.get("id"),
            entityId: $scope.document.get("entityId"),
            userId: $rootScope.getUser().Id,
            success: function () {
                $rootScope.hideLoader();
                documentsDirectory.data.documents.remove(documentDisplay.data.document);
                documentsDirectory.data.calls = documentsDirectory.data.calls + 1;

                documentDisplay.data.document = null;
                documentDisplay.data.calls = documentDisplay.data.calls + 1;
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.hideLoader();
            }
        };
        $rootScope.showLoader("Suppression....");
        documentsExt.delete(param);
    };

    $scope.doSave = function () {
        var success = function () {
            $rootScope.hideLoader();
            generatePdf();
        };
        var error = function () {
            $rootScope.hideLoader();
            $rootScope.showError(arguments[0]);
        };
        var param = {
            userId: $rootScope.getUser().Id,
            entityId: $scope.document.get("entityId"),
            versionId: $scope.document.get("versionId")
        };
        
        // data field are mapped to the model here because date picker cause problem
        var elemsDate = $('#divInboxDisplay input[type="date"]');
        $.each(elemsDate, function (index, value) {
            var elemMetaId = value.name;
            var elemMetaValue = value.value;
            var current = documentDisplay.data.metadatas.where({ id: elemMetaId });
            current[0].set("dateValue", elemMetaValue);
        });

        $rootScope.showLoader("Enregistrement....");
        documentDisplay.data.metadatas.save(param, success, error);
    };

    var generatePdf = function () {
        var param = {
            userId: $rootScope.getUser().Id,
            entityId: $scope.document.get("entityId"),
            versionId: $scope.document.get("versionId"),
            success: function () { },
            error: function () {
                $rootScope.showError("La générartion du PDF est en erreur");
            }
        };
        
        documentDisplay.data.metadatas.generatePdf(param);
    };

    var loadImage = function () {
        var success = function () {
            $scope.pages = arguments[0];
            if (!$scope.$$phase) $scope.$apply();
        };

        var param = {
            userId: $rootScope.getUser().Id,
            entityId: $scope.document.get("entityId"),
            versionId: $scope.document.get("versionId"),
            success: success,
            error: function () {
                $rootScope.showError("Les images ne sont pas disponibles");
            }
        };

        pages.load(param);
    };
}]);