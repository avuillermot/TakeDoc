'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', function ($scope, $rootScope, $stateParams, documentDisplay) {

    $scope.$watch(function () { return documentDisplay.data.calls; }, function () {
        $rootScope.hideLoader();
        if (documentDisplay.data.metadatas.length > 0) {
            $scope.document = documentDisplay.data.document;
            $scope.metadatas = documentDisplay.data.metadatas.models;
        }
    });
}]);