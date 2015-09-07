'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', function ($scope, $rootScope, $uiCalendarConfig) {

    $("#viewLeft").css("width", "0%");
    $("#viewRight").css("width", "99%");


    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    var events = [];

    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + "folder/get/A90CEA2D-7599-437B-88D3-A5405BE3EF93",
        beforeSend: requestHelper.beforeSend(),
        success: function () {
            $.each(arguments[0], function (index, value) {
                $scope.eventSources[0].push(value);
            });
        },
        error: function () {
            alert("err");
        }
    });

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
            //businessHours: true,
            lang: 'fr',

            eventClick: function (calEvent, jsEvent, view) {
                alert(22);
                $scope.current = calEvent;
            },
            eventResize: function () {
                alert(555);
            },
            eventDrop: function(event, delta, revertFunc, jsEvent, ui, view) {
                alert(11);
            }
        }
    };
    /* event sources array*/
    $scope.eventSources = [events,
        {
            className: 'gcal-event', 
            currentTimezone: 'Europe/Paris'
        }
    ];
}]);
