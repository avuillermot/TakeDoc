'use strict';
backOffice.controller('searchTypeDocumentController', ['$scope', '$rootScope', 'typeDocumentResult', function ($scope, $rootScope, typeDocumentResult) {

    var typeDoc = new TypeDocuments();

    var onSuccess = function () {
        $rootScope.hideLoader();

        typeDocumentResult.data.calls = typeDocumentResult.data.calls + 1;
        typeDocumentResult.data.typeDocuments = arguments[0];
        if (!$scope.$$phase) $scope.$apply();
    };

    var onError = function () {
        $rootScope.hideLoader();
    };

    $scope.search = {
        userId: $rootScope.getUser().Id,
        label: null,
        entityId: null,
        success: onSuccess,
        error: onError
    };

    $scope.doReset = function () {
        $scope.selectedEntity = $scope.entitys[0];
        $scope.search.entityId = $scope.entitys[0].Id;
        $scope.search.label = "";
    }

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $scope.search.entityId = this.entity.Id;
        return false;
    };
        
    $scope.doSearch = function () {
        $rootScope.showLoader("Recherche en cours...");
        typeDoc.load($scope.search);
    }

    $scope.entitys = $rootScope.getUser().Entitys;
    $scope.doReset();
}]);