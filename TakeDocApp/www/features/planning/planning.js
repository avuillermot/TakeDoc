'use strict';
takeDoc.controller('planningController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {
    
    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };

    var get = function () {
        var context = {
            agendas: angular.toJson([{ id: $rootScope.User.Id }]),
            start: moment().utc().hours(0).minutes(0).seconds(0).milliseconds(0),
            end: moment().utc().hours(23).minutes(59).seconds(59).milliseconds(0)
        };
        $.ajax({
            type: 'POST',
            data: { '': angular.toJson(context) },
            url: environnement.UrlBase + "folder/get/" + $rootScope.User.Id,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $scope.folders = arguments[0];
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                alert("Une erreur est survenue lors de l'obtention des agendas.");
            }
        });
    };

    $scope.$on("metadata$refreshPage", fRefresh);
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        /*var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
        $scope.mode = states.stateParams.mode;*/
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        get();
    });


    
}]);
