var ManagerTypeDocumnent = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        firstName: null,
        lastName: null,
        deleted: null,
        entityId: null
    }
});

var ManagerTypeDocumnents = Backbone.Collection.extend({
    model: ManagerTypeDocumnent,
    load: function (param) {
        var url = environnement.UrlBase + ("typedocument/get/backofficeuser/{typeDocumentId}/{entityId}").replace("{typeDocumentId}", param.typeDocumentId).replace("{entityId}", param.entityId);
        this.fetch({ success: param.success, error: param.error, url: url, reset: true });
    }
});