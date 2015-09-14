'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', '$timeout', function ($scope, $rootScope, $uiCalendarConfig, $timeout) {

    $("#viewLeft").css("width", "0%");
    $("#viewRight").css("width", "99%");

    var userEntitys = new UserEntitys();
    $scope.agendas = [];
    $scope.agendas.push({
        id: $rootScope.getUser().Id,
        name: $rootScope.getUser().FirstName + " " + $rootScope.getUser().LastName,
        display: true
    });
    $scope.entitys = [];
    /* event sources array*/
    $scope.eventSources = [[]];
    $scope.currentSource = 0;


    var myTypeDocs = new TypeDocuments();
    var myOwnerId = $rootScope.getUser().Id;

    var update = function (event, changeDate) {
        var myUrl = environnement.UrlBase + "folder/set/{userId}"
                .replace("{userId}", myOwnerId);

        var data = {
            id: event.id,
            folderId: event.folderId,
            entityId: event.entityId,
            ownerId: event.ownerId,
            title: event.title,
            start: (changeDate == true) ? event.start : "",
            end: (changeDate == true) ? event.end : ""
        };

        $.ajax({
            type: 'POST',
            data: { '': angular.toJson(data) },
            url: myUrl,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $.each($scope.eventSources[$scope.currentSource], function (index, value) {
                    if (value.id == event.id) {
                        $scope.eventSources[$scope.currentSource][index] = event;
                    }
                });
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError({ message: "Votre rendez-vous n'a pas été modifié" });
            }
        });
    };

    var create = function (event) {
        var myUrl = environnement.UrlBase + "folder/put/{userId}"
                .replace("{userId}", myOwnerId);

        var data = {
            id: null,
            folderId: null,
            entityId: event.entityId,
            folderTypeId: "FAC8EFBC-001D-4C4B-85EF-8ACDDE1EA724",
            ownerId: event.ownerId,
            title: event.title,
            start: event.start.toJSON(),
            end: event.end.toJSON(),
            userCreateId: $rootScope.getUser().Id
        };

        $.ajax({
            type: 'PUT',
            data: { '': angular.toJson(data) },
            url: myUrl,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $scope.eventSources[$scope.currentSource].push(arguments[0]);
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError({ message: "Votre rendez-vous n'a pas été créé" });
            }
        });
    };

    var remove = function (event) {
        var myUrl = environnement.UrlBase + "folder/delete/{folderId}/{userId}/{entityId}"
                .replace("{folderId}", event.folderId)
                .replace("{userId}", $rootScope.getUser().Id)
                .replace("{entityId}", event.entityId);

        $.ajax({
            type: 'DELETE',
            url: myUrl,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                var events = [];
                $.each($scope.eventSources[$scope.currentSource], function (index, value) {
                    if (value.id != event.id) events.push(value);
                });
                $scope.eventSources[$scope.currentSource] = null;
                $scope.eventSources[$scope.currentSource] = events;
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError({ message: "Votre rendez-vous n'a pas été supprimé" });
            }
        });
    };

    var get = function (start, end) {
        var data = {
            agendas: null,
            start: start.toJSON(),
            end: end.toJSON()
        };
        var agendas = []
        $.each($scope.agendas, function (index, value) {
            agendas.push(value.id);
        });

        data.agendas =  angular.toJson(agendas);
        $.ajax({
            type: 'POST',
            data: { '': angular.toJson(data) },
            url: environnement.UrlBase + "folder/get/" + $rootScope.getUser().Id,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $scope.eventSources[$scope.currentSource] = arguments[0];
                if (!$scope.$$phase) $scope.$apply();
            },
            error: function () {
                $rootScope.showError("La liste des événements de votre agenda n'est pas disponible.");
            }
        });
    };

    /* config object */
    $scope.uiConfig = {
        calendar: {
           header: {
                left: 'prev,next today',
                center: 'title',
                right: 'agendaWeek,agendaDay'
            },
            defaultView: 'agendaWeek',
            height: 650,
            selectable: true,
            editable: true,
            minTime: '08:00:00',
            maxTime: '23:00:00',
            slotDuration: '00:15:00',
            lang: 'fr',
            eventClick: function (calEvent, jsEvent, view) {
                $scope.current = calEvent;
            },
            eventResize: function (event, delta, revertFunc, jsEvent, ui, view) {
                update(event, true);
            },
            eventDrop: function(event, delta, revertFunc, jsEvent, ui, view) {
                update(event, true);
            },
            select: function (start, end) {
                $scope.current = {};
                $scope.current.start = start;
                $scope.current.end = end;
                $("#modalSelectTypeDoc").modal("show");
            }
        }
    };

    $scope.doSave = function () {
        update($scope.current, false);
    }

    $scope.doDelete = function () {
        remove($scope.current);
    };

    $scope.doSelectOwner = function () {
        $scope.selectedAgenda = this.agenda;
        var loadEntity = function (agendaId) {
            var onSuccess = function () {
                $.each(arguments[0].models, function (index, value) {

                        for (var i = 0; i < $rootScope.getUser().Entitys.length; i++) {
                            var ok = ($rootScope.getUser().Entitys[i].Id === value.get("id") && value.get("enable") === true);
                            if (ok) {
                                $scope.entitys.push({
                                    id: value.get("id"),
                                    label: value.get("label")
                                });
                                break;
                            }
                        }

                });
                if (!$scope.$$phase) $scope.$apply();
            };

            var onError = function () {
                $rootScope.showError({ message: "Une erreur est survenue lors du chargement des entitées." });
            };

            var param = {
                userId: agendaId,
                success: onSuccess,
                error: onError
            };
            userEntitys.loadByUser(param)
        };
        $scope.entitys.clear();
        $scope.selectedEntity = null;
        $scope.typeDocs = null;
        $scope.selectedTypeDoc = null;
        if (!$scope.$$phase) $scope.$apply();

        loadEntity($scope.selectedAgenda.id);
    };

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $scope.selectedTypeDoc = null;
        var loadTypeDocument = function (entityId) {
            var onSuccess = function () {
                $scope.typeDocs = myTypeDocs.models;
                if ($scope.typeDocs.length > 0) {
                    $scope.selectedTypeDoc = $scope.typeDocs[0];
                }
                if (!$scope.$$phase) $scope.$apply();
            };

            var onError = function () {
                $rootScope.showError({ message: "Une erreur est survenue lors de la préparation de l'écran de recherche." });
            };

            var param = {
                entityId: entityId,
                deleted: false,
                success: onSuccess,
                error: onError
            };

            myTypeDocs.load(param)
        };
        $scope.selectedTypeDoc = null;
        $scope.typeDocs = null;
        loadTypeDocument($scope.selectedEntity.id);
    };

    $scope.doSelectTypeDoc = function () {
        $scope.selectedTypeDoc = this.typeDoc;
    };

    $scope.doValidCreate = function () {
        if ($scope.selectedAgenda.id != null
            && $scope.selectedEntity.id != null
            && $scope.selectedTypeDoc.get("id") != null
            && $scope.current.title != null) {
                var data = {
                    ownerId: $scope.selectedAgenda.id,
                    start: $scope.current.start,
                    end: $scope.current.end,
                    title: $scope.current.title,
                    typeDoc: $scope.selectedTypeDoc.get("id"),
                    entityId: $scope.selectedEntity.id
                };
                $("#modalSelectTypeDoc").modal("hide");
                create(data);
        }
    }

    //*******************************************
    // event next an previous
    //*******************************************
    $scope.$on('$viewContentLoaded', function () {
        var fnEvent = function () {

            $(".fc-prev-button.fc-button").click(function () {
                refreshCalendar();
            });
            $(".fc-next-button.fc-button").click(function () {
                refreshCalendar();
            });

            refreshCalendar();
        };

        $("#calendar").ready(function () { $timeout(fnEvent, 1000); });
    });

    //*******************************************
    // event select agenda
    //*******************************************
    $scope.$watch(function () { return $scope.searchUserId; }, function () {
        doAddAgenda();
    });

    var doAddAgenda = function () {
        if ($scope.searchUserId != null) {
            var exist = false;
            $.each($scope.agendas, function (index, value) {
                if (value.id == $scope.searchUserId) exist = true;
            });

            if (exist == false) {
                var agenda = {
                    id: $scope.searchUserId,
                    name: $scope.searchUserName
                };
                $scope.agendas.push(agenda);
                $scope.searchUserId = null;
                $scope.searchUserName = null;
                
                refreshCalendar();
            }
        }
    };

    $scope.doRemoveAgenda = function () {
        var place = -1;
        $.each($scope.agendas, function (index, value) {
            if (value.id == $scope.searchUserId) place = 1;
        });
        $scope.agendas.splice(place, 1);

        refreshCalendar();
    };

    var refreshCalendar = function () {
        var end = $('#calendar').fullCalendar('getView').intervalEnd;
        var start = $('#calendar').fullCalendar('getView').intervalStart;
        get(start, end);
    };
}]);
