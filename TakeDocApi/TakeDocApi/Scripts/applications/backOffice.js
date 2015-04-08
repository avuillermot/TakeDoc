'use strict';
var backOffice = angular.module("backOffice", ['ui.router']);

backOffice.run(function ($rootScope, $location) {

    $rootScope.getUser = function () {
        return JSON.parse(localStorage.getItem("TkUser"));
    };
    $rootScope.setUser = function () {
        localStorage.setItem("TkUser", JSON.stringify(arguments[0]));
    };

    $rootScope.getGroup = function () {
        return JSON.parse(localStorage.getItem("TkGroups"));
    };
    $rootScope.setGroup = function () {
        localStorage.setItem("TkGroups", JSON.stringify(arguments[0]));
    };

   $rootScope.showLoader = function () {
        $("#span-loader-message").html(arguments[0]);
        $(".btn-loader-container").css("display", "block");
    };
    $rootScope.hideLoader = function () {
        $(".btn-loader-container").css("display", "none");
    };

    $rootScope.showModal = function (title, body) {
        $("#myModalLabel").html(title);
        $("#myModalBody").html(body);
        $('#myModal').modal('show');
    };
    $rootScope.hideModal = function () {
    };

    $rootScope.$on("$viewContentLoaded", function (scopes) {
        $rootScope.hideLoader();
        if ($rootScope.getUser() == null) alert("user null");
    });
});

backOffice.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider.state('test', {
        url: "/test",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            },
            "viewDetail": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    $stateProvider.state('home', {
        url: "/home",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/myDocument/myDocument.html",
                controller: 'myDocumentController'
            }
        }
    });
    $stateProvider.state('login', {
        url: "/login",
        views: {
            "viewGrid": {
                templateUrl: "features/welcome/welcome.html"
            },
            "viewDetail": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});

backOffice.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.bind('click', function () {
            scope.$apply(function () {
                $rootScope.User = null;
                $location.path("login");
            });
        });
    };
});





