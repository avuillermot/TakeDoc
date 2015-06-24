'use strict';
backOffice.controller('searchFindController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    var myTypeDocs = new TypeDocuments();
    $scope.entitys = $rootScope.getUser().Entitys;

    var loadField = function (entityId) {
        var onSuccess = function () {
            $scope.typeDocs = myTypeDocs.models;
            if ($scope.typeDocs.length > 0) $scope.selectedTypeDoc = $scope.typeDocs[0];
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
        loadField($scope.selectedEntity.Id);
    };

    $scope.doSelectTypeDoc = function () {
        $scope.selectedTypeDoc = this.typeDoc;
    };

    if ($scope.selectedEntity == null) {
        $scope.selectedEntity = $scope.entitys[0];
        loadField($scope.selectedEntity.Id);
    };
    
}]);
