var ReadOnlyMetadata = Backbone.Model.extend({
    defaults: {
        Name: null,
        EntityId: null,
        Label: null,
        Value: null,
        Text: null,
        DisplayIndex: null,
    }
});

var ReadOnlyMetadatas = Backbone.Collection.extend({
    model: ReadOnlyMetadata,
    urlBase: environnement.UrlBase + "/MetaData/",
    loadBy: function (param) {
        this.url = this.urlBase + "Version/ReadOnly/<versionId/>/<entityId/>";
        this.url = this.url.replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error });
    }
});