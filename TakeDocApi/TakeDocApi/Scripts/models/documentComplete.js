var DocumentComplete = Backbone.Model.extend({
    document: null,
    metadatas: null,
    pages: null,
    defaults: {
        
    },
    load: function (context) {
        var url = environnement.UrlBase + "DocumentComplete/version/{{versionId}}/{{userId}}/{{entityId}}"
            .replace("{{versionId}}", context.versionId)
            .replace("{{userId}}", context.userId)
            .replace("{{entityId}}", context.entityId);
        this.fetch({ url: url, success: context.success, error: context.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    add: function (context) {
        var url = environnement.UrlBase + "DocumentComplete/{{typeDocumentId}}/{{userId}}/{{entityId}}"
            .replace("{{typeDocumentId}}", context.typeDocumentId)
            .replace("{{userId}}", context.userId)
            .replace("{{entityId}}", context.entityId);
        this.fetch({ url: url, type: 'PUT', data: { '': context.label }, success: context.success, error: context.error, beforeSend: requestHelper.beforeSend(), reset: true });
    },
    parse: function () {
        this.document = new DocumentExtended();
        this.metadatas = new MetaDatas();
        this.pages = new Pages();

        this.document.parse(arguments[0].Document);
        this.metadatas.parse(arguments[0].MetaDatas);
        this.pages.parse(arguments[0].Pages);
    },
    save: function (context) {
        var result = this.metadatas.check();
        if (result.valid) {
            var data = new Array();
            data.push(context);
            data.push(this.document);
            data.push(this.metadatas);
            data.push(this.pages);

            var json = JSON.stringify(data);

            var url = environnement.UrlBase + "DocumentComplete/{{userId}}/{{entityId}}/{{startWorkflow}}"
                .replace("{{startWorkflow}}", context.startWorkflow)
                .replace("{{userId}}", context.userId)
                .replace("{{entityId}}", this.document.get("entityId"));
            $.ajax({
                type: 'POST',
                url: url,
                data: { '': json },
                success: context.success,
                error: context.error,
                beforeSend: requestHelper.beforeSend()
            });
        }
        else {
            alert(result.message);
            context.error(false);
        }
    }/*,
    startWorkflow: function (context) {
        var data = new Array();
        data.push("");
        data.push({ versionId: this.document.get("versionId") });
        data.push("");
        data.push("");

        var json = JSON.stringify(data);

        var url = environnement.UrlBase + "DocumentComplete/{{userId}}/{{entityId}}/{{startWorkflow}}"
            .replace("{{startWorkflow}}", context.startWorkflow)
            .replace("{{userId}}", context.userId)
            .replace("{{entityId}}", this.document.get("entityId"));
        $.ajax({
            type: 'POST',
            url: url,
            data: { '': json },
            success: context.success,
            error: context.error,
            beforeSend: requestHelper.beforeSend()
        });
    }*/
});
