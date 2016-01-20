'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', '$location', '$ionicModal', '$ionicLoading', '$timeout', function ($scope, $rootScope, $location, $ionicModal, $ionicLoading, $timeout) {

    $scope.myDoc = new DocumentComplete();

    var enlargePage = new modalHelper($ionicModal, $rootScope, 'enlarge-page-modal');

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {

        $scope.mode = states.stateParams.mode;
        $scope.loading = ($scope.mode != "ADD");
        var success = function () {
            var imageToBase64 = function (url, number) {
                var img = new Image();
                img.onload = function () {
                    var canvas = document.createElement('CANVAS');
                    var ctx = canvas.getContext('2d');
                    ctx.drawImage(this, 0, 0);
                    var data = canvas.toDataURL("image/png");
                    $scope.myDoc.pages.add({action: 'ADD', id: "P" + number, base64Image: data, rotation: "000", pageNumber: number + 1 });
                    if (!$scope.$$phase) $scope.$apply();
                };
                img.src = url;
            }
            if (arguments[0].pages.length == 0 && environnement.isApp == false) {
                $scope.myDoc.pages = new Pages();
                imageToBase64("img/page1.png", 0);
                imageToBase64("img/page2.png", 1);
                imageToBase64("img/page3.png", 2);
                if (!$scope.$$phase) $scope.$apply();
            }
            else $scope.myDoc.pages = arguments[0].pages;
            $rootScope.CurrentDocument = arguments[0];

            $scope.loading = false;
            if (!$scope.$$phase) $scope.$apply();
        }
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        var context = {
            versionId: states.stateParams.versionId,
            userId: $rootScope.User.Id,
            entityId: states.stateParams.entityId,
            success: success,
            error: function () {
                $rootScope.PopupHelper.show("Erreur","Erreur lors du chargement du document.")
            }
        };

        $scope.myDoc.load(context);
    });

    $scope.takePicture = function () {
        takePictureGo();
    };

    /*
    $scope.$on("metadata$refreshPage", fRefresh);

        var success = function () {
            $ionicLoading.hide();
            $location.path($scope.nextUrl.replace("#/", ""));
            $scope.$broadcast("metadata$refreshPage");
        };

        var error = function () {
            $ionicLoading.hide();
            var msg = (arguments[0].responseJSON != null) ? arguments[0].responseJSON : "Une erreur est survenue.";
            $rootScope.PopupHelper.show("Erreur", arguments[0].responseJSON);
        };

        var fn = function () {
            if (arguments[0] === "Ok") {
                $ionicLoading.show({
                    template: 'Enregistrement...'
                });

                $rootScope.CurrentDocument.metadatas.save({
                    userId: $rootScope.User.Id,
                    entityId: $rootScope.myTakeDoc.get("EntityId"),
                    versionId: $rootScope.myTakeDoc.get("DocumentCurrentVersionId"),
                    startWorkflow: startWorkflow
                }, success, error);
            }
        };

        if (startWorkflow) {
            $rootScope.PopupHelper.show("Workflow", "Confirmer l'envoi du document au back-office.", "OkCancel", fn);
        }
        else fn("Ok");
        return false;*/

    $scope.doSave = function () {
        if ($scope.mode == "READ") {
            $location.path($scope.nextUrl.replace("#/", ""));
            if (!$scope.$$phase) $scope.$apply();
        }
        else {
            var context = {
                versionId: $scope.myDoc.document.get("versionId"),
                userId: $rootScope.User.Id,
                entityId: $scope.myDoc.document.get("entityId"),
                startWorkflow: false,
                onlyPage: true,
                success: function () {
                    $ionicLoading.hide();
                    $location.path($scope.nextUrl.replace("#/", ""));
                    if (!$scope.$$phase) $scope.$apply();
                },
                error: function () {
                    $ionicLoading.hide()
                    $rootScope.PopupHelper.show("Erreur", "Une erreur est survenue lors de l'enregistrement.");
                }
            };
            debugger;
            if ($scope.myDoc.pages.CountNotDeleted() == 0 && $scope.myDoc.document.get("pageNeed") == true) {
                $rootScope.PopupHelper.show("Erreur", "Une photographie est obligatoire.");
            }
            else {
                $ionicLoading.show({
                    template: 'Enregistrement...'
                });

                $scope.myDoc.save(context);
            }
        }
    };

    $scope.moveUp = function (id) {
        photo.moveUp($scope.myDoc, id);
        if (!$scope.$$phase) $scope.$apply();
    };

    $scope.moveDown = function (id) {
        photo.moveDown($scope.myDoc, id);
    }
    $scope.rotate = function (id) {
        photo.rotate($scope.myDoc, id);
        if (!$scope.$$phase) $scope.$apply();
    }

    $scope.delete = function (id) {
        photo.delete($scope.myDoc, id);
        if (!$scope.$$phase) $scope.$apply();
    }

    $scope.enlarge = function (id) {
        var data = photo.enlarge($scope.myDoc, id);
        enlargePage.show("Page " + data.pageNumber + " - Zoom", data.img);
    };

	$scope.pageNotDeleted = function (item) {
        return item.attributes.action != "DELETE";
	};
    
	var takePictureOnSuccess = function (imageURI) {
	    try {
	        var number = $scope.myDoc.pages.length + 1;
	        var data = "data:image/png;base64," + imageURI;
	        $scope.myDoc.pages.add({ action: 'ADD', id: "P" + number, base64Image: data, rotation: "000", pageNumber: number });
	    }
	    catch (ex) {
	        $rootScope.PopupHelper.show("Camera", ex.message);
	    }
	    if (!$scope.$$phase) $scope.$apply();
	};

	var takePictureOnFail = function () {
	    if (!$scope.$$phase) $scope.$apply();
	};

	var takePictureGo = function (index) {
	    that.index = index;
	    try {
	        navigator.camera.getPicture(takePictureOnSuccess, takePictureOnFail,
                {
                    quality: 50,
                    destinationType: Camera.DestinationType.DATA_URL,
                    sourceType: Camera.PictureSourceType.CAMERA,
                    encodingType: Camera.EncodingType.PNG,
                    targetHeight: 1000,
                    targetWidth: 750,
                    correctOrientation: true
                });
	    }
	    catch (ex) {
	        $rootScope.PopupHelper.show("Camera", ex.message);
	    }
	}
}]);
