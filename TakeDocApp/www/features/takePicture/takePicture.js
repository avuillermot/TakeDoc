'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', 'takePictureService', function ($scope, $rootScope, takePictureService) {

    /*$rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {

    });*/

    var step = $rootScope.Scenario.next();
    $scope.nextUrl = step.to;
    $scope.takePicture = function () {
        takePictureService.takePicture();

    };

    $scope.doSave = function () {
        var callback = function (fn, msg) {
            alert(fn + "-" + msg);
        };
        $rootScope.documentToAdd.callback = callback;
        $rootScope.documentToAdd.create();
    };

    var fRefresh = function () {
        $scope.Pages = $rootScope.documentToAdd.Pages;
        $scope.$apply();
    };
    $scope.$on("takePicture$refreshPage", fRefresh);
    
    $scope.Pages = $rootScope.documentToAdd.Pages;
}]);



takeDoc.service('takePictureService', ['$http', '$rootScope', function ($http, $rootScope) {
    var that = this;
    this.onSuccess = function (imageURI) {
        try {
            $rootScope.documentToAdd.Pages.push({ fileURI: imageURI, state: "toAdd", PageNumber: $rootScope.documentToAdd.Pages.length });
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

    this.takePicture = function () {
        try {
            navigator.camera.getPicture(this.onSuccess, this.onFail, 
                { 
                    quality: 100,
                    destinationType : Camera.DestinationType.DATA_URL,
                    sourceType : Camera.PictureSourceType.CAMERA,
                    encodingType: Camera.EncodingType.JPEG,
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

