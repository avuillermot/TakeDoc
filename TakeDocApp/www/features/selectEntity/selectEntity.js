'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.onChoose = function (entityId) {
        $rootScope.User.CurrentEntity = entityId;
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.documentToAdd = new documents();
        $rootScope.documentToAdd.Pages = new Pictures();
		
		var step = $rootScope.Scenario.next();

		$scope.nextUrl = step.to;
		$scope.Entitys = $rootScope.User.Entitys;
    });
    
}]);

