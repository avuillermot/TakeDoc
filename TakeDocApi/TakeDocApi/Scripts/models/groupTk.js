﻿var GroupTk = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null,
        level: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.GroupTkId);
        this.set("reference", current.GroupTkReference);
        this.set("label", current.GroupTkLabel);
        this.set("level", current.GroupTkLevel);
        return this;
    }
});

var GroupTks = Backbone.Collection.extend({
    model: GroupTk,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new GroupTk();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    loadAll: function (param) {
        this.url = this.urlBase;
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: (environnement.UrlBase + "odata/GroupTks") });
    }
});