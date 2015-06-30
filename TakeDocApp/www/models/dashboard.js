var Dashboard = Backbone.Model.extend({
    defaults: {
        EntityId: null,
        EntityReference: null,
        TypeDocumentId: null,
        TypeDocumentReference: null,
        TypeDocumentLabel: null,
        StatusReference: null,
        TypeDocumentId: null,
        Count: null,
    }
});

var Dashboards = Backbone.Collection.extend({
    model: Dashboard,
    urlDetailled: environnement.UrlBase + "dashboard/detailled/statusdocument/<userId/>",
    load: function (userId, success, error) {
        this.mode = this.urlDetailled;
        this.fetch({ success: success, error: error, url: this.urlDetailled.replace("<userId/>", userId) });
    },
    countEntity: function (entityId) {
        var arr = this.where({ EntityId: entityId });
        return this.count(arr);
    },
    countStatus: function (status) {
        var arr = this.where({ StatusReference: status });
        return this.count(arr);
    },
    countTypeEntity: function (typeDocumentId, entityId) {
        var arr = this.where({ TypeDocumentId: typeDocumentId, EntityId: entityId });
        return this.count(arr);
    },
    countStatusEntity: function (status, entityId) {
        var arr = this.where({ StatusReference: status, EntityId: entityId });
        return this.count(arr);
    },
    countTypeStatusEntity: function (typeDocumentId, status, entityId) {
        var arr = this.where({ TypeDocumentId: typeDocumentId, StatusReference: status, EntityId: entityId });
        return this.count(arr);
    },
    count: function (arr) {
        var count = 0;
        $.each(arr, function (index, value) { count = count + value.get("Count"); });
        return count;
    },
    getBarGraphStatusDataSource: function (entityId) {
        var typesDoc = new Array();
        var status = new Array();

        var that = this;
        // group by type document
        $.each(that.where({EntityId: entityId}), function (index, value) {
            if (value.get("TypeDocumentReference") != null
                && that.countTypeEntity(value.get("TypeDocumentId"),entityId)
                && $.inArray(value.get("TypeDocumentLabel"), typesDoc) == -1)
                typesDoc.push(value.get("TypeDocumentLabel"));
        });
        
        // group by type status
        $.each(that.where({ EntityId: entityId }), function (index, value) {
            if (value.get("StatusReference") != null && that.exist(status, "name", value.get("StatusLabel")) == false) {    
                var data = {
                    name: value.get("StatusLabel"),
                    data: []
                }
                status.push(data);
            }
        });

        // count foreach couple type document/status
        $.each(typesDoc, function (itype, itemType) {
            $.each(status, function (istatus, itemStatus) {
                var arr = that.where({ TypeDocumentLabel: itemType, StatusLabel: itemStatus.name, EntityId: entityId });
                itemStatus.data.push(that.count(arr));
            });
        });

        return {
            typesDocument: typesDoc,
            statusCount: status
        }
    },
    exist: function (arr, property, value) {
        var found = false;
        $.each(arr, function (index, item) {
            if (item[property] === value && found == false) found = true;
        });
        return found;
    }
});