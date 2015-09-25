'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', '$timeout', 'documentDisplay', function ($scope, $rootScope, uiCalendarConfig, $timeout, documentDisplay) {

    var myDocs = new DocumentsExtended();
    var userEntitys = new UserEntitys();
    $scope.agendas = [];
    var agendasColor = ['blue', 'red', 'green', 'orange', 'pink'];
    $scope.agendas.push({
        id: $rootScope.getUser().Id,
        name: $rootScope.getUser().FirstName + " " + $rootScope.getUser().LastName,
        color: agendasColor[$scope.agendas.length],
        events: [],
        display: true
    });
    $scope.entitys = [];
    $scope.eventSources = [[]];

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
            detail: event.detail,
            start: (changeDate == true) ? event.start : "",
            end: (changeDate == true) ? event.end : ""
        };

        $.ajax({
            type: 'POST',
            data: { '': angular.toJson(data) },
            url: myUrl,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $timeout(get(), 1500);
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
            folderTypeId: event.folderTypeId,
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
                $timeout(get(), 1500);
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
                $timeout(get(), 1500);
            },
            error: function () {
                $rootScope.showError({ message: "Votre rendez-vous n'a pas été supprimé" });
            }
        });
    };

    var get = function () {

        $scope.eventSources[0] =  {
            type: 'POST',
            data: function () { // a function that returns an object
                var start = $('#calendar').fullCalendar('getView').intervalStart;
                var end = $('#calendar').fullCalendar('getView').intervalEnd;

                var data = {
                    agendas: null,
                    start: start.toJSON(),
                    end: end.toJSON()
                };
                var agendas = []
                $.each($scope.agendas, function (index, value) {
                    if (value.display == true) agendas.push({ id: value.id, color: value.color });
                });

                data.agendas = angular.toJson(agendas);
                
                return {'': angular.toJson(data)};
            },
            url: environnement.UrlBase + "folder/get/" + $rootScope.getUser().Id,
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                $("#calendar").fullCalendar("rerenderEvents");
            },
            error: function () {
                alert("Une erreur est survenue lors de l'obtention des agendas.");
            }
        };
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
                $("#modalAddFolder").modal("show");

                $scope.selectedAgenda = null;
                $scope.entitys.clear();
                $scope.selectedEntity = null;
                $scope.folderTypes = null;
                $scope.selectedFolderType = null;
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
        $scope.folderTypes = null;
        $scope.selectedFolderType = null;
        if (!$scope.$$phase) $scope.$apply();

        loadEntity($scope.selectedAgenda.id);
    };

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $scope.selectedFolderType = null;
        var loadTypeDocument = function (entityId) {
            var onSuccess = function () {
                $scope.folderTypes = arguments[0].value;
                if ($scope.folderTypes.length > 0) {
                    $scope.selectedFolderType = $scope.folderTypes[0];
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

            $.ajax({
                type: 'GET',
                url: environnement.UrlBase + "odata/FolderTypes?$filter=EntityId eq guid'" + param.entityId + "' and EtatDeleteData eq false",
                beforeSend: requestHelper.beforeSend(),
                success: onSuccess,
                error: onError
            });
        };
        $scope.selectedFolderType = null;
        $scope.folderTypes = null;
        loadTypeDocument($scope.selectedEntity.id);
    };

    $scope.doSelectFolderType = function () {
        $scope.selectedFolderType = this.folderType;
    };

    $scope.doValidCreate = function () {
        if ($scope.selectedAgenda.id != null
            && $scope.selectedEntity.id != null
            && $scope.selectedFolderType.FolderTypeId != null
            && $scope.current.title != null) {
                var data = {
                    ownerId: $scope.selectedAgenda.id,
                    start: $scope.current.start,
                    end: $scope.current.end,
                    title: $scope.current.title,
                    folderTypeId: $scope.selectedFolderType.FolderTypeId,
                    entityId: $scope.selectedEntity.id
                };
                $("#modalAddFolder").modal("hide");
                create(data);
        }
    }

    //*******************************************
    // event next an previous
    //*******************************************
    $scope.$on('$viewContentLoaded', function () {
        var fnEvent = function () {
            $scope.doDisplayAgenda();
            get();
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
                    name: $scope.searchUserName,
                    color: agendasColor[$scope.agendas.length],
                    events: [],
                    display: true
                };
                $scope.agendas.push(agenda);
                $scope.searchUserId = null;
                $scope.searchUserName = null;

                get(null, null);
             }
        }
    };

    $scope.doDisplay = function (id) {
        get();
    };

    $scope.doRemoveAgenda = function (id) {
        var place = -1;
        $.each($scope.agendas, function (index, value) {
            if (value.id == id) place = index;
        });
        $scope.agendas.splice(place, 1);
        get();
    };


    $scope.doDisplayAgenda = function () {
        $("#viewRight").css("width", "0%");
        $("#viewLeft").css("width", "95%");

        $("#divDetailFolder").show();
        $("#calendar").css("width", "70%");
        $("#divDetailFolder").css("width", "28%");

        $("#divGoBack").hide();
        $("#viewRight").hide();

        documentDisplay.data.document = null;
        documentDisplay.data.calls = documentDisplay.data.calls + 1;
        if (!$scope.$$phase) $scope.$apply();
    }

    $scope.doDisplayDocument = function () {
            var toDisplay = new DocumentExtended();

            toDisplay.set("entityId", $scope.current.entityId);
            toDisplay.set("versionId", $scope.current.documentVersionId);
            toDisplay.set("statusReference", $scope.current.documentStatutReference);
            $("#viewRight").css("width", "47%");
            $("#viewLeft").css("width", "47%");

            $("#divDetailFolder").hide();
            $("#calendar").css("width", "90%");
            $("#divDetailFolder").css("width", "0%");

            $("#divGoBack").show();
            $("#viewRight").show();

            documentDisplay.data.document = toDisplay;
            documentDisplay.data.calls = documentDisplay.data.calls + 1;
            if (!$scope.$$phase) $scope.$apply();
    }

}]);
