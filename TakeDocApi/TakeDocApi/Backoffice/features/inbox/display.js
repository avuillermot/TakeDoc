'use strict';
backOffice.controller('displayController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', function ($scope, $rootScope, $stateParams, documentDisplay) {

    $scope.$watch(function () { return documentDisplay.data.calls; }, function () {
        $rootScope.hideLoader();
        if (documentDisplay.data.metadatas.length > 0) {
            $scope.document = documentDisplay.data.document;
            $scope.metadatas = documentDisplay.data.metadatas.models;
        }
    });

    $scope.doOpenDocument = function () {
        var success = function () {

        };
        var error = function () {
            $rootScope.showError("Impossible d'ouvrir le document.")
        };

        fileHelper.readUrl(documentDisplay.data.document.get("versionId"), documentDisplay.data.document.get("entityId"), success, error);
    };
}]);