var Workflow = Backbone.Model.extend({
    defaults: {
        id: null
        
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DocumentId);
        return this;
    }
});

var Workflow = Backbone.Collection.extend({
    model: DocumentExtended
});