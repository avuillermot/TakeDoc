var Metadata = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        name: null,
        value: null,
        mandatoty: null,
        input: null
    }
});


var Metadatas = Backbone.Collection.extend({
    initialize: function () {
        if (arguments[0] == "byVersion") {
            this.url = environnement.UrlBase + "metadata/version/<versionId/>/<entityId/>";
            this.url = this.url.replace("<versionId/>", arguments[1]);
            this.url = this.url.replace("<entityId/>", arguments[2]);
        }
    },
    model: Metadata,
    parse: function () {
        var data = arguments[0];
        for (var i = 0; i < data.length; i++) {
            var meta = new Metadata();
            meta.set("id", data[i].MetaDataId);
            meta.set("name", data[i].MetaDataName);
            meta.set("index", data[i].MetaDataDisplayIndex);
            this.models.push(meta);
        }

        /*"MetaDataId": "6c400d60-16c5-432b-9349-1a8259184a55",
        "MetaDataDisplayIndex": 0,
        "MetaDataName": "COMMENT",
        "MetaDataValue": null,
        "DataFieldMandatory": false,
        "DataFieldInputType": null*/
    }
});