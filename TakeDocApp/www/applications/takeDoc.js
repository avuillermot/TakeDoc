'use strict';
var takeDoc = angular.module("takeDoc", ['ionic', 'ngRoute']);

takeDoc.run(function ($rootScope, $ionicPlatform, $ionicPopup, $location, $ionicLoading) {
    
    $rootScope.$on("$ionicView.beforeEnter", function (scopes, states) {

        $ionicLoading.hide();

        if ($location.$$path != "/login") {
            if ($rootScope.User == null) $location.path("#/login");
        }
    });

    $rootScope.isApp = true;
    $rootScope.PopupHelper = new popupHelper($ionicPopup, $rootScope);
    $rootScope.Scenario = new scenario();
    $rootScope.Dashboards = new Dashboards();

    var scenarioAddDocument = [
        { from: "#/menu", to: "#/selectEntity/status/" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/CREATE/status/" },
        { from: "#/selectTypeDocument", to: "#/createDocument" },
        { from: "#/createDocument", to: "#/takePicture" },
        //{ from: "#/takePicture", to: "#/autocomplete" },
        { from: "#/takePicture", to: "#/metadata/mode/add" },
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
    var scenarioFindSend = [
        { from: "#/menu", to: "#/selectEntity/status/TO_VALIDATE" },
        { from: "#/selectEntity", to: "#/selectTypeDocument/mode/SEARCH/status/TO_VALIDATE" },
        { from: "#/selectTypeDocument", to: "#/findDocument/search/TO_VALIDATE" },
        { from: "#/findDocument", to: "#/menu" }
    ];
    var scenarioDetailIncomplet = [
        { from: "#/findDocument", to: "#/metadata/mode/UPDATE" },
        //{ from: "#/autocomplete", to: "#/metadata/mode/UPDATE" },
        { from: "#/metadata", to: "#/menu" }
    ];

    $rootScope.Scenario.init("addDocument", scenarioAddDocument);
    $rootScope.Scenario.init("findDocument", scenarioFindComplet);
    $rootScope.Scenario.init("findIncomplet", scenarioFindIncomplet);
    $rootScope.Scenario.init("findSend", scenarioFindSend);
    $rootScope.Scenario.init("detailIncomplet", scenarioDetailIncomplet);
    
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

takeDoc.directive('tdAutocomplete', ['$http', '$rootScope', function ($http, $rootScope) {
    return {
        link: function (scope, elem, attr) {
            function highLightData(text, term) {
                var matcher = new RegExp('(' + $.ui.autocomplete.escapeRegex(term) + ')', 'gi');
                return text.replace(matcher, '<strong>$1</strong>');
            }

            // elem is a jquery lite object if jquery is not present, but with jquery and jquery ui, it will be a full jquery object.
            elem.autocomplete({
                source: function (request, response) {
                    if (scope.$parent.metadata.get("value").length > 3) {
                        var url = environnement.UrlBase + scope.$parent.metadata.get("autoCompleteUrl");
                        url = url.toUpperCase().replace("<ENTITYID/>", $rootScope.User.CurrentEntity.Id);
                        url = url.toUpperCase().replace("<USERID/>", $rootScope.User.Id);
                        url = url.toUpperCase().replace("<VALUE/>", scope.$parent.metadata.get("value"));
                        $http.get(url).success(function (data) {
                            response(data);
                        });
                    }
                },
                focus: function (event, ui) {
                    // on ne fait rien au survol de la souris sur les choix de la liste proposée
                    return false;
                },
                select: function (event, ui) {
                    $('#item-' + scope.$parent.metadata.get("id")).height("");
                    // lors de la sélection d'un choix dans la liste, on affiche le libellé de la carte et on déclenche la recherche
                    scope.card = ui.item.label;
                    scope.$apply();
                    return false;
                },
                appendTo: attr.appendTo

            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                // set du label pour récupération dans la méthode select
                item.label = item.text;
                // nom de carte highlighted
                var cardNameHighlighted = highLightData(item.text, scope.$parent.metadata.get("value"));

                // construction de l'affichage d'une ligne
                var cardLine = $("<div>").html(cardNameHighlighted);
                // sortie pour jquery-ui
                return $("<li z-index='999'>").append("<a>" + $("<div>").append(cardLine).html() + "</a>").appendTo(ul);
            };
        }
    }
}]);



