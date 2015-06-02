var Action = Backbone.Model.extend({
    defaults: {
        id: null,
        lastName: null,
        firstName: null,
        fullName: null,
        dateRealize: null,
        answerId: null,
        answerReference: null,
        statusId: null,
        typeDocumentId: null,
        typeDocumentLabel: null,
        index: null,
        versionId: null,
        entityId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.Id);
        this.set("lastName", current.LastName);
        this.set("firstName", current.FirstName);
        this.set("fullName", current.FullName);
        this.set("dateRealize", current.DateRealize);
        this.set("answerId", current.AnswerId);
        this.set("answerReference", current.AnswerReference);
        this.set("statusId", current.StatusDocumentId);
        this.set("typeDocumentId", current.TypeDocumentId);
        this.set("typeDocumentLabel", current.TypeDocumentLabel);
        this.set("index", current.Index);
        this.set("versionId", current.VersionId);
        this.set("entityId", current.EntityId);
        return this;
    }
});

var Actions = Backbone.Collection.extend({
    model: Action,
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new Action();
            arr.push(current.parse(data[i]));
        }
        return arr;
    }
});

var WorkflowHistory = Backbone.Model.extend({
    defaults: {
        id: null,
        documentId: null,
        documentIndex: null,
        statusId: null,
        statusLabel: null,
        versionId: null,
        entityId: null,
        dateCreateData: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DocumentId+"-"+current.DocumentIndex);
        this.set("documentId", current.DocumentId);
        this.set("documentIndex", current.DocumentIndex);
        this.set("statusId", current.DocumentStatusId);
        this.set("statusLabel", current.DocumentStatusLabel);
        this.set("versionId", current.DocumentVersionId);
        this.set("entityId", current.EntityId);
        this.set("dateCreateData", current.DateCreateData);
        return this;
    }
});

var WorkflowHistorys = Backbone.Collection.extend({
    model: WorkflowHistory,
    url: environnement.UrlBase + "workflow/history/<documentId/>/<entityId/>",
    parse: function () {
        debugger;
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new WorkflowHistory();
            var wf = current.parse(data[i]);
            var actions = new Array();
            for (var j = 0; j < data[i].Actions.length; j++) {
                var action = new Action();
                actions.push(action.parse(data[i].Actions[j]));
            }
            wf.actions = actions;
            arr.push(wf);
        }
        return arr;
    },
    load: function (param) {
        debugger;
        var myUrl = this.url.replace("<documentId/>", param.documentId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error, url: myUrl, reset: true }).always(param.always);
    },
});