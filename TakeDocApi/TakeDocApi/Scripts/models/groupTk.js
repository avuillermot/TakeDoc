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
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new GroupTk();
            this.models.push(current.parse(data[i]));
        }
    },
    loadAll: function (param) {
        this.url = this.urlBase;
        this.fetch({ success: param.success, error: param.error, url: (environnement.UrlBase + "odata/GroupTks") });
    }
});