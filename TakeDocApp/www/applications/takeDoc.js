'use strict';
var takeDoc = angular.module("takeDoc", ['ionic','ngRoute']);

takeDoc.run(function ($rootScope, $ionicPlatform, $ionicModal, $location) {
    
    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {
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
        { from: "#/takePicture", to: "#/formElement/page1" },
        { from: "#/formElement", to: "#/menu" }
    ];
    /*var scenarioLogin = [
        { from: "#/login", to: "#/menu" }
    ];*/
    var scenarioTestForm = [
        { from: "#/menu", to: "#/formElement/page1" },
        { from: "#/formElement/page1", to: "#/formElement/page2" },
        { from: "#/formElement/page2", to: "#/menu" }
    ];

    //$rootScope.Scenario.init("login", scenarioLogin);
    $rootScope.Scenario.init("addDocument", scenarioAddDocument);
    $rootScope.Scenario.init("scenarioTestForm", scenarioTestForm);
    
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
        .state('takePicture', _routeHelper.get("takePicture", false))
        .state('menu', _routeHelper.get("menu", false));

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});

takeDoc.directive( 'goClick', function ( $location, $route ) {
    return function ($scope, element, attrs) {
        var path;
        element.addClass("button next");
        attrs.$observe( 'goClick', function (val) {
            path = val;
        });

        element.bind('click', function () {
            var isValid = true;
            if (this.attributes["valid-input"] != null) {
                var fn = this.attributes["valid-input"].value;
                isValid = $scope.$apply(fn);
            }
            if (isValid) {
                $scope.$apply(function () {
                    if (path.substring(0, 2) == "#/") path = path.substring(2);
                    $location.path(path);
                });
            }
        });
    };
});

takeDoc.directive('goBack', function ($location) {
    return function (scope, element, attrs) {
        element.addClass("button home");
        element.bind('click', function () {
            scope.$apply(function () {
                $location.path("menu");
            });
        });
    };
});

takeDoc.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.addClass("button logout");
        element.bind('click', function () {
            scope.$apply(function () {
                $rootScope.User = null;
                $location.path("login");
            });
        });
    };
});



