'use strict';
backOffice.controller('detailTypeDocumentController', ['$scope', '$rootScope', '$location', '$stateParams', 'typeDocumentResult', function ($scope, $rootScope, $location, $stateParams, typeDocumentResult) {
    var typeDocuments = new TypeDocuments();
    var typeValidations = new TypeValidations();

    $scope.validations = typeValidations;

    // if datasource is empty, we load type document
    if (typeDocumentResult.data.typeDocuments != null) {
        $scope.selectedItem = typeDocumentResult.data.typeDocuments.where({ id: $stateParams.typeDocument })[0];
        $scope.selectedValidation = typeValidations.where({ id: $scope.selectedItem.get("typeValidationId") })[0];
    }
    else {
        var param = {
            id: $stateParams.typeDocument,
            success: function () {
                debugger;
                $scope.selectedItem = arguments[0].at(0);
                $scope.selectedValidation = typeValidations.where({ id: $scope.selectedItem.get("typeValidationId") })[0];
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError("Une erreur est survenue lors du chargement du type document.")
            }
        };
        typeDocuments.loadById(param);
    }

    $scope.doSelectValidation = function() {
    }
}]);