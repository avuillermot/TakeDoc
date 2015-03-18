'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', 'takePictureService', '$location', '$ionicModal', '$ionicLoading', function ($scope, $rootScope, takePictureService, $location, $ionicModal, $ionicLoading) {
    var enlargePage = new modalHelper($ionicModal, $rootScope, 'enlarge-page-modal');

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.documentToAdd.Pages = new Pictures();

        var imageToBase64 = function (url, number) {
            var img = new Image();
            img.onload = function () {
                var canvas = document.createElement('CANVAS');
                var ctx = canvas.getContext('2d');
                ctx.drawImage(this, 0, 0);
                var data = canvas.toDataURL("image/" + $rootScope.documentToAdd.Extension);
                var p = new Picture({ id: "P" + number, imageURI: data, state: "toAdd", pageNumber: number + 1 });
                $rootScope.documentToAdd.Pages.add(p);
                $scope.Pages = $rootScope.documentToAdd.Pages.models;
                $scope.$apply();
            };
            img.src = url;
        }
        
        imageToBase64("img/page1.jpeg",0);
        /*imageToBase64("img/page2.jpeg",1);
        imageToBase64("img/r1.jpeg",2);*/

        $scope.Pages = $rootScope.documentToAdd.Pages.models;
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });


    $scope.takePicture = function () {
        takePictureService.takePicture();
    };

    $scope.doSave = function () {
        $ionicLoading.show({
            template: 'Enregistrement...'
        });

        var success = function () {
            $location.path($scope.nextUrl.replace("#/",""));
			$scope.$apply();
		};
        var error = function (success, error) {
            $ionicLoading.hide();
            $rootScope.ErrorHelper.show("Création", arguments[0].responseJSON.Message);
		};
		try {
			documentService.create($rootScope.documentToAdd, success, error);
		}
        catch (ex) {
            $ionicLoading.hide();
			$rootScope.ErrorHelper.show("Création", ex);
		};
		return false;
    };

    var fRefresh = function () {
        $scope.Pages = $rootScope.documentToAdd.Pages.models;
        $scope.$apply();
    };
    $scope.$on("takePicture$refreshPage", fRefresh);

    $scope.mooveUp = function (id) {
		var size = $rootScope.documentToAdd.Pages.length;
		var page = $rootScope.documentToAdd.Pages.where({ id: id });
		var currentIndex = page[0].get('pageNumber');
		if (currentIndex > 1) {
			var pageToMove = $rootScope.documentToAdd.Pages.where({ pageNumber: currentIndex - 1 });
			page[0].set('pageNumber', currentIndex - 1);
			pageToMove[0].set('pageNumber', currentIndex);
			fRefresh();
		}
    };
    $scope.mooveDown = function (id) {
		var size = $rootScope.documentToAdd.Pages.length;
		var page = $rootScope.documentToAdd.Pages.where({ id: id });
		var currentIndex = page[0].get('pageNumber');
		if (currentIndex < size) {
			var pageToMove = $rootScope.documentToAdd.Pages.where({ pageNumber: currentIndex + 1 });
			page[0].set('pageNumber', currentIndex + 1);
			pageToMove[0].set('pageNumber', currentIndex);
			fRefresh();
		}
    }
    $scope.rotate = function (id) {
        var elem = angular.element("#img-page-" + id);
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

        var page = $rootScope.documentToAdd.Pages.where({ id: id });
        page[0].set('rotation',rotation);
    }

    $scope.delete = function (id) {
		var size = $rootScope.documentToAdd.Pages.length;
        $rootScope.documentToAdd.Pages.remove({ id: id });
		fRefresh();
		$scope.numeroter(1, size);
    }
	
	$scope.numeroter = function(startIndex, size) {
		var index = startIndex;
		var nb = startIndex;
		while(index <= size) {
			var page = $rootScope.documentToAdd.Pages.where({pageNumber: index});
			if (page.length > 0) {
				page[0].set('pageNumber',nb++);
			}
			index++;
		}
	};

	$scope.enlarge = function (id) {
	    var page = $rootScope.documentToAdd.Pages.where({ id: id })[0];


	    var elem = angular.element("#img-page-" + id);
	    var prefix = "take-picture-rotate";
	    var cssRotation = prefix + "000";
	    if (elem.hasClass(prefix + "090")) cssRotation = prefix + "090";
	    if (elem.hasClass(prefix + "180")) cssRotation = prefix + "180";
	    if (elem.hasClass(prefix + "270")) cssRotation = prefix + "270";

	    var img = "<img style='width:100%;height:100%' class='" + cssRotation + "' src='" + page.get("imageURI") + "' />";
	    enlargePage.show("Page " + page.get("pageNumber") + " - Zoom", img);
	};

}]);

takeDoc.service('takePictureService', ['$http', '$rootScope', function ($http, $rootScope) {
    var that = this;

    this.onSuccess = function (imageURI) {
        try {
            var myPageNumber = $rootScope.documentToAdd.Pages.length + 1;
            var data = "data:image/" + $rootScope.documentToAdd.Extension + ";base64," + imageURI;
            var p1 = new Picture({ id: 'P' + myPageNumber, imageURI: data, state: "toAdd", pageNumber: myPageNumber });
            $rootScope.documentToAdd.Pages.add(p1);
        }
        catch (ex) {
            $rootScope.ErrorHelper.show("Camera", ex.message);
        }
        $rootScope.$broadcast('takePicture$refreshPage');
    };

    this.onFail = function () {
        $rootScope.ErrorHelper.show("Camera", "Une erreur est survenue lors de la prise de vue.");
        $rootScope.$broadcast('takePicture$refreshPage');
    };

    this.takePicture = function (index) {
        that.index = index;
        try {
            navigator.camera.getPicture(this.onSuccess, this.onFail, 
                { 
                    quality: 100,
                    destinationType : Camera.DestinationType.DATA_URL,
                    sourceType : Camera.PictureSourceType.CAMERA,
                    encodingType: Camera.EncodingType.PNG,
                    targetHeight: 1000,
                    targetWidth: 750,
                    correctOrientation: true
                });
        }
        catch (ex) {
            $rootScope.ErrorHelper.show("Camera", ex.message);
        }
    }
}]);

