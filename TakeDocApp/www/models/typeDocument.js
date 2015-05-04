var TypeDocument = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityRefence: null,
        label: null,
        delete: null,
        pageNeed: null,
        typeValidationId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.TypeDocumentId);
        this.set("reference", current.TypeDocumentReference);
        this.set("label", current.TypeDocumentLabel);
        this.set("entityId", current.EntityId);
        this.set("delete", current.EtatDeleteData);
        this.set("pageNeed", current.TypeDocumentPageNeed);
        this.set("typeValidationId", current.TypeDocumentValidationId);
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
        if (param.label != null && param.label != "") url = url + " and startswith(TypeDocumentLabel,'"+ param.label +"')"
        this.fetch({ success: param.success, error: param.error, url: url, reset: true });
    },
    loadById: function (param) {
        var url = environnement.UrlBase + "odata/TypeDocuments?$filter=TypeDocumentId eq guid'" + param.id + "'";
        this.fetch({ success: param.success, error: param.error, url: url, reset: true });
    }
});

var TypeValidation = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.TypeDocumentId);
        this.set("reference", current.TypeDocumentReference);
        this.set("label", current.TypeDocumentLabel);
        return this;
    }
});

var TypeValidations = Backbone.Collection.extend({
    model: TypeValidation,
    initialize: function () {
        var v1 = new TypeValidation();
        v1.set("id", ("CD584F3C-D9A6-4627-82A2-07841680A628").toLowerCase());
        v1.set("reference", "MANAGER");
        v1.set("label", "Manager");
        var v2 = new TypeValidation();
        v2.set("id", "45305AFC-7FF2-45BF-9B24-2D5017F0363D".toLowerCase());
        v2.set("reference", "AUTO");
        v2.set("label", "Automatique");
        var v3 = new TypeValidation();
        v3.set("id", "172ADA1D-BB70-4262-BA51-4242AD4F8A3F".toLowerCase());
        v3.set("reference", "BACKOFFICE");
        v3.set("label", "Gestionnaire");
        var v4 = new TypeValidation();
        v4.set("id", "27E4620F-1BFE-4988-A70C-A2675AA6CE80".toLowerCase());
        v4.set("reference", "NO");
        v4.set("label", "Aucune");
        var v5 = new TypeValidation();
        v5.set("id", "42F98639-A0A3-483F-88DA-79C1C20B8EC3".toLowerCase());
        v5.set("reference", "MANAGER-BACKOFFICE");
        v5.set("label", "Manager/Backoffice");

        this.models.push(v1);
        this.models.push(v2);
        this.models.push(v3);
        this.models.push(v4);
        this.models.push(v5);
        this.length = this.model.length;
    }
});