﻿var Metadata = Backbone.Model.extend({
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
    check: function () {
        var retour = { message: "", valid: true };

        var msg = "";
        var nbError = 0;
        for (var i = 0; i < this.models.length; i++) {
            var current = this.models[i];
            var mandatory = current.get("mandatory");
            if (mandatory == true) {
                var myValue = current.get("value");
                if (myValue == null || myValue == "") {
                    nbError++;
                    retour.valid = false;
                    msg = msg + " " + current.get("label");
                }
            }
        }
        if (retour.valid == false) {
            if (nbError > 1) retour.message = "Les champs [<field/>] sont obligatoires.".replace("<field/>", msg);
            else retour.message = "Le champ [<field/>] est obligatoire.".replace("<field/>", msg);
        }
        return retour;
    },

    save: function (user) {
        var retour = this.check();
        if (retour.valid) {
            this.update(user);
        }
        return retour;
    },

    update: function (user) {
        var data = JSON.stringify(this.models);
        var myUrl = environnement.UrlBase + "MetaData/<versionId/>/<userId/>/<entityId/>".replace("<userId/>", user.userId)
            .replace("<entityId/>", user.entityId)
            .replace("<versionId/>",user.versionId);
        $.ajax({
            type: 'PUT',
            url: myUrl,
            data: { '': data },
            success: function () {
                alert("ok");
            },
            error: function () {
                alert("error");
            }
        });
    }
});