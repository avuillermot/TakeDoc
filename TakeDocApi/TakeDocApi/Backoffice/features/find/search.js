'use strict';
backOffice.controller('searchFindController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $("#viewRight").css("width", "0%");
    $("#viewRight").hide();
    $("#viewLeft").css("width", "98%");

    var myTypeDocs = new TypeDocuments();
    var myFields = new DocumentFields();
    $scope.entitys = $rootScope.getUser().Entitys;

    $scope.doReset = function () {
        $('.search-item-input').val("");
    };

    $scope.doSearch = function () {
        var req = "";
        $.each(myFields.models, function (index, field) {
            if (field.get("value") != null && field.get("value") != "" && field.get("value") != "undefined") {
                req = req + field.get("reference") + field.get("value");
            }
        });
        alert(req);
    };

    var loadField = function (typeDoc) {
        var onSucces = function () {
            $scope.fields = myFields.models;
            if (!$scope.$$phase) $scope.$apply();
        };
        var onError = function () {
            $rootScope.showError({ message: "Impossible d'obtenir la liste des champs." });
        };
        var param = {
            id: typeDoc.get("id"),
            success: onSucces,
            error: onError,
            always: null
        };
        myFields.load(param);
    }

    var loadTypeDocument = function (entityId) {
        var onSuccess = function () {
            $scope.typeDocs = myTypeDocs.models;
            if ($scope.typeDocs.length > 0) {
                $scope.selectedTypeDoc = $scope.typeDocs[0];
                loadField($scope.selectedTypeDoc);
            }
            if (!$scope.$$phase) $scope.$apply();
        };

        var onError = function () {
            $rootScope.showError({ message: "Une erreur est survenue lors de la préparation de l'écran de recherche." });
        };

        var param = {
            entityId: entityId,
            deleted: false,
            success: onSuccess,
            error: onError
        };

        myTypeDocs.load(param)
    };

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $scope.selectedTypeDoc = null;
        loadTypeDocument($scope.selectedEntity.Id);
    };

    $scope.doSelectTypeDoc = function () {
        $scope.selectedTypeDoc = this.typeDoc;
        loadField($scope.selectedTypeDoc);
    };

    if ($scope.selectedEntity == null) {
        $scope.selectedEntity = $scope.entitys[0];
        loadTypeDocument($scope.selectedEntity.Id);
    };
    
}]);
