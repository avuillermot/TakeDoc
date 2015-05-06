var TypeDocument = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
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
    },
    update: function (param) {
        var fn1 = function () {
            $.ajax({
                type: 'POST',
                url: environnement.UrlBase + "TypeDocument/Update/"+param.userId,
                data: { '': JSON.stringify(param.typeDocument) },
                success: fn2,
                error: param.error,
                beforeSend: requestHelper.beforeSend()
            });
        };

        var fn2 = function () {
            $.ajax({
                type: 'POST',
                url: environnement.UrlBase + "TypeDocument/Update/DataField/" + param.typeDocument.id + "/" + param.userId + "/" + param.typeDocument.entityId,
                data: { '': JSON.stringify(param.fields) },
                success: param.success,
                error: param.error,
                beforeSend: requestHelper.beforeSend()
            });
        };

        fn1();
    },
});

var TypeValidation = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.TypeValidationId);
        this.set("reference", current.TypeValidationReference);
        this.set("label", current.TypeValidationLabel);
        return this;
    }
});

var TypeValidations = Backbone.Collection.extend({
    model: TypeValidation,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new TypeValidation();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        var url = environnement.UrlBase + "odata/TypeValidations";
        this.fetch({ success: param.success, error: param.error, url: url, reset: true }).always(param.always);
    },
});