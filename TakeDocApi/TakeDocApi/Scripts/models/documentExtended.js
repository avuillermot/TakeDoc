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
        statusReference: null,
        ownerId: null,
        ownerReference: null,
        ownerFullName: null,
        documentDateCreate: null,
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
        this.set("ownerId", current.DocumentOwnerId);
        this.set("ownerReference", current.DocumentOwnerReference);
        this.set("ownerFullName", current.DocumentOwnerFullName);
        this.set("versionId", current.VersionId);
        this.set("versionReference", current.VersionReference);
        this.set("versionDateCreate", current.VersionDateCreateData);
        this.set("documentDateCreate", current.DocumentDateCreateData);
        this.set("formatDate", moment(current.VersionDateCreateData).format("L"));
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
    loadOptions: "&$orderby=VersionDateCreateData desc&$top=10000",
    loadBase: "?$filter=EntityReference eq '<entityReference/>' and TypeDocumentReference eq '<typeDocumentReference/>' and DocumentOwnerId eq guid'<documentOwnerId/>'",
    replaceParameter: function(clause, field, value) {
        if (value == null) this.url = this.url.replace(clause, "");
        else this.url = this.url.replace("<" + field + "/>", value);
    },
    clauses: {
        complete: " and DocumentStatusReference eq 'COMPLETE' ",
        incomplete: " and (DocumentStatusReference eq 'INCOMPLETE' or DocumentStatusReference eq 'CREATE')",
        toValidate: " DocumentValidateUserId eq guid'<documentValidateUserId/>' and DocumentStatusReference eq 'TO_VALIDATE'",
    },
    loadToValidate: function (param) {
        this.url = this.urlBase + "?$filter=" + this.clauses.toValidate + this.loadOptions;
        this.replaceParameter("and DocumentValidateUserId eq guid'<documentValidateUserId/>'", "documentValidateUserId", param.userId);

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend() });
    },
    /*loadComplete: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.complete + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend() });
    },
    loadIncomplete: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.incomplete + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend() });
    },*/
    loadAll: function (param) {
        this.url = this.urlBase + ("?$filter=DocumentOwnerId eq guid'<documentOwnerId/>'&$orderby=VersionDateCreateData desc").replace("<documentOwnerId/>", param.userId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend() });
    },
    delete: function (param) {
        var url = (environnement.UrlBase + "document/delete/<documentId/>/<entityId/>/<userId/>").replace("<documentId/>", param.documentId).replace("<entityId/>", param.entityId).replace("<userId/>", param.userId);
        $.ajax({
            type: 'DELETE',
            url: url,
            success: param.success,
            error: param.error,
            beforeSend: requestHelper.beforeSend()
        });
    }
});
