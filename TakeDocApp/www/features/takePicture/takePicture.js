'use strict';
takeDoc.controller('takePictureController', ['$scope', '$rootScope', 'takePictureService', function ($scope, $rootScope, takePictureService) {

    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {
        //alert("test");
    });

    var step = $rootScope.Scenario.next();
    $scope.nextUrl = step.to;
    $scope.takePicture = function () {
        takePictureService.takePicture();
        /*var f = function () {
            //alert($rootScope.documentToAdd.Pages.length);
            $scope.$broadcast('scroll.refreshComplete');
        };
        window.setInterval(f, 5000);*/
    };

    $scope.doSave = function () {
        var callback = function (fn, msg) {
            alert(fn + "-" + msg);
        };
        $rootScope.documentToAdd.callback = callback;
        $rootScope.documentToAdd.create();
    };

    $scope.refresh = function () {
        that.$scope.$broadcast('scroll.refreshComplete');
    }
    
    $scope.Pages = $rootScope.documentToAdd.Pages;
}]);



takeDoc.service('takePictureService', ['$http', '$rootScope', function ($http, $rootScope) {
    var that = this;
    this.onSuccess = function (imageURI) {
        try {
            $rootScope.documentToAdd.Pages.push({ fileURI: imageURI, state: "toAdd" });
        }
        catch (ex) {
            $rootScope.ErrorHelper.show("Camera", ex.message);
        }
        //that.$scope.$broadcast('scroll.refreshComplete');
    };

    this.onFail = function () {
        $rootScope.ErrorHelper.show("Camera", "Une erreur est survenue lors de la prise de vue.");
        //that.$scope.$broadcast('scroll.refreshComplete');
    };

    this.takePicture = function () {
        //that.$scope = $scope;
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

