var DataField = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityId: null,
        label: null,
        delete: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DataFieldId);
        this.set("reference", current.DataFieldReference);
        this.set("entityId", current.EntityId);
        this.set("label", current.DataFieldLabel);
        this.set("delete", current.EtatDeleteData);
        return this;
    }
});

var DataFields = Backbone.Collection.extend({
    model: DataField,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new DataField();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.url = this.urlBase;
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/DataFields?$filter=EntityId eq guid'" + param.entityId + "' and EtatDeleteData eq false") }).always(param.always);
    }
});