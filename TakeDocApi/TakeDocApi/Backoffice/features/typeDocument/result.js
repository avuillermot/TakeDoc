'use strict';
backOffice.controller('resultTypeDocumentController', ['$scope', '$rootScope', '$location', 'typeDocumentResult', function ($scope, $rootScope, $location, typeDocumentResult) {

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

    $scope.deleteMe = function () {
        var toDel = arguments[0].entity;
        alert(toDel.id);
    };

    $scope.gridResultSearchTypeDoc = {
       enableSorting: true,
       columnDefs: [
          { name: ' ', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-pencil" ng-click="grid.appScope.showMe(row)"></button>&#160;&#160;&#160;&#160;&#160;<button class="btn btn-danger btn-xs glyphicon glyphicon-remove" ng-click="grid.appScope.deleteMe(row)"></button>' },
          { name: 'Libelle', field: 'attributes.label' },
          { name: 'Photographie requise', field: '', cellTemplate: '<input type="checkbox" ng-checked="grid.appScope.pageNeed(row)" onclick="return false"/>' }
       ],
       data: []
    };

}]);