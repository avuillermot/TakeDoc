//http://localhost/TakeDocApi/odata/DocumentsExtended?$filter=substringof('Note de frais1',DocumentLabel)

var DocumentExtended = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityLabel : null,
        label: null,
        typeLabel: null,
        statusLabel: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DocumentReference);
        this.set("reference", current.DocumentReference);
        this.set("label", current.DocumentLabel);
        this.set("entityLabel", current.EntityLabel);
        this.set("typeLabel", current.DocumentTypeLabel);
        this.set("statusLabel", current.DocumentStatusLabel);
        return this;
    }
});

var DocumentsExtended = Backbone.Collection.extend({
    model: DocumentExtended,
    urlBase: environnement.UrlBase + "odata/DocumentExtendeds",
    initialize: function () {
        this.url = this.urlBase;
    },
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new DocumentExtended();
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
    loadComplete: function (param) {
        this.url = this.urlBase + "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>' and (DocumentStatusReference eq 'META_SEND' or DocumentStatusReference eq 'COMPLETE')";
        this.url = this.url.replace("<entityReference/>", param.entityReference);
        this.url = this.url.replace("<typeDocumentReference/>", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error });
    },
    loadIncomplete: function (param) {
        this.url = this.urlBase + "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>' and (DocumentStatusReference eq 'DATA_SEND' or DocumentStatusReference eq 'CREATE')";
        this.url = this.url.replace("<entityReference/>", param.entityReference);
        this.url = this.url.replace("<typeDocumentReference/>", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error });
    }
});
