'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$ionicPlatform', '$route', '$location', '$ionicLoading', function ($scope, $rootScope, $ionicPlatform, $route, $location, $ionicLoading) {
    
    var fRefresh = function () {
        if (!$scope.$$phase) {
            try { $scope.$apply(); } catch (ex) { }
        }
    };

    $scope.$on("metadata$refreshPage", fRefresh);
    
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
        $scope.mode = states.stateParams.mode;
        $scope.browse = false;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        if (metas.length == 0) $scope.doSave();
    });

    $scope.doOnFocus = function (id) {
        $location.path("autocomplete/id/" + id);
        if (!$scope.$$phase) $scope.$apply();
    };

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });
        $scope.$on("metadata$refreshPage", fRefresh);

        var success = function () {
            $ionicLoading.hide();
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

    $scope.doBrowse = function () {
        alert(1);
        $scope.browse = true;
        var path = 'file:///storage/';
        // Constructor takes FileSelector(elem, path, masks, success, fail, cancel, menu, pathChanged, openFile)
        // Only elem is really required, but you'll have to provide the path sooner or later anyway.
        // If you don't provide a mask *.* will be used
        var fileSelector = new FileSelector($('#browser'), path, 'Documents (html, txt)|*.htm;*.html;*.txt|All files|*.*');
        // Mask can be changed later using setMasks method.
        fileSelector.onCancel = function (e) // Fires on the back button
        {
            // Add code for closing the file selector, going one folder back (like below) or something else
            $(fileSelector.elem).find('.file-container .item.back').click();
            e.stop(); // prevent other backbutton event listners from firing
        };
        fileSelector.onSuccess = function (path) {
            // If you click on a file, this function will be called with the name of the file
        };
        fileSelector.onPathChanged = function (path) {
            // Each time you change directory this callback will be launched (here we're saving lastpath in local storage)
            localStorage['lastPath'] = path;
        };
        fileSelector.onFail = function (error) {
            // If something goes wrong this code will be executed
            alert(error.message);
        };
        // There are also onMenu() and onOpenFile(fileEntry, path) callbacks.
        // First is called when you press menu button, the other when path leads to a file and not a directory.
        // Make the selector load file\directory list from a path (if no path is provided, component will try using previous path)
        fileSelector.open(path);
        if (!$scope.$$phase) $scope.$apply();
        // Directories and files will be alphabetically ordered and directories will be listed before the files.
    };
}]);
