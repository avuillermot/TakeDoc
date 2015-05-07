﻿'use strict';
backOffice.controller('inboxController', ['$scope', '$rootScope', '$stateParams', 'documentDisplay', 'documentsDirectory', function ($scope, $rootScope, $stateParams, documentDisplay, documentsDirectory) {

    var myDocuments = new DocumentsExtended();
    var myMetas = new MetaDatas();

    // subscribe to displayController that it can update list currently display
    $scope.$watch(function () { return documentsDirectory.data.calls; }, function () {
        if (documentsDirectory.data.documents != null)
            $scope.gridDocuments.data = documentsDirectory.data.documents.models;
    });

    var resizeGridInbox = function () {
        var h = ($(document).height() - 100);
        $("#inbox-items").css('height', h + 'px');
    };

    /**************************************************
    ***************************************************
    display/use item list and item to display
    ***************************************************
    ***************************************************/
    // document grid display
    var cellTitle = '<div ng-click="grid.appScope.showMe(row)" ng-class="{inboxActiveItem : grid.appScope.isSelectedItem(row)}"><div class="cell-inbox-item-title">{{row.entity.attributes.label}}<div id="divStatus" class="inbox-item-{{row.entity.attributes.statusReference}}">{{row.entity.attributes.statusLabel}}</div></div><div class="cell-inbox-item-entity">({{row.entity.attributes.entityLabel}} - {{row.entity.attributes.typeLabel}})</div></div>';
    var cellDate = '<div ng-click="grid.appScope.showMe(row)">{{row.entity.attributes.formatDate}}</div>';

    $scope.gridDocuments = {
        columnDefs: [
           { name: 'Titre', field: '', cellTemplate: cellTitle, cellClass: "cell-inbox-item" },
           { name: 'Date', field: 'attributes.formatDate', cellTemplate: cellDate, cellClass: "cell-inbox-item" }
        ],
        paginationPageSizes:  [20, 50, 100, 500],
        data: []
    };

    // set css for selected item
    $scope.isSelectedItem = function () {
       return $scope.selectedItem === arguments[0].entity;
    };
    
    // display detail of this document in the display module
    $scope.showMe = function () {
        
        var toShow = arguments[0].entity;
        // store selected item
        $scope.selectedItem = toShow;
        $scope.$apply();

        var success = function () {
            documentDisplay.data.metadatas = arguments[0];
            documentDisplay.data.document = toShow;
            documentDisplay.data.calls = documentDisplay.data.calls + 1;
            if(!$scope.$$phase) $scope.$apply();
        };
        var error = function () {
            $rootScope.showError(arguments[0]);
        };

        var param = {
            versionId: toShow.get("versionId"),
            entityId: toShow.get("entityId"),
            success: success, 
            error: error
        };
        myMetas = new MetaDatas();
        myMetas.load(param);
    };
    var setDashBoard = function () {
        var all = documentsDirectory.data.documents.length;
        var toValidate = documentsDirectory.data.documents.where({ statusReference: "TO_VALIDATE" }).length;
        var approve = documentsDirectory.data.documents.where({ statusReference: "APPROVE" }).length;
        $("#badge-all").html(approve);
        $("#badge-approve").html(approve);

    };

    var loadDocument = function () {
        // load documents
        var param = {
            userId: $rootScope.getUser().Id,
            success: function () {
                documentsDirectory.data.documents = arguments[0];
                $scope.gridDocuments.data = documentsDirectory.data.documents.models;
                if (!$scope.$$phase) $scope.$apply();
                resizeGridInbox();
                setDashBoard();
            },
            error: function () {
                $rootScope.showError(arguments[0]);
            }
        };

        if ($scope.selectedDirectory === "MYDOC")
            myDocuments.loadAll(param);
        else if ($scope.selectedDirectory === "TO_VALIDATE") {
            myDocuments.loadToValidate(param);
        }
        else if ($scope.selectedDirectory === "APPROVE") {
            myDocuments.loadToValidate(param);
        }
        else {
            documentsDirectory.data.documents = [];
            $scope.gridDocuments.data = [];
        }
    }

    /**************************************************
    ***************************************************
    display/use directory list and directory to display
    ***************************************************
    ***************************************************/
    // set css for selected item
    $scope.isSelectedDirectory = function (id) {
        return $scope.selectedDirectory === id;
    };

    $scope.setSelectedDirectory = function (id) {
        $scope.selectedDirectory = id;
        loadDocument(id);
    };

    $scope.setSelectedDirectory("MYDOC");
}]);