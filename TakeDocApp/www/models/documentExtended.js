var DocumentExtended = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityId: null,
        entityReference: null,
        entityLabel : null,
        label: null,
        typeLabel: null,
        statusLabel: null,
        statusId: null,
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
        this.set("statusId", current.DocumentStatusId);
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
    },
    isReadOnly: function () {
        return this.get("statusReference") == "ARCHIVE" || this.get("statusReference") == "TO_VALIDATE" || this.get("statusReference") == "APPROVE" || this.get("statusReference") == "REFUSE";
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
        while (this.url.indexOf("?$filter= ") > -1) this.url = this.url.replace("?$filter= ", "?$filter=");
    },
    clauses: {
        complete: " and DocumentStatusReference eq 'COMPLETE' ",
        incomplete: " and (DocumentStatusReference eq 'INCOMPLETE' or DocumentStatusReference eq 'CREATE')",
        toValidate: " DocumentValidateUserId eq guid'<documentValidateUserId/>' and DocumentStatusReference eq 'TO_VALIDATE'",
        waitValidate: " and DocumentStatusReference eq 'TO_VALIDATE'",
        approve: " and DocumentStatusReference eq 'APPROVE'",
        archive: " and DocumentStatusReference eq 'ARCHIVE'",
        refuse: " and DocumentStatusReference eq 'REFUSE'",
    },
    loadAll: function (param) {
        this.url = this.urlBase + ("?$filter=DocumentOwnerId eq guid'<documentOwnerId/>'&$orderby=VersionDateCreateData desc").replace("<documentOwnerId/>", param.ownerId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    //************************************************************************
    // load my document that wait a validation, i'm owner of this document
    //************************************************************************
    loadWaitValidate: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.waitValidate + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    loadApprove: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.approve + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    loadArchive: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.archive + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    loadRefuse: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.refuse + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    //************************************************************************
    // ????
    //************************************************************************
    loadComplete: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.complete + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    loadIncomplete: function (param) {
        this.url = this.urlBase + this.loadBase + this.clauses.incomplete + this.loadOptions;
        this.replaceParameter("EntityReference eq '<entityReference/>'", "entityReference", param.entityReference);
        this.replaceParameter("and DocumentOwnerId = guid'<documentOwnerId/>'", "documentOwnerId", param.ownerId);
        this.replaceParameter("and TypeDocumentReference eq '<typeDocumentReference/>'", "typeDocumentReference", param.typeDocumentReference);
        while (this.url.indexOf("?$filter=and") > -1) this.url = this.url.replace("?$filter=and", "?$filter=");

        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), reset: true });
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
    },
    loadToValidateAsManager: function (param) {
        var url = (environnement.UrlBase + "tovalidate/manager/{managerId}").replace("{managerId}", param.managerId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: url, reset: true });
    },
    loadToValidateAsBackOffice: function (param) {
        var url = (environnement.UrlBase + "tovalidate/backoffice/{managerId}").replace("{managerId}", param.managerId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: url, reset: true });
    }
});
