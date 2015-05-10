var TypeDocument = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityRefence: null,
        label: null,
        deleted: null,
        pageNeed: null,
        typeValidationId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.TypeDocumentId);
        this.set("reference", current.TypeDocumentReference);
        this.set("label", current.TypeDocumentLabel);
        this.set("entityId", current.EntityId);
        this.set("deleted", current.EtatDeleteData);
        this.set("pageNeed", current.TypeDocumentPageNeed);
        this.set("typeValidationId", current.TypeDocumentValidationId);
        return this;
    }
});

var TypeDocuments = Backbone.Collection.extend({
    model: TypeDocument,
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new TypeDocument();
            /*this.models.push(current.parse(data[i]));
            this.length = this.models.length;*/
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
    load: function (param) {
        if (param.deleted == null) param.deleted = false;
        var url = environnement.UrlBase + "odata/TypeDocuments?$filter=EntityId eq guid'" + param.entityId + "' and EtatDeleteData eq "+param.deleted;
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
    insert: function (param) {
        $.ajax({
            type: 'PUT',
            url: environnement.UrlBase + "TypeDocument/Add/" + param.label + "/" + param.userId + "/" + param.entityId,
            success: param.success,
            error: param.error,
            beforeSend: requestHelper.beforeSend()
        }).always(param.always);
    },
    delete: function (param) {
        $.ajax({
            type: 'DELETE',
            url: environnement.UrlBase + "TypeDocument/Delete/" + param.typeDocumentId + "/" + param.userId + "/" + param.entityId,
            success: param.success,
            error: param.error,
            beforeSend: requestHelper.beforeSend()
        }).always(param.always);
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