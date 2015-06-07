var WorkflowAnswer = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null,
        workflowTypeId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.WorkflowAnswerId);
        this.set("reference", current.WorkflowAnswerReference);
        this.set("label", current.WorkflowAnswerLabel);
        this.set("workflowTypeId", current.WorkflowTypeId);
        return this;
    }
});

var WorkflowAnswers = Backbone.Collection.extend({
    model: WorkflowAnswer,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new WorkflowAnswer();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        var url = environnement.UrlBase + "odata/WorkflowAnswers?$filter=WorkflowTypeId eq guid'{{workflowTypeId}}'";
        url = url.replace("{{workflowTypeId}}", param.action.get("typeWorkflowId"));
        this.fetch({ success: param.success, error: param.error, url: url, reset: true }).always(param.always);
    },
    answer: function (param) {
        var url = environnement.UrlBase + "workflow/answer/{{workflowId}}/{{userId}}/{{answerId}}";
        url = url.replace("{{workflowId}}", param.workflowId);
        url = url.replace("{{userId}}", param.userId);
        url = url.replace("{{answerId}}", param.answerId);

        $.ajax({
            type: 'POST',
            url: url,
            data: { '': JSON.stringify(param.fields) },
            success: param.success,
            error: param.error,
            beforeSend: requestHelper.beforeSend()
        }).always(param.always);
    }
});