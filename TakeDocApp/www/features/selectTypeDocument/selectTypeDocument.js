'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    $scope.$on("typeDocument$refreshPage", fRefresh);
    
	$scope.$on("$ionicView.beforeEnter", function (scopes, states) {
	    typeDocumentService.get($rootScope.User.CurrentEntityId, success, error);
		
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;

		$scope.$broadcast('typeDocument$refreshPage');
    });

    $scope.searchTypeDocument = function () {
        return (arguments[0].EtatDeleteData == false);
    };

    var success = function () {
        $scope.TypeDocuments = arguments[0].value;
        var nb = 0;
        $.each($scope.TypeDocuments, function(index, value) {
            if (value.EtatDeleteData == false)nb++;
        });

        if (nb == 0) {
            $rootScope.PopupHelper.show("Type de documents", "Aucun type de document disponible");
            $location.path("menu");
        }
        $scope.$broadcast('typeDocument$refreshPage');
    };

    var error = function () {
        $scope.TypeDocuments = null;
        $rootScope.PopupHelper.show("Type de documents", "La liste des types de document n'est pas disponibles.");
    };
	
    $scope.onChoose = function (typeDocumentId) {
        $.each($scope.TypeDocuments, function (index, value) {
            if (value.TypeDocumentId == typeDocumentId) $rootScope.User.CurrentTypeDocument = value;
        });
    };
}]);

