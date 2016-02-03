'use strict';
takeDoc.controller('planningController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {
    
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
                $.each($scope.folders, function (index, value) {
                    value.startLabel = moment(value.start).format("lll");
                    value.endLabel = moment(value.end).format("lll");
                });
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                alert("Une erreur est survenue lors de l'obtention des agendas.");
            }
        });
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.folders = null;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        get();
    });

    $scope.doOpen = function () {
        var current = this.folder;
        
        var starter = "detailMetadataUpdate";
        var step = $rootScope.Scenario.start(starter);
        var versionId = current.documentVersionId;
        var entityId = current.entityId;
        var go = step.to.substr(2) + versionId + "/entity/" + entityId;
        $location.path(go);
        if (!$scope.$$phase) $scope.$apply();
    }
}]);
