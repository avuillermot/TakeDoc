//http://localhost/TakeDocApi/odata/DocumentsExtended?$filter=substringof('Note de frais1',DocumentLabel)

var DocumentExtended = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityLabel : null,
        label: null,
        typeLabel: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DocumentReference);
        this.set("reference", current.DocumentReference);
        this.set("label", current.DocumentLabel);
        this.set("entityLabel", current.EntityLabel);
        this.set("typeLabel", current.DocumentTypeLabel);
        return this;
    }
});

var DocumentsExtended = Backbone.Collection.extend({
    model: DocumentExtended,
    initialize: function () {
        this.url = environnement.UrlBase + "odata/DocumentExtendeds";
    },
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new DocumentExtended();
            arr.push(current.parse(data[i]));
        }
        return arr;
    }
});
