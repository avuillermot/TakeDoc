var UserEntity = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null,
        enable: null,
        userId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.EntityId);
        this.set("reference", current.EntityReference);
        this.set("label", current.EntityLabel);
        this.set("enable", !current.EtatDeleteData);
        this.set("userId", current.UserTkId);
        return this;
    }
});

var UserEntitys = Backbone.Collection.extend({
    model: UserEntity,
    initialize: function () {
    },
    parse: function () {
        var data = arguments[0];
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new UserEntity();
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
    loadByUser: function (param) {
        this.url = (environnement.UrlBase + "identity/user/entity/<userId/>").replace("<userId/>", param.userId);
        this.fetch({ success: param.success, error: param.error });
    }
});