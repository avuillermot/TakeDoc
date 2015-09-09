'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', function ($scope, $rootScope, $uiCalendarConfig) {

    $("#viewLeft").css("width", "0%");
    $("#viewRight").css("width", "99%");

    var update = function (event) {
        var myUrl = environnement.UrlBase + "folder/set/{userId}"
                .replace("{userId}", $rootScope.getUser().Id);

        var data = {
            id: event.id,
            folderId: event.folderId,
            entityId: event.entityId,
            ownerId: event.ownerId,
            title: event.title,
            start: event.start.toJSON(),
            end: event.end.toJSON()
        }

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
    }


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
                update(event);
            },
            eventDrop: function(event, delta, revertFunc, jsEvent, ui, view) {
                update(event);
            },
            select: function (start, end) {
                var data = {
                    /*id: event.id,
                    folderId: event.folderId,
                    entityId: event.entityId,
                    ownerId: event.ownerId,*/
                    title: event.title,
                    start: start.toJSON(),
                    end: end.toJSON()
                };
                $scope.eventSources[2].push(data);
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
        update($scope.current);
    }

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
}]);
