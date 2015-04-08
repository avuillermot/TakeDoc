var GroupTk = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.GroupTkId);
        this.set("reference", current.GroupTkReference);
        this.set("label", current.GroupTkLabel);
        return this;
    }
});

var GroupTks = Backbone.Collection.extend({
    model: GroupTk,
    urlBase: environnement.UrlBase + "odata/GroupTks",
    initialize: function () {
        this.url = this.urlBase;
    },
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new GroupTk();
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
    loadAll: function (param) {
        this.url = this.urlBase;
        this.fetch({ success: param.success, error: param.error });
    }
});