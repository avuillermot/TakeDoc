'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {

    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };
    var autocompleteOldValue = "";
    $scope.$on("metadata$refreshPage", fRefresh);
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
        if (metas.length == 0) $scope.doSave();
    });

    $scope.doOnFocus = function (id) {
        $(".metadata-item-list").hide();
        $("ion-footer-bar").hide();
        $('#item-' + id).height($(window).height() - 30);
        $('#item-' + id).show();
        $('#autocomplete-close-' + id).css("display", "inline-block");
        var metadata = $rootScope.myTakeDoc.Metadatas.where({ id: id });
        if (metadata.length > 0) {
            autocompleteOldValue = metadata[0].get("value", "");
        }
        $('#item-' + id).animate({
            scrollTop: 0
        }, 1000);
    };

    $scope.doLostFocus = function (id) {
        $(".metadata-item-list").show();
        $("ion-footer-bar").show();
        $('#item-' + id).height("");
        $('#autocomplete-close-' + id).css("display", "none");

        // set value to origine
        var metadata = $rootScope.myTakeDoc.Metadatas.where({id: id});
        if (metadata.length > 0) {
            //var changed = scope.$parent.metadata.hasChanged();
            metadata[0].set("value", autocompleteOldValue);
        }
    }

    $scope.autocompleteSelected = function (id) {
        $(".metadata-item-list").show();
        $("ion-footer-bar").show();
        $('#item-' + id).height("");
        $('#autocomplete-close-' + id).css("display", "none");
    };

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });

        var success = function () {
            $location.path($scope.nextUrl.replace("#/", ""));
            $scope.$broadcast("metadata$refreshPage");
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].message != null) ? arguments[0].message : arguments[0].responseJSON.Message;
            $rootScope.PopupHelper.show("Informations", msg);
        };

        $rootScope.myTakeDoc.Metadatas.save({
            userId: $rootScope.User.Id,
            entityId: $rootScope.myTakeDoc.get("EntityId"),
            versionId: $rootScope.myTakeDoc.get("DocumentCurrentVersionId")
        }, success, error);
        return false;
    };

}]);
