'use strict';
takeDoc.controller('uploadController', ['$scope', '$rootScope', '$route', '$location', function ($scope, $rootScope, $route, $location) {

    $scope.doReset = function () {
        $location.path("metadata/mode/UPDATE");
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var id = states.stateParams.id;
        var current = $rootScope.myTakeDoc.Metadatas.where({ id: id });
        if (current.length > 0) $scope.current = current[0].attributes;
        if (!$scope.$$phase) $scope.$apply();
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        if (environnement.isApp == true) {
            var path = 'file:///storage/sdcard0/Download/';
            // Constructor takes FileSelector(elem, path, masks, success, fail, cancel, menu, pathChanged, openFile)
            // Only elem is really required, but you'll have to provide the path sooner or later anyway.
            // If you don't provide a mask *.* will be used
            var fileSelector = new FileSelector($('#browser'), path, 'All|*.*');
            // Mask can be changed later using setMasks method.
            fileSelector.onCancel = function (e) // Fires on the back button
            {
                // Add code for closing the file selector, going one folder back (like below) or something else
                $(fileSelector.elem).find('.fileselector-container .fileselector-item.back').click();
                e.stop(); // prevent other backbutton event listners from firing
                if (!$scope.$$phase) $scope.$apply();
            };
            fileSelector.onSuccess = function (path) {
                var name = path.replace(/^.*[\\\/]/, '');
                // we save only path, file wil be read before save
                $scope.current.value = name;
                $scope.current.file.set("path", path);
                $scope.current.file.set("name", name);
                $location.path("metadata/mode/UPDATE");
                if (!$scope.$$phase) $scope.$apply();
            };
            fileSelector.onPathChanged = function (path) {
                // Each time you change directory this callback will be launched (here we're saving lastpath in local storage)
                localStorage['lastPath'] = path;
                if (!$scope.$$phase) $scope.$apply();
            };
            fileSelector.onFail = function (error) {
                // If something goes wrong this code will be executed
                alert(error.message);
                if (!$scope.$$phase) $scope.$apply();
            };
            // There are also onMenu() and onOpenFile(fileEntry, path) callbacks.
            // First is called when you press menu button, the other when path leads to a file and not a directory.
            // Make the selector load file\directory list from a path (if no path is provided, component will try using previous path)
            fileSelector.open(path);
            if (!$scope.$$phase) $scope.$apply();
            // Directories and files will be alphabetically ordered and directories will be listed before the files.
        }
        else {
            // nothing
        }
    });
}]);
