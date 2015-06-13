'use strict';
var takeDoc = angular.module("takeDoc", ['ionic', 'ngRoute']);

takeDoc.run(function ($rootScope, $ionicPlatform, $ionicPopup, $location, $ionicLoading, $timeout) {
    
    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $ionicLoading.hide();

        if ($location.$$path != "/login") {
            if ($rootScope.User == null) $location.path("#/login");
        }
    });

    $rootScope.isApp = environnement.isApp;
    $rootScope.PopupHelper = new popupHelper($ionicPopup, $rootScope);
    $rootScope.Scenario = new scenario();
    $rootScope.Dashboards = new Dashboards();

    var scenarioAddDocument = [
        { from: "#/menu", to: "#/selectEntity/status/" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/CREATE/status/" },
        { from: "#/selectTypeDocument", to: "#/createDocument" },
        { from: "#/createDocument", to: "#/takePicture" },
        //{ from: "#/takePicture", to: "#/autocomplete" },
        { from: "#/takePicture", to: "#/metadata/mode/ADD" },
        { from: "#/metadata", to: "#/menu" }
    ];
    var scenarioFindIncomplet = [
        { from: "#/menu", to: "#/selectEntity/status/INCOMPLETE" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/INCOMPLETE" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/INCOMPLETE" },
        { from: "#/findDocument", to: "#/menu" }
        ];
    var scenarioFindComplet = [
        { from: "#/menu", to: "#/selectEntity/status/COMPLETE" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/COMPLETE" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/COMPLETE" },
        { from: "#/findDocument", to: "#/menu" }
    ];

    var scenarioFindWait = [
        { from: "#/menu", to: "#/selectEntity/status/TO_VALIDATE" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/TO_VALIDATE" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/TO_VALIDATE" },
        { from: "#/findDocument", to: "#/menu" }
    ];

    var scenarioFindApprove = [
            { from: "#/menu", to: "#/selectEntity/status/APPROVE" },
            { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/APPROVE" },
            { from: "#/selectTypeDocument", to: "#/findDocument/search/APPROVE" },
            { from: "#/findDocument", to: "#/menu" }
    ];

    var scenarioFindRefuse = [
                { from: "#/menu", to: "#/selectEntity/status/REFUSE" },
                { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/REFUSE" },
                { from: "#/selectTypeDocument", to: "#/findDocument/search/REFUSE" },
                { from: "#/findDocument", to: "#/menu" }
    ];

    var scenarioDetailMetadata = [
        { from: "#/findDocument", to: "#/metadata/mode/UPDATE" },
        { from: "#/metadata", to: "#/menu" }
    ];

    $rootScope.Scenario.init("addDocument", scenarioAddDocument);
    $rootScope.Scenario.init("findComplet", scenarioFindComplet);
    $rootScope.Scenario.init("findIncomplet", scenarioFindIncomplet);
    $rootScope.Scenario.init("findWait", scenarioFindWait);
    $rootScope.Scenario.init("findApprove", scenarioFindApprove);
    $rootScope.Scenario.init("findRefuse", scenarioFindRefuse);
    $rootScope.Scenario.init("detailMetadata", scenarioDetailMetadata);
    
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
        .state('selectEntity', _routeHelper.get("selectEntity", false, "/status/:status"))
        .state('selectTypeDocument', _routeHelper.get("selectTypeDocument", false, "/mode/:mode/status/:status"))
        .state('profil', _routeHelper.get("profil", false))
        .state('about', _routeHelper.get("about", false))
        .state('metadata', _routeHelper.get("metadata", false, "/mode/:mode"))
        .state('takePicture', _routeHelper.get("takePicture", false))
        .state('menu', _routeHelper.get("menu", false))
        .state('autocomplete', _routeHelper.get("autocomplete", false, "/id/:id"))

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
        var message = "Vos données en cours de modification seront disponibles dans le menu INCOMPLET.";
        attrs.$observe('goBack', function (val) {
            force = (val === "true") ? true : false;
        });

        element.bind('click', function () {
            if (this.attributes["message"] != null) {
                message = this.attributes["message"].value;
            }
            scope.$apply(function () {
                if (force) $location.path("menu");
                else {
                    var onTap = function () {
                        if (arguments[0] === "Ok") {
                            $location.path("menu");
                        }
                    };
                    $rootScope.PopupHelper.show("Annulation", message, "OkCancel", onTap);
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



