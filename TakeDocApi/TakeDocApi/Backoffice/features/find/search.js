'use strict';
backOffice.controller('searchFindController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $("#viewRight").css("width", "0%");
    $("#viewRight").hide();
    $("#viewLeft").css("width", "98%");

    var myTypeDocs = new TypeDocuments();
    var myDocs = new DocumentsExtended();
    var myFields = new DocumentFields();
    $scope.entitys = $rootScope.getUser().Entitys;

    var resizeGridInbox = function () {
        var h = ($(document).height() - 200);
        $("#seach-result-items").css('height', h + 'px');
    };

    $scope.doReset = function () {
        $('.search-item-input').val("");
    };

    $scope.doSearch = function () {
        var success = function () {
            $scope.gridDocuments.data = myDocs.models;
            if (!$scope.$$phase) $scope.$apply();
        };
        var error = function () {
            alert("erreur");
        };

        var conditions = new Array();
        $.each(myFields.models, function (index, field) {
            if (field.get("value") != null && field.get("value") != "" && field.get("value") != "undefined") {
                conditions.push({name: field.get("reference"), condition: "start", value: field.get("value")});
            }
        });

        var param = {
            typeDocumentId: $scope.selectedTypeDoc.get("id"),
            userId: $rootScope.getUser().Id,
            entityId: $scope.selectedEntity.Id,
            conditions: conditions,
            success: success,
            error: error
        };

        myDocs.search(param);
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
            resizeGridInbox();
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

    /**************************************************
    ***************************************************
    display/use item list and item to display
    ***************************************************
    ***************************************************/
    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: 'attributes.label', cellClass: "cell-inbox-item", width: 280 },
           { name: 'Type', field: 'attributes.typeLabel', cellClass: "cell-inbox-item", width: 250 },
           { name: 'Proprietaire', field: 'attributes.ownerFullName', cellClass: "cell-inbox-item", width: 110 },
           { name: 'Status', field: 'attributes.statusLabel', cellClass: "cell-inbox-item", width: 110 },
           { name: 'Date', field: 'attributes.formatDate', cellClass: "cell-inbox-item", width: 80 }
        ],
        enableRowHeaderSelection: false,
        enableRowSelection: true,
        multiSelect: false,
        modifierKeysToMultiSelect: false,
        noUnselect: true,
        paginationPageSizes: [20, 50, 100, 500],
        data: []
    };
    
}]);
