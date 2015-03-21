var MetaDataValue = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        key: null,
        value: null,
        reference: null,
        etatDelete: null,
    }
});

var MetaDataValues = Backbone.Collection.extend({
    model: MetaDataValue
});

var Metadata = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        name: null,
        value: null,
        mandatory: null,
        type: null,
        label: null,
        htmlType: null,
        valueList: new MetaDataValues(),
        autoCompleteId: null,
        autoCompleteTitle: null,
        autoCompletePlaceHolder: null,
        autoCompleteUrl: null,
        autoCompleteReference: null
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
    check: function () {
        var retour = { message: "", valid: true };

        var msg = "";
        var nbError = 0;
        for (var i = 0; i < this.models.length; i++) {
            var current = this.models[i];
            var mandatory = current.get("mandatory");

            if (current.get("type") == "ion-toggle" && (current.get("value") == null || current.get("value") == "")) current.set("value", "false");
            
            if (mandatory == true) {
                var myValue = (current.get("type") != "date") ? current.get("value") : current.get("dateValue");

                if (myValue == null || myValue == "") {
                    nbError++;
                    retour.valid = false;
                    msg = msg + " - " + current.get("label");
                }
            }
        }
        if (msg.length > 3 && msg.substr(0, 3) == " - ") msg = msg.substring(3, msg.length);
        if (retour.valid == false) {
            if (nbError > 1) retour.message = "Les champs [<field/>] sont obligatoires.".replace("<field/>", msg);
            else retour.message = "Le champ [<field/>] est obligatoire.".replace("<field/>", msg);
        }
        return retour;
    },

    save: function (user, onSucces, onError) {
        var retour = this.check();
        if (retour.valid) {
            this.update(user, onSucces, onError);
        }
        else onError(retour);
    },

    update: function (user, onSucces, onError) {
        this.each(function (model, index) {
            if (model.get("type") == "date" && model.get("dateValue") != "" && model.get("dateValue") != null) {
                model.set("value", moment(model.get("dateValue")).format("YYYY-MM-DD"));
            }
        });
        var data = JSON.stringify(this.models);
        var myUrl = environnement.UrlBase + "MetaData/<versionId/>/<userId/>/<entityId/>".replace("<userId/>", user.userId)
            .replace("<entityId/>", user.entityId)
            .replace("<versionId/>",user.versionId);
        $.ajax({
            type: 'PUT',
            url: myUrl,
            data: { '': data },
            success: function () {
                onSucces();
            },
            error: function () {
                onError();
            }
        });
    }
});