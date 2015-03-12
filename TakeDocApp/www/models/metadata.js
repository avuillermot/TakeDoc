var Metadata = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        name: null,
        value: null,
        mandatoty: null,
        type: null,
        label: null
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
            meta.set("index", data[i].MetaDataDisplayIndex);
            meta.set("name", data[i].MetaDataName);
            meta.set("value", data[i].MetaDataValue);
            meta.set("mandatory", data[i].DataFieldMandatory);
            meta.set("type", data[i].DataFieldInputType);
            meta.set("label", data[i].DataFieldLabel);
            this.models.push(meta);
        }
    },
    save: function () {
        var retour = { message: "Les champs [<field/>] sont obligatoires.", valid: true };

        var msg = "";
        for (var i = 0; i < this.models.length; i++) {
            var current = this.models[i];
            var mandatory = current.get("mandatory");
            if (mandatory == true) {
                var myValue = current.get("value");
                if (myValue == null || myValue == "") {
                    retour.valid = false;
                    msg = msg + " " + current.get("label");
                }
            }
        }
        retour.message = retour.message.replace("<field/>", msg);
        return retour;
    }
});