'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.onChoose = function (entityId) {
        $rootScope.User.CurrentEntityId = entityId;
        $location.path($scope.nextUrl.replace("#/", ""));
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.Entitys = $rootScope.User.Entitys;
		
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;

		if ($scope.Entitys.length == 1) {
		    $rootScope.User.CurrentEntityId = $scope.Entitys[0].Id;
		    $location.path($scope.nextUrl.replace("#/", ""));
		}
    });
    
}]);

