'use strict';

var Picture = Backbone.Model.extend({
	defaults: {
        id: null,
        imageURI: null,
		state: null,
		pageNumber: null,
        rotation: 0
    }
});

 var Pictures = Backbone.Collection.extend({
    model: Picture
 });

takeDoc.controller('takePictureController', ['$scope', '$rootScope', 'takePictureService', '$location', function ($scope, $rootScope, takePictureService, $location) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.documentToAdd.Pages = new Pictures();
		var p1 = new Picture({ id: "P0" ,imageURI: "img/page1.jpeg", state: "toAdd", pageNumber: 1 })
		var p2 = new Picture({ id: "P1", imageURI: "img/page2.jpeg", state: "toAdd", pageNumber: 2 })
		var p3 = new Picture({ id: "P2", imageURI: "img/r1.jpeg", state: "toAdd", pageNumber: 3 })
		$rootScope.documentToAdd.Pages.push(p1);
		$rootScope.documentToAdd.Pages.push(p2);
		$rootScope.documentToAdd.Pages.push(p3);
		
		$scope.Pages = $rootScope.documentToAdd.Pages.models;
		
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
    });


    $scope.takePicture = function () {
        takePictureService.takePicture();
    };

    $scope.doSave = function () {

        var success = function () {
            $location.path($scope.nextUrl);
			$scope.$apply();
		};
        var error = function (success, error) {
			$rootScope.ErrorHelper.show("Création", "Une erreur est survenue lors de la de la création du document.");
		};
		try {
			documentService.create($rootScope.documentToAdd, success, error);
		}
		catch(ex) {
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

}]);

takeDoc.service('takePictureService', ['$http', '$rootScope', function ($http, $rootScope) {
    var that = this;

    this.onSuccess = function (imageURI) {
        try {
            var myPageNumber = $rootScope.documentToAdd.Pages.length + 1;
            var p1 = new Picture({ id: 'P' + myPageNumber, imageURI: imageURI, state: "toAdd", pageNumber: myPageNumber });
            $rootScope.documentToAdd.Pages.push(p1);
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

