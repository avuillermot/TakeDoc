var ReadOnlyMetadata = Backbone.Model.extend({
    defaults: {
        Name: null,
        EntityId: null,
        Label: null,
        Value: null,
        Text: null,
        DisplayIndex: null,
        Type: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("Name", current.Name);
        this.set("EntityId", current.EntityId);
        this.set("Label", current.Label);
        this.set("Value", current.Value);
        this.set("Text", current.Text);
        this.set("DisplayIndex", current.DisplayIndex);
        this.set("Type", current.Type);
        if (current.Type == "date") this.set("Text", moment(current.Text).format("L"));
        return this;
    }
});

var ReadOnlyMetadatas = Backbone.Collection.extend({
    model: ReadOnlyMetadata,
    urlBase: environnement.UrlBase + "/MetaData/",
    loadBy: function (param) {
        this.url = this.urlBase + "Version/ReadOnly/<versionId/>/<entityId/>";
        this.url = this.url.replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error });
    },
    parse: function () {
        var data = arguments[0];
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new ReadOnlyMetadata();
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
});