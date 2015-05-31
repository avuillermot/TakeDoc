var Step = Backbone.Model.extend({
    defaults: {
        id: null
    }
});

var Steps = Backbone.Collection.extend({
    model: Step,
    load: function () {

    }
});