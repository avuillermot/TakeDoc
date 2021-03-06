﻿backOffice.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider.state('home', {
        url: "/home",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/home/home.html",
                controller: 'homeController'
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
    $stateProvider.state('find', {
        url: "/find",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewLeft": {
                templateUrl: "features/find/search.html",
                controller: 'searchFindController'
            },
            "viewRight": {
                templateUrl: "features/inbox/display.html",
                controller: 'displayController'
            }
        }
    });

    $stateProvider.state('agenda', {
        url: "/agenda",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewRight": {
                templateUrl: "features/inbox/display.html",
                controller: 'displayController'
            },
            "viewLeft": {
                templateUrl: "features/agenda/agenda.html",
                controller: 'agendaController'
            }
        }
    });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});

backOffice.config(['flowFactoryProvider', function (flowFactoryProvider) {
    flowFactoryProvider.defaults = {
        
    };
    flowFactoryProvider.on('catchAll', function (event) {
        console.log('catchAll', arguments);
    });
    // Can be used with different implementations of Flow.js
    // flowFactoryProvider.factory = fustyFlowFactory;
}]);