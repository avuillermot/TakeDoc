'use strict';
backOffice.controller('searchTypeDocumentController', ['$scope', '$rootScope', '$location', '$timeout', 'typeDocumentResult', function ($scope, $rootScope, $location, $timeout, typeDocumentResult) {

    $scope.showAddTypeDocument = false;
    $scope.entitysList = $rootScope.getUser().Entitys;
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
        $scope.showAddTypeDocument = true;
    }

    $scope.entitys = $rootScope.getUser().Entitys;
    $scope.doReset();

    //***********************************************
    // add new document type
    //***********************************************
    $scope.doOpenModalAddTypeDocument = function () {
        $scope.toAdd = {}
        $scope.toAddEntity = $scope.selectedEntity;
        $("#modalAddTypeDocument").modal("show");
    };

    $scope.doAddTypeDocumentSelectEntity = function () {
        $scope.toAddEntity = this.entity;
    };

    $scope.doAddTypeDocument = function () {
        var param = {
            label: $scope.toAdd.label,
            entityId: $scope.toAddEntity.Id,
            userId: $rootScope.getUser().Id,
            always: null,
            success: function () {
                var id = arguments[0];
                var fn = function () {
                    // display the document type that we just add
                    $location.path("/typeDocument/" + id);
                    if (!$scope.$$phase) $scope.$apply();
                };
                $timeout(fn, 300);
            },
            error: function () {
                $rootScope.showError({message:"Erreur lors de l'ajout du type de document."});
            }
        }
        $("#modalAddTypeDocument").modal("hide");
        typeDoc.insert(param);
    };

    //***********************************************
    // delete new document type
    //***********************************************
}]);