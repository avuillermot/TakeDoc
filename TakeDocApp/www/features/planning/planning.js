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
        var step = $rootScope.Scenario.start("detailMetadataUpdate");
            
        $rootScope.myTakeDoc = new CreateDocumentTk();
        $rootScope.myTakeDoc.set("DocumentCurrentVersionId", current.documentVersionId);
        $rootScope.myTakeDoc.set("EntityId", current.entityId);
        $rootScope.myTakeDoc.set("UserCreateData", $rootScope.User.Id);
        $rootScope.myTakeDoc.set("DocumentPageNeed", ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.get("pageNeed") : true);
        $rootScope.User.CurrentEntity = {
            Id: current.entityId
        };
        $location.path(step.to.substr(2));
        if (!$scope.$$phase) $scope.$apply();
    }
}]);
