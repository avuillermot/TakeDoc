'use strict';
backOffice.controller('detailTypeDocumentController', ['$scope', '$rootScope', '$location', '$stateParams', 'typeDocumentResult', function ($scope, $rootScope, $location, $stateParams, typeDocumentResult) {
    var typeDocuments = new TypeDocuments();
    var typeValidations = new TypeValidations();
    var fields = new documentFields();
    var values = new fieldValues();
    var autocompletes = new fieldAutocompletes();

    var loadDocumentField = function (typeDocumentId) {
        var param = {
            id: typeDocumentId,
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement des champs pour ce type document.")
            },
            success: function () {
                $scope.fields = arguments[0];
            }
        };
        fields.load(param)
    };

    $scope.validations = typeValidations;
    $("#viewRight").css("width", "0%");
    $("#viewLeft").css("width", "100%");

    // if datasource is empty, we call api
    if (typeDocumentResult.data.typeDocuments != null) {
        $scope.selectedItem = typeDocumentResult.data.typeDocuments.where({ id: $stateParams.typeDocument })[0];
        $scope.selectedValidation = typeValidations.where({ id: $scope.selectedItem.get("typeValidationId") })[0];
        loadDocumentField($scope.selectedItem.get("id"));
    }
    else {
        var param = {
            id: $stateParams.typeDocument,
            success: function () {
                $scope.selectedItem = arguments[0].at(0);
                $scope.selectedValidation = typeValidations.where({ id: $scope.selectedItem.get("typeValidationId") })[0];
                loadDocumentField($scope.selectedItem.get("id"));
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement du type document.");
            }
        };
        typeDocuments.loadById(param);
    }

    $scope.doSelectValidation = function() {
    }

    $scope.onListValues = function () {
        var title = this.field.get("label");
        var param = {
            id: this.field.get("id"),
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            success: function () {
                $scope.fieldValues = arguments[0];
                $("#modalFieldValues .modal-title").html(title)
                $("#modalFieldValues").modal("show");
            },
            error: null
        };
        values.load(param);
    };

    $scope.onAutocomplete = function (id) {
        debugger;
        var title = this.field.get("label");
        var param = {
            id: this.field.get("autoCompleteId"),
            always: function () {
                if (!$scope.$$phase) $scope.$apply();
            },
            success: function () {
                $scope.fielAutocompletes = arguments[0];
                $("#modalFieldAutocompletes .modal-title").html(title)
                $("#modalFieldAutocompletes").modal("show");
            },
            error: null
        };
        autocompletes.load(param);
    };
}]);