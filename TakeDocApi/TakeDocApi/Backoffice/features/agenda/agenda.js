'use strict';
backOffice.controller('agendaController', ['$scope', '$rootScope', 'uiCalendarConfig', function ($scope, $rootScope, $uiCalendarConfig) {

    $("#viewLeft").css("width", "0%");
    $("#viewRight").css("width", "99%");

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    /* event source that contains custom events on the scope */
    var events = [
      { title: 'All Day Event', start: new Date(y, m, 1), backgroundColor: 'red' },
      { title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
      { id: 999, title: 'Repeating Event', start: new Date(y, m, d - 3, 16, 0), allDay: false },
      { id: 999, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: false },
      { title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false }
    ];

    /* Change View */
    $scope.changeView = function (view, calendar) {
        uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
    };
    /* Change View */
    $scope.renderCalender = function (calendar) {
        if (uiCalendarConfig.calendars[calendar]) {
            uiCalendarConfig.calendars[calendar].fullCalendar('render');
        }
    };

    /* config object */
    $scope.uiConfig = {
        calendar: {
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            height: 650,
            selectable: true,
            //editable: true,
            businessHours: true,
            lang: 'fr',
            eventRender: function (event, el) {
                // render the timezone offset below the event title
                if (event.start.hasZone()) {
                    var end = (event.end == null) ? "" : (" - " + event.end.format('hh:mm'));

                    el.find('.fc-title').after(
                        $('<div class="tzo"/>').text(event.start.format('hh:mm') + end)
                    );
                }
            },            eventClick: function (calEvent, jsEvent, view) {
                $scope.current = calEvent;
            }        }
    };
    /* event sources array*/
    $scope.eventSources = [events];
}]);
