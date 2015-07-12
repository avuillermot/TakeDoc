var WorkflowType = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.WorkflowTypeId);
        this.set("reference", current.WorkflowTypeReference);
        this.set("label", current.WorkflowTypeLabel);
        return this;
    }
});

var WorkflowTypes = Backbone.Collection.extend({
    model: WorkflowType,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new WorkflowType();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        var url = environnement.UrlBase + "odata/WorkflowTypes";
        this.fetch({ success: param.success, error: param.error, url: url, beforeSend: requestHelper.beforeSend(), reset: true }).always(param.always);
    },
});