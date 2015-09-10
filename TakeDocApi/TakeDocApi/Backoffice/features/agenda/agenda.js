'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', function ($scope, $rootScope, $uiCalendarConfig) {

    $("#viewLeft").css("width", "0%");
    $("#viewRight").css("width", "99%");

    var myTypeDocs = new TypeDocuments();
    var myOwnerId = $rootScope.getUser().Id;
    var myEntityId = "55C72E33-8864-4E0E-9BC8-C82378B2BF8C";

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
                $.each($scope.eventSources[2], function (index, value) {
                    if (value.id == event.id) {
                        $scope.eventSources[2][index] = event;
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
            entityId: myEntityId,
            folderTypeId: "FAC8EFBC-001D-4C4B-85EF-8ACDDE1EA724",
            ownerId: myOwnerId,
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
                $scope.eventSources[2].push(arguments[0]);
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
                /*$scope.eventSources[2].push(arguments[0]);
                if (!$scope.$$phase) $scope.$apply();*/
            },
            error: function () {
                $rootScope.showError({ message: "Votre rendez-vous n'a pas été supprimé" });
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

    /* event sources array*/
    $scope.eventSources = [[],
        {
            className: 'gcal-event'
        },[]
    ];

    $scope.doSave = function () {
        update($scope.current, false);
    }

    $scope.doDelete = function () {
        remove($scope.current);
    };

    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + "folder/get/" + $rootScope.getUser().Id,
        beforeSend: requestHelper.beforeSend(),
        success: function () {
            $scope.eventSources[2] = arguments[0];
            if (!$scope.$$phase) $scope.$apply();
        },
        error: function () {
            $rootScope.showError("La liste des événements de votre agenda n'est pas disponible.");
        }
    });

    $scope.entitys = $rootScope.getUser().Entitys;

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $scope.selectedTypeDoc = null;
        loadTypeDocument($scope.selectedEntity.Id);
    };

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

    $scope.doSelectTypeDoc = function () {
        $scope.selectedTypeDoc = this.typeDoc;
    };

    $scope.doValidCreate = function () {

        var data = {
            start: $scope.current.start,
            end: $scope.current.end,
            title: $scope.current.title
        };
        $("#modalSelectTypeDoc").modal("hide");
        create(data);
    }
}]);
