//http://localhost/TakeDocApi/odata/DocumentsExtended?$filter=substringof('Note de frais1',DocumentLabel)

var DocumentExtended = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityId: null,
        entityRefence: null,
        entityLabel : null,
        label: null,
        typeLabel: null,
        statusLabel: null,
        ownerFullName: null,
        versionId: null,
        versionReference: null,
        versionDateCreate: null,
        formatDate: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DocumentId);
        this.set("reference", current.DocumentReference);
        this.set("label", current.DocumentLabel);
        this.set("entityId", current.EntityId);
        this.set("entityReference", current.EntityReference);
        this.set("entityLabel", current.EntityLabel);
        this.set("typeLabel", current.DocumentTypeLabel);
        this.set("statusReference", current.DocumentStatusReference);
        this.set("statusLabel", current.DocumentStatusLabel);
        this.set("ownerFullName", current.DocumentOwnerFullName);
        this.set("versionId", current.VersionId);
        this.set("versionReference", current.VersionReference);
        this.set("versionDateCreate", current.VersionDateCreateData);
        this.set("formatDate", moment(current.VersionDateCreateData).format("LLL"));
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
    loadOptions: "&$orderby=VersionDateCreateData desc&$top=100",
    loadComplete: function (param) {
        this.url = this.urlBase + "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>' and (DocumentStatusReference eq 'META_SEND' or DocumentStatusReference eq 'COMPLETE')"+this.loadOptions;
        this.url = this.url.replace("<entityReference/>", param.entityReference);
        this.url = this.url.replace("<typeDocumentReference/>", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error });
    },
    loadIncomplete: function (param) {
        this.url = this.urlBase + "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>' and (DocumentStatusReference eq 'DATA_SEND' or DocumentStatusReference eq 'CREATE')"+this.loadOptions;
        this.url = this.url.replace("<entityReference/>", param.entityReference);
        this.url = this.url.replace("<typeDocumentReference/>", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error });
    },
    loadLast: function (param) {
        this.url = this.urlBase + "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>'"+this.loadOptions;
        this.url = this.url.replace("<entityReference/>", param.entityReference);
        this.url = this.url.replace("<typeDocumentReference/>", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error });
    }
});
