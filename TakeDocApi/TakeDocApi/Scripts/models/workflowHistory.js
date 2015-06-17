var Action = Backbone.Model.extend({
    defaults: {
        id: null,
        lastName: null,
        firstName: null,
        fullName: null,
        dateRealize: null,
        answerId: null,
        answerReference: null,
        answerLabel: null,
        statusId: null,
        typeDocumentId: null,
        typeDocumentLabel: null,
        index: null,
        versionId: null,
        typeWorkflowId: null,
        entityId: null,
        comment: null,
        deleted: null
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
        this.set("answerLabel", current.AnswerLabel);
        this.set("statusId", current.StatusDocumentId);
        this.set("typeDocumentId", current.TypeDocumentId);
        this.set("typeDocumentLabel", current.TypeDocumentLabel);
        this.set("index", current.Index);
        this.set("versionId", current.VersionId);
        this.set("typeWorkflowId", current.WorkflowTypeId);
        this.set("entityId", current.EntityId);
        this.set("comment", current.Comment);
        this.set("deleted", current.EtatDeleteData);
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
        var myUrl = this.url.replace("<documentId/>", param.documentId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error, url: myUrl, reset: true }).always(param.always);
    },
    //*************************************
    // return current action to answer
    //*************************************
    getCurrentAction: function () {
        var back = null;
        var allActions = new Array();
        $.each(this.models, function (iw, vw) {
            $.each(vw.actions, function(ia, va) {
                var current = va;
                current.set("documentIndex", vw.get("documentIndex"));
                allActions.push(current);
            });
        });

        // we search first action with no answer
        var indexDoc = 32000, indexAction = 32000;
        $.each(allActions, function (index, value) {
            if (value.get("documentIndex") <= indexDoc) {
                if (value.get("index") < indexAction && value.get("answerId") == null) {
                    back = value;
                    indexDoc = value.get("documentIndex");
                    indexAction = value.get("index");
                }
            }
        });
        return back;
    }
});