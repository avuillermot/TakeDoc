var Dashboard = Backbone.Model.extend({
    defaults: {
        EntityId: null,
        Code: null,
        Type: null,
        Value: null,
    }
});

var Dashboards = Backbone.Collection.extend({
    model: Dashboard,
    urlBase: environnement.UrlBase + "identity/dashboard/<userId/>",
    load: function (userId, success, error) {
        this.url = this.urlBase.replace("<userId/>", userId);
        this.fetch({ success: success, error: error });
    }
});