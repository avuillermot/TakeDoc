'use strict';
backOffice.controller('resultTypeDocumentController', ['$scope', '$rootScope', '$location', 'typeDocumentResult', function ($scope, $rootScope, $location, typeDocumentResult) {

    var typeDocuments = new TypeDocuments();

    $scope.$watch(function () { return typeDocumentResult.data.calls; }, function () {
        $rootScope.hideLoader();
        if (typeDocumentResult.data.typeDocuments != null) {
            $scope.gridResultSearchTypeDoc.data = typeDocumentResult.data.typeDocuments.models;
        }
        resizeGridInbox();
    });

    $scope.showMe = function () {
        var toShow = arguments[0].entity;
        $location.path("/typeDocument/" + toShow.id);
    };

    $scope.pageNeed = function () {
        return arguments[0].entity.get("pageNeed");
    };

    var resizeGridInbox = function () {
        var h = ($(document).height() - 150);
        $("#gridResultSearchTypeDoc").css('height', h + 'px');
    };

    $scope.gridResultSearchTypeDoc = {
       enableSorting: true,
       columnDefs: [
          { name: ' ', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-pencil" ng-click="grid.appScope.showMe(row)"></button>&#160;&#160;&#160;&#160;&#160;<button class="btn btn-danger btn-xs glyphicon glyphicon-remove" ng-hide="row.entity.attributes.deleted" ng-click="grid.appScope.deleteMe(row)"></button>' },
          { name: 'Libelle', field: 'attributes.label' },
          { name: 'Photographie requise', field: '', cellTemplate: '<input type="checkbox" ng-checked="grid.appScope.pageNeed(row)" onclick="return false"/>' }
       ],
       data: []
    };

    //***********************************************
    // delete document type
    //***********************************************
    $scope.deleteMe = function () {
        var toDel = arguments[0].entity;
        var param = {
            typeDocumentId: toDel.get("id"),
            entityId: toDel.get("entityId"),
            userId: $rootScope.getUser().Id,
            always: null,
            success: function () {
                debugger;
                alert("ok");
            },
            error: function () {
                $rootScope.showError({ message: "Erreur lors de la suppression du type de document." });
            }
        }
        typeDocuments.delete(param);
    };
}]);