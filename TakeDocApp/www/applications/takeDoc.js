﻿'use strict';
var takeDoc = angular.module("takeDoc", ['ionic', 'ngRoute']);

takeDoc.run(function ($rootScope, $ionicPlatform, $ionicModal, $location, $ionicLoading) {
    
    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {

        $ionicLoading.hide();

        if ($location.$$path != "/login") {
            if ($rootScope.User == null) $location.path("#/login");
            else if ($rootScope.User.IsLog == false) $location.path("#/login");
        }
    });

    $rootScope.ErrorHelper = new modalHelper($ionicModal, $rootScope, 'error-modal');
    $rootScope.Scenario = new scenario();

    var scenarioAddDocument = [
        { from: "#/menu", to: "#/selectEntity" },
        { from: "#/selectEntity", to: "#/selectTypeDocument" },
        { from: "#/selectTypeDocument", to: "#/createDocument" },
        { from: "#/createDocument", to: "#/takePicture" },
        { from: "#/takePicture", to: "#/metadata" },
        { from: "#/metadata", to: "#/menu" }
    ];

    $rootScope.Scenario.init("addDocument", scenarioAddDocument);
    
    $ionicPlatform.ready(function() {
        // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
        // for form inputs)
        if (window.cordova && window.cordova.plugins.Keyboard) {
            cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
        }
        if (window.StatusBar) {
            // org.apache.cordova.statusbar required
            StatusBar.styleDefault();
        }
    });
});

takeDoc.config(function ($stateProvider, $urlRouterProvider) {
    var _routeHelper = new routeHelper();
    $stateProvider
        .state('login', _routeHelper.get("login", false))
        .state('createDocument', _routeHelper.get("createDocument", false))
        .state('selectEntity', _routeHelper.get("selectEntity", false))
        .state('selectTypeDocument', _routeHelper.get("selectTypeDocument", false))
        .state('profil', _routeHelper.get("profil", false))
        .state('about', _routeHelper.get("about", false))
        .state('formElement', _routeHelper.get("formElement", false, "/:page"))
        .state('metadata', _routeHelper.get("metadata", false))
        .state('takePicture', _routeHelper.get("takePicture", false))
        .state('menu', _routeHelper.get("menu", false));

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});

takeDoc.directive( 'goClick', function ( $location, $route ) {
    return function (scope, element, attrs) {
        var path;
        element.addClass("button button-stable ion-checkmark-round");
        attrs.$observe( 'goClick', function (val) {
            path = val;
        });

        element.bind('click', function () {
            var isValid = true;
            if (this.attributes["valid-input"] != null) {
                var fn = this.attributes["valid-input"].value;
                isValid = scope.$apply(fn);
            }
            if (isValid) {
                scope.$apply(function () {
                    if (path.substring(0, 2) == "#/") path = path.substring(2);
                    $location.path(path);
                });
            }
        });
    };
});

takeDoc.directive('goBack', function ($location) {
    return function (scope, element, attrs) {
        element.addClass("button button-stable ion-home");
        element.bind('click', function () {
            scope.$apply(function () {
                $location.path("menu");
            });
        });
    };
});

takeDoc.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.bind('click', function () {
            scope.$apply(function () {
                $rootScope.User = null;
                $location.path("login");
            });
        });
    };
});

takeDoc.directive('tdSelect', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        var data = scope.$parent.listValue;
        /*$.each(data.models, function (index, value) {
            element.append($('<option></option>').val(value.attributes.key).html(value.attributes.text));
        });*/
    };
});



