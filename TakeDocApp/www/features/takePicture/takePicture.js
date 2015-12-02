'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', '$location', '$ionicModal', '$ionicLoading', '$timeout', function ($scope, $rootScope, $location, $ionicModal, $ionicLoading, $timeout) {

    var myDocComplete = new DocumentComplete();

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
                    var data = canvas.toDataURL("image/" + $rootScope.myTakeDoc.get("Extension"));
                    $scope.Pages.add({action: 'ADD', id: "P" + number, base64Image: data, rotation: "000", pageNumber: number + 1 });
                    if (!$scope.$$phase) $scope.$apply();
                };
                img.src = url;
            }
            if (arguments[0].pages.length == 0 && environnement.isApp == false) {
                $scope.Pages = new Pages();
                imageToBase64("img/page1.png", 0);
                imageToBase64("img/page2.png", 1);
                imageToBase64("img/page3.png", 2);
                if (!$scope.$$phase) $scope.$apply();
            }
            else $scope.Pages = arguments[0].pages;
            $rootScope.CurrentDocument = arguments[0];

            $scope.loading = false;
            if (!$scope.$$phase) $scope.$apply();
        }
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        var context = {
            versionId: $rootScope.myTakeDoc.get("DocumentCurrentVersionId"),
            userId: $rootScope.User.Id,
            entityId: $rootScope.myTakeDoc.get("EntityId"),
            success: success,
            error: function () {
                $rootScope.PopupHelper.show("Erreur","Erreur lors du chargement du document.")
            }
        };

        myDocComplete.load(context);
    });

    $scope.takePicture = function () {
        takePictureGo();
    };

    $scope.doSave = function () {
        if ($scope.mode == "READ") {
            $location.path($scope.nextUrl.replace("#/", ""));
            if (!$scope.$$phase) $scope.$apply();
        }
        else {
            var context = {
                versionId: $rootScope.CurrentDocument.document.get("versionId"),
                userId: $rootScope.User.Id,
                entityId: $rootScope.CurrentDocument.document.get("entityId"),
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
            if ($scope.Pages.CountNotDeleted() == 0 && $rootScope.myTakeDoc.get("DocumentPageNeed") == true) {
                $rootScope.PopupHelper.show("Erreur", "Une photographie est obligatoire.");
            }
            else {
                $ionicLoading.show({
                    template: 'Enregistrement...'
                });

                myDocComplete.pages = $scope.Pages;
                myDocComplete.save(context);
            }
        }
    };

    $scope.moveUp = function (id) {
		var size = $scope.Pages.length;
		var page = $scope.Pages.where({ id: id });
		var currentIndex = page[0].get('pageNumber');
		if (currentIndex > 1) {
			var pageToMove = $scope.Pages.where({ pageNumber: currentIndex -1 });
			page[0].set('pageNumber', currentIndex -1);
			pageToMove[0].set('pageNumber', currentIndex);

			page[0].setUpdate();
            pageToMove[0].setUpdate();

			if (!$scope.$$phase) $scope.$apply();
        }
    };

    $scope.moveDown = function (id) {
        var size = $scope.Pages.length;
        var page = $scope.Pages.where({ id: id });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex < size) {
            var pageToMove = $scope.Pages.where({ pageNumber: currentIndex +1 });
            page[0].set('pageNumber', currentIndex +1);
            pageToMove[0].set('pageNumber', currentIndex);

            page[0].setUpdate();
            pageToMove[0].setUpdate();

            if (!$scope.$$phase) $scope.$apply();
        }
    }
    $scope.rotate = function (id) {
        var elem = angular.element("#img-page-" +id);
        var prefix = "take-picture-rotate";
        var r000 = elem.hasClass(prefix + "000");
        var r090 = elem.hasClass(prefix + "090");
        var r180 = elem.hasClass(prefix + "180");
        var r270 = elem.hasClass(prefix + "270");

        elem.removeClass(prefix + "000");
        elem.removeClass(prefix + "090");
        elem.removeClass(prefix + "180");
        elem.removeClass(prefix + "270");

        var rotation = 0;
        if (r000) {
            elem.addClass(prefix + "090");
            rotation = 90;
        }
            else if (r090) {
                elem.addClass(prefix + "180");
                rotation = 180;
            }
            else if (r180) {
                elem.addClass(prefix + "270");
                rotation = 270;
            }
            else if (r270) {
                elem.addClass(prefix + "000");
                rotation = 0;
            }
            var page = $scope.Pages.where({ id: id});
        page[0].set('rotation', rotation);
        page[0].setUpdate();
    }

    $scope.delete = function (id) {
        var size = $scope.Pages.length;
        var page = $scope.Pages.where({ id: id });
        page[0].set('action', 'DELETE');
        $scope.numeroter(1, size);
        if (!$scope.$$phase) $scope.$apply();
    }

    $scope.numeroter = function (startIndex, size) {
        var toDelete = $scope.Pages.where({ action: 'DELETE' });
        $.each(toDelete, function (index, value) {
            value.set('pageNumber', '-1');
        });

		var index = startIndex;
		var nb = startIndex;
		while(index <= size + 10) {
			var page = $scope.Pages.where({ pageNumber: index });
			if (page.length > 0) {
			    page[0].set('pageNumber', nb++);
			    page[0].setUpdate();
		    }
			index++;
		}
	};

	$scope.enlarge = function (id) {
	    var page = $scope.Pages.where({ id: id	})[0];

	    var elem = angular.element("#img-page-" + id);
	    var prefix = "take-picture-rotate";
	    var cssRotation = prefix + "000";
	    if (elem.hasClass(prefix + "090")) cssRotation = prefix + "090";
	    if (elem.hasClass(prefix + "180")) cssRotation = prefix + "180";
	    if (elem.hasClass(prefix + "270")) cssRotation = prefix + "270";

	    var img = "<img style='width:100%;height:100%' class='" + cssRotation + "' src='" +page.get("base64Image") + "' />";
	    enlargePage.show("Page " +page.get("pageNumber") + " - Zoom", img);
	    };

	$scope.pageNotDeleted = function (item) {
        return item.attributes.action != "DELETE";
	};
    
	var takePictureOnSuccess = function (imageURI) {
	    try {
	        var number = $scope.Pages.length + 1;
	        var data = "data:image/png;base64," + imageURI;
	        $scope.Pages.add({ action: 'ADD', id: "P" + number, base64Image: data, rotation: "000", pageNumber: number });
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
