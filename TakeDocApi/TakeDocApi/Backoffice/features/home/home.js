'use strict';
backOffice.controller('homeController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $("#viewLeft").css("width", "100%");
    var maxHeigtht = 0;

    var dash = new Dashboards();
    $scope.entitys = $rootScope.getUser().Entitys;
    if ($scope.selectedEntity == null) $scope.selectedEntity = $scope.entitys[0];

    var createBar = function (entity) {
        if (dash.countEntity(entity.Id) == 0) return false;
        var data = dash.getBarGraphStatusDataSource(entity.Id);

        $('#bar-'+entity.Id).highcharts({
            chart: {
                type: 'bar',
                height: (data.typesDocument.length * 30) + 200
            },
            title: {
                text: entity.Label
            },
            xAxis: {
                categories: data.typesDocument
            },
            yAxis: {
                min: 0,
                allowDecimals: false,
                maxPadding: 0,
                title: {
                    text: 'Répartition par type et statut'
                }
            },
            legend: {
                reversed: true
            },
            plotOptions: {
                series: {
                    stacking: 'normal'
                }
            },
            series: data.statusCount
        });
    };
    
    var onSuccess = function () {
        $.each($scope.entitys, function (index, value) {
            createBar(value);
        });
                
        $(".bargraph-entity").hide();
        $("#bar-" + $scope.selectedEntity.Id).show();
    };

    var onError = function () {
        $rootScope.showError({ message: "Les statistiques ne sont pas disponibles." });
    }

    var param = {
        userId: $rootScope.getUser().Id,
        success: onSuccess,
        error: onError
    };
    dash.load(param.userId, param.success, param.error);

    $scope.doSelectEntity = function () {
        $scope.selectedEntity = this.entity;
        $(".bargraph-entity").hide();
        $("#bar-"+$scope.selectedEntity.Id).show();
    };
}]);
