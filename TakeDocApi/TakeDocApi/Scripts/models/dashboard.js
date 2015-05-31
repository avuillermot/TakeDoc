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
    urlBase: environnement.UrlBase + "dashboard/statusdocument/<userId/>",
    load: function (userId, success, error) {
        this.fetch({ success: success, error: error, url: this.urlBase.replace("<userId/>", userId) });
    },
    count: function () {
        var count = 0;
        $.each(this.models, function (index, value) {
            count = count + value.get("Count");
        });
        return count;
    },
    countStatus: function(status) {
        var count = 0;
        $.each(this.models, function (index, value) {
            if (status != null && value.get("StatusReference") == status) count = count + value.get("Count");
        });
        return count;
    },
    countStatusEntity: function (entityId, status) {
        var count = 0;
        $.each(this.models, function (index, value) {
            if (entityId != null && status != null
                && value.get("StatusReference") == status
                && value.get("EntityId") == entityId) count = count + value.get("Count");
        });
        return count;
    },
    countTypeStatusEntity: function (typeDocumentId, entityId, status) {
        var count = 0;
        $.each(this.models, function (index, value) {
            if (entityId != null && status != null && typeDocumentId != null
                && value.get("TypeDocumentId") == typeDocumentId
                && value.get("StatusReference") == status
                && value.get("EntityId") == entityId) count = count + value.get("Count");
        });
        return count;
    }
});