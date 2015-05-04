backOffice.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider.state('home', {
        url: "/home",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/myDocument/myDocument.html",
                controller: 'myDocumentController'
            }
        }
    });
    $stateProvider.state('login', {
        url: "/login",
        views: {
            "viewLeft": {
                templateUrl: "features/welcome/welcome.html"
            },
            "viewRight": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    $stateProvider.state('account', {
        url: "/account/:user",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/account/accountInfo.html",
                controller: 'accountController'
            },
            "viewRight": {
                templateUrl: "features/account/entity.html",
                controller: 'entityController'
            }
        }
    });
    $stateProvider.state('updatePassword', {
        url: "/updatePassword",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/passwordUpdate/passwordUpdate.html",
                controller: 'passwordController'
            }
        }
    });
    $stateProvider.state('searchUsers', {
        url: "/searchUsers",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/users/searchUser.html",
                controller: 'searchUserController'
            },
            "viewRight": {
                templateUrl: "features/users/resultUser.html",
                controller: 'resultUserController'
            }
        }
    });
    $stateProvider.state('inbox', {
        url: "/inbox",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/inbox/inbox.html",
                controller: 'inboxController'
            },
            "viewRight": {
                templateUrl: "features/inbox/display.html",
                controller: 'displayController'
            }
        }
    });
    $stateProvider.state('searchTypeDocument', {
        url: "/searchTypeDocument",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/typedocument/search.html",
                controller: 'searchTypeDocumentController'
            },
            "viewRight": {
                templateUrl: "features/typeDocument/result.html",
                controller: 'resultTypeDocumentController'
            }
        }
    });
    $stateProvider.state('typeDocument', {
        url: "/typeDocument/:typeDocument",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/typedocument/detail.html",
                controller: 'detailTypeDocumentController'
            }
        }
    });


    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});