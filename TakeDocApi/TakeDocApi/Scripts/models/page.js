var Page = Backbone.Model.extend({
    defaults: {
        id: null,
        pageNumber: null,
        rotation: null,
        base64Image: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.id);
        this.set("pageNumber", current.pageNumber);
        this.set("rotation", current.rotation);
        this.set("base64Image", current.base64Image);
        return this;
    }
});

var Pages = Backbone.Collection.extend({
    model: Page,
    parse: function () {
        var data = arguments[0];
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new Page();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
        return arr;
    }
});
