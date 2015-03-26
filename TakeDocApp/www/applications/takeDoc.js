﻿'use strict';
var takeDoc = angular.module("takeDoc", ['ionic']);

takeDoc.run(function ($rootScope, $ionicPlatform, $ionicPopup, $location, $ionicLoading) {
    
    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {

        $ionicLoading.hide();

        if ($location.$$path != "/login") {
            if ($rootScope.User == null) $location.path("#/login");
            else if ($rootScope.User.IsLog == false) $location.path("#/login");
        }
    });

    $rootScope.isApp = false;
    $rootScope.PopupHelper = new popupHelper($ionicPopup, $rootScope);
    $rootScope.Scenario = new scenario();

    var scenarioAddDocument = [
        { from: "#/menu", to: "#/selectEntity" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/create" },
        { from: "#/selectTypeDocument", to: "#/createDocument" },
        { from: "#/createDocument", to: "#/takePicture" },
        { from: "#/takePicture", to: "#/autocomplete" },
        { from: "#/autocomplete", to: "#/metadata/mode/add" },
        { from: "#/metadata", to: "#/menu" }
    ];
    var scenarioFindComplet = [
        { from: "#/menu", to: "#/selectEntity" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/search" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/complete" },
        { from: "#/findDocument", to: "#/menu" }
    ];
    var scenarioFindIncomplet = [
        { from: "#/menu", to: "#/selectEntity" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/search" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/incomplete" },
        { from: "#/findDocument", to: "#/menu" }
    ];
    var scenarioFindLast = [
        { from: "#/menu", to: "#/selectEntity" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/search" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/last" },
        { from: "#/findDocument", to: "#/menu" }
    ];
    var scenarioDetailIncomplet = [
        { from: "#/findDocument", to: "#/autocomplete" },
        { from: "#/autocomplete", to: "#/metadata/mode/update" },
        { from: "#/metadata", to: "#/menu" }
    ];

    $rootScope.Scenario.init("addDocument", scenarioAddDocument);
    $rootScope.Scenario.init("findDocument", scenarioFindComplet);
    $rootScope.Scenario.init("findIncomplet", scenarioFindIncomplet);
    $rootScope.Scenario.init("findLast", scenarioFindLast);
    $rootScope.Scenario.init("detailIncomplet", scenarioDetailIncomplet);

    $rootScope.urlParam = function (name) {
        var url = window.location.href;
        var results = new RegExp('[\/' + name + '\/].*\/').exec(url);
        if (results != null) {
            url = url.replace(results[0], "");
            if (url.indexOf('/') > -1) return url.split('/')[0].replace("https:","").replace("http:","");
            else return url.replace("https:", "").replace("http:", "");
        }
        return "";
    };
    
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

    $ionicPlatform.registerBackButtonAction(function () {
        var fn = function () {
            if (arguments[0] == "Ok") navigator.app.exitApp();
        };
        $rootScope.PopupHelper.show("Quitter", "Voulez-vous quitter l'application ?", "OkCancel", fn)
    }, 100);
});

takeDoc.config(function ($stateProvider, $urlRouterProvider) {
    var _routeHelper = new routeHelper();
    $stateProvider
        .state('login', _routeHelper.get("login", false))
        .state('createDocument', _routeHelper.get("createDocument", false))
        .state('selectEntity', _routeHelper.get("selectEntity", false))
        .state('selectTypeDocument', _routeHelper.get("selectTypeDocument", false, "/mode/:mode"))
        .state('profil', _routeHelper.get("profil", false))
        .state('about', _routeHelper.get("about", false))
        .state('metadata', _routeHelper.get("metadata", false, "/mode/:mode"))
        .state('takePicture', _routeHelper.get("takePicture", false))
        .state('menu', _routeHelper.get("menu", false))
        .state('autocomplete', _routeHelper.get("autocomplete", false))

        .state('findDocument', _routeHelper.get("findDocument", false, "/search/:search"));

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

takeDoc.directive('goBack', function ($location, $rootScope) {
    return function (scope, element, attrs) {
        element.addClass("button button-stable ion-home");
        var force = false;
        attrs.$observe('goBack', function (val) {
            force = (val === "true")? true : false;
        });

        element.bind('click', function () {
            scope.$apply(function () {
                if (force) $location.path("menu");
                else {
                    var onTap = function () {
                        if (arguments[0] === "Ok") {
                            $location.path("menu");
                        }
                    };
                    $rootScope.PopupHelper.show("Annulation", "Vos données en cours de modification seront perdues.", "OkCancel", onTap);
                }
            });
        });
    };
});

takeDoc.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.bind('click', function () {
            scope.$apply(function () {
                var onTap = function () {
                    if (arguments[0] === "Ok") {
                        $rootScope.User = null;
                        $location.path("login");
                    }
                };
                $rootScope.PopupHelper.show("Déconnexion", "Vos données en cours de modification seront perdues.", "OkCancel", onTap);
            });
        });
    };
});



