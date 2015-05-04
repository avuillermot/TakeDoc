var TypeDocument = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityRefence: null,
        label: null,
        delete: null,
        pageNeed: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.TypeDocumentId);
        this.set("reference", current.TypeDocumentReference);
        this.set("label", current.TypeDocumentLabel);
        this.set("entityId", current.EntityId);
        this.set("delete", current.EtatDeleteData);
        this.set("pageNeed", current.TypeDocumentPageNeed);
        return this;
    }
});

var TypeDocuments = Backbone.Collection.extend({
    model: TypeDocument,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new TypeDocument();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        var url = environnement.UrlBase + "odata/TypeDocuments?$filter=EntityId eq guid'" + param.entityId + "'";
        this.fetch({ success: param.success, error: param.error, url: url });
    }
});