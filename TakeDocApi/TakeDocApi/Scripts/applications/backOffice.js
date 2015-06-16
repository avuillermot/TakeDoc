'use strict';
var backOffice = angular.module("backOffice", ['ui.router', 'ui.grid', 'ui.grid.edit', 'ui.grid.resizeColumns', 'ui.grid.pagination', 'ui.grid.autoResize', 'ui.grid.selection','angularLoad']);

backOffice.run(function ($rootScope, $location, angularLoad) {

    $rootScope.getUser = function () {
        return JSON.parse(localStorage.getItem("TkUser"));
    };
    $rootScope.setUser = function () {
        if (arguments[0] != null) {
            var culture = arguments[0].Culture;
            angularLoad.loadScript('../Scripts/lib/moment/locale/' + culture + '.js').then(function () {
                moment.locale(culture);
            }).catch(function () {
                alert("Culture can't be load");
            });

            localStorage.setItem("TkUser", JSON.stringify(arguments[0]));
        }
    };

    $rootScope.isBackofficeUser = function () {
        if ($rootScope.getUser() == null) return false;
        if ($rootScope.getUser().GroupReference == null) return false;
        return ($rootScope.getUser().GroupReference == "ADMIN" || $rootScope.getUser().GroupReference == "BACKOFFICE")
    };

    $rootScope.showLoader = function () {
        var msg = "Traitement en cours....";
        if (arguments[0] != null) msg = arguments[0];;
        $("#span-loader-message").html(msg);
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

    $rootScope.showError = function (err) {
        if (err == null || err.responseJSON == null || err.responseJSON.Message == null) {
            if (err.message != null) $rootScope.showModal("Erreur", err.message)
            else if (typeof (err.responseJSON)) $rootScope.showModal("Erreur", err.responseJSON);
            else $rootScope.showModal("Erreur", "Une erreur est survenue.")
        }
        else $rootScope.showModal("Erreur", err.responseJSON.Message);
    }

    $rootScope.$on("$viewContentLoaded", function (scopes) {
        $rootScope.hideLoader();

        if ($location.$$path != "/login") {
            if ($rootScope.getUser() == null) $location.path("#/login");
        }

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
});

backOffice.directive('tkDate', function ($rootScope) {
    var def = {
        restrict: 'A',
        terminal: true,
        transclude: false,
        link: function (scope, element, attrs) {
            var cDate = scope.$eval(attrs.tkDate);
            if (cDate == null || cDate == "") return "";
            try {
                element[0].innerHTML = moment(cDate).format('llll');
            }
            catch (ex) {
                element[0].innerHTML = cDate;
            }
        }
    };
    return def;
});


backOffice.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.bind('click', function () {
            scope.$apply(function () {
                $rootScope.setUser(null);
                $location.path("login");
            });
        });
    };
});

backOffice.directive('tkAutocompleteUsertk', ['$http', '$rootScope', function ($http, $rootScope) {
    return {
        link: function (scope, elem, attr) {
            function highLightData(text, term) {
                var matcher = new RegExp('(' + $.ui.autocomplete.escapeRegex(term) + ')', 'gi');
                return text.replace(matcher, '<strong>$1</strong>');
            }

            // elem is a jquery lite object if jquery is not present, but with jquery and jquery ui, it will be a full jquery object.
            elem.autocomplete({
                source: function (request, response) {
                    var url = environnement.UrlBase + "UserTk/ByName/{CURRENTUSERID}/{VALUE}/{ENTITYID}";
                    url = url.toUpperCase().replace("{CURRENTUSERID}", $rootScope.getUser().Id);
                    url = url.toUpperCase().replace("{VALUE}", scope.searchUserName);
                    if (scope.searchEntityId != null) url = url.toUpperCase().replace("{ENTITYID}", scope.searchEntityId);
                    else url = url.toUpperCase().replace("{ENTITYID}", "");
                    $http.get(url).success(function (data) {
                        response(data);
                    });
                },
                minLength: 3,
                focus: function (event, ui) {
                    // on ne fait rien au survol de la souris sur les choix de la liste proposée
                    return false;
                },
                select: function (event, ui) {
                    //$('#item-' + scope.$parent.metadata.get("id")).height("");
                    // lors de la sélection d'un choix dans la liste, on affiche le libellé de la carte et on déclenche la recherche
                    scope.searchUserName = ui.item.text;
                    scope.searchUserId = ui.item.key;
                    scope.$apply();
                    return false;
                },
                appendTo: attr.appendTo
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                // set du label pour récupération dans la méthode select
                item.label = item.text;
                var highLighted = highLightData(item.text, scope.searchUserName);

                // construction de l'affichage d'une ligne
                var line = $("<div>").html(highLighted);
                // sortie pour jquery-ui
                return $("<li z-index='999' class='autocomplete-list-choice'>").append("<a>" + $("<div>").append(line).html() + "</a>").appendTo(ul);
            };
        }
    }
}]);

backOffice.directive('tkAutocomplete', ['$http', '$rootScope', function ($http, $rootScope) {
    return {
        link: function (scope, elem, attr) {
        function highLightData(text, term) {
            var matcher = new RegExp('(' + $.ui.autocomplete.escapeRegex(term) + ')', 'gi');
            return text.replace(matcher, '<strong>$1</strong>');
        }

        // elem is a jquery lite object if jquery is not present, but with jquery and jquery ui, it will be a full jquery object.
        elem.autocomplete({
            source: function (request, response) {
                    var url = environnement.UrlBase + scope.$parent.metadata.get("autoCompleteUrl");
                    url = url.toUpperCase().replace("<ENTITYID/>", scope.$parent.metadata.get("entityId"));
                    url = url.toUpperCase().replace("<USERID/>", $rootScope.getUser().Id);
                    url = url.toUpperCase().replace("<VALUE/>", scope.$parent.metadata.get("value"));
                    $http.get(url).success(function (data) {
                        response(data);
                    });
            },
            minLength: 3,
            focus: function (event, ui) {
                // on ne fait rien au survol de la souris sur les choix de la liste proposée
                return false;
            },
            select: function (event, ui) {
                $('#item-' + scope.$parent.metadata.get("id")).height("");
                // lors de la sélection d'un choix dans la liste, on affiche le libellé et on déclenche la recherche
                scope.$parent.metadata.set("value", ui.item.text);
                scope.$apply();
                return false;
            },
            appendTo: attr.appendTo
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            // set du label pour récupération dans la méthode select
            item.label = item.text;
            var highLighted = highLightData(item.text, scope.$parent.metadata.get("value"));

            // construction de l'affichage d'une ligne
            var line = $("<div>").html(highLighted);
            // sortie pour jquery-ui
            return $("<li z-index='999' class='autocomplete-list-choice'>").append("<a>" + $("<div>").append(line).html() + "</a>").appendTo(ul);
        };
    }
}
}]);

/* contain result of user search */
backOffice.factory('usersResult', function () {
    var data = { users: [], calls: 0 };

    return {
        set: function () {
            data.users = arguments[0];
            data.calls = data.calls + 1;
        },
        data: data
    }
});

/* contain all user group */
backOffice.factory('groups', function () {
    var data = { groups: null, calls: 0 };

    var groups = new GroupTks();
    var success = function () {
        data.groups = arguments[0];
        data.calls = data.calls + 1;
    };
    var error = function()
    {
        alert("Error in factory groups");
    };
    groups.loadAll({success: success, error: error});

    return {
        data: data
    }
});

backOffice.factory('inputTypes', function () {
    var data = { inputTypes: [], calls: 0 };

    var types = new DataFieldTypes();
    var success = function () {
        data.inputTypes = arguments[0];
        data.calls = data.calls + 1;
    };
    var error = function () {
        alert("Error in factory inputTypes");
    };
    types.load({ success: success, error: error });

    return {
        data: data
    }
});

backOffice.factory('documentDisplay', function () {
    var data = { document: null, metadatas: [], calls: 0 };

    return {
        set: function () {
            data.document = arguments[0];
            data.metadatas = arguments[1];
            data.viewType = arguments[2];
            data.calls = data.calls + 1;
        },
        data: data
    }

    return {
        data: data
    }
});

/* contain document display in detail in inbox */
backOffice.factory('documentsDirectory', function () {
    var data = { documents: null, calls: 0 };

    return {
        set: function () {
            data.documents = arguments[0];
            data.calls = data.calls + 1;
        },
        data: data
    }

    return {
        data: data
    }
});

/* contain type document result after search */
backOffice.factory('typeDocumentResult', function () {
    var data = { typeDocuments: null, calls: 0 };

    return {
        set: function () {
            data.typeDocuments = arguments[0];
            data.calls = data.calls + 1;
        },
        data: data
    }

    return {
        data: data
    }
});

/* contain cpt for refreh detail view */
backOffice.factory('refreshDetail', function () {
    var data = { calls: 0 };
    
    return {
        data: data
    }
});