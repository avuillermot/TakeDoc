'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', 'takePictureService', function ($scope, $rootScope, takePictureService) {

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
}]);



takeDoc.service('takePictureService', ['$http', '$rootScope', function ($http, $rootScope) {
    var that = this;
    this.onSuccess = function (imageURI) {
        try {
            $rootScope.documentToAdd.Pages.push({ fileURI: imageURI, state: "toAdd" });
        }
        catch (ex) {
            alert(ex.message);
        }
    };

    this.onFail = function () {
        alert("nok");
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
            alert(ex.message);
        }
    }

}]);

