'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

	$scope.$on("$ionicView.beforeEnter", function (scopes, states) {
		typeDocumentService.get($rootScope.User.CurrentEntity, success, error);
        $rootScope.documentToAdd = new documents();
        $rootScope.documentToAdd.DocumentLabel = "";
        $rootScope.documentToAdd.EntityId = $rootScope.User.CurrentEntity;
		$rootScope.documentToAdd.UserCreateData = $rootScope.User.Id;
		
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
    });

    $scope.searchTypeDocument = function () {
        return (arguments[0].EtatDeleteData == false);
    };

    var success = function () {
        $scope.TypeDocuments = arguments[0].value;
        if ($scope.TypeDocuments.length == 0) {
            $rootScope.ErrorHelper.show("Type de documents", arguments[0].responseJSON.Message);
            $location.path("menu");
		}
    };

    var error = function () {
        $scope.TypeDocuments = null;
        $rootScope.ErrorHelper.show("Type de documents", "La liste des types de document n'est pas disponibles.");
    };
	
	$scope.onChoose = function (typeDocumentId) {
        $rootScope.documentToAdd.DocumentTypeId = typeDocumentId;
    };
}]);

