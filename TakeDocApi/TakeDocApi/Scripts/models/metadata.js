var MetaDataValue = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        key: null,
        text: null,
        reference: null,
        etatDelete: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.id);
        this.set("index", current.index);
        this.set("key", current.key);
        this.set("text", current.text);
        this.set("reference", current.reference);
        this.set("etatDelete", current.etatDelete);
        return this;
    }
});

var MetaDataValues = Backbone.Collection.extend({
    model: MetaDataValue,
    parse: function () {
        var data = arguments[0];
        for (var i = 0; i < data.length; i++) {
            var current = new MetaDataValue();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    }
});

var MetaData = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        name: null,
        value: null,
        mandatory: null,
        type: null,
        label: null,
        htmlType: null,
        entityId: null,
        valueList: null,
        autoCompleteId: null,
        autoCompleteTitle: null,
        autoCompletePlaceHolder: null,
        autoCompleteUrl: null,
        autoCompleteReference: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.id);
        this.set("index", current.index);
        this.set("name", current.name);
        this.set("value", current.value);
        this.set("mandatory", current.mandatory);
        this.set("type", current.type);
        this.set("label", current.label);
        this.set("htmlType", current.htmlType);
        this.set("entityId", current.entityId);
        this.set("autoCompleteId", current.autoCompleteId);
        this.set("autoCompleteTitle", current.autoCompleteTitle);
        this.set("autoCompletePlaceHolder", current.autoCompletePlaceHolder);
        this.set("autoCompleteUrl", current.autoCompleteUrl);
        this.set("autoCompleteReference", current.autoCompleteReference);
        var values = new MetaDataValues();
        values.parse(current.valueList);
        this.set("valueList", values);
        return this;
    }
});


var MetaDatas = Backbone.Collection.extend({
    model: MetaData,
    parse: function () {
        var data = arguments[0];
        for (var i = 0; i < data.length; i++) {
            var current = new MetaData();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    check: function () {
        var retour = { message: "", valid: true };

        var msg = "";
        var nbError = 0;
        for (var i = 0; i < this.models.length; i++) {
            var current = this.models[i];
            var mandatory = current.get("mandatory");

            if (current.get("type") == "ion-toggle" && (current.get("value") == null || current.get("value") == "")) current.set("value", "false");

            if (mandatory == true) {
                var myValue = (current.get("type") != "date") ? current.get("value") : moment(current.get("value")).format("YYYY-MM-DD");

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

    save: function (ctx, onSucces, onError) {
        var retour = this.check();
        if (retour.valid) {
            this.update(ctx, onSucces, onError);
        }
        else onError(retour);
    },

    update: function (ctx, onSucces, onError) {
        this.each(function (model, index) {
            if (model.get("type") == "date") {
                model.set("value", moment(model.get("value")).format("YYYY-MM-DD"));
            }
        });
        var data = JSON.stringify(this.models);
        var myUrl = environnement.UrlBase + "metaData/version/<versionId/>/<userId/>/<entityId/>".replace("<userId/>", ctx.userId)
            .replace("<entityId/>", ctx.entityId)
            .replace("<versionId/>", ctx.versionId);
        $.ajax({
            type: 'PUT',
            url: myUrl,
            data: { '': data },
            beforeSend: requestHelper.beforeSend(),
            success: function () {
                onSucces.apply(this, arguments);
            },
            error: function () {
                onError.apply(this, arguments);
            }
        });
    },

    generatePdf: function (param) {
        var url = environnement.UrlBase + "print/generatepdf/version/<versionId/>/<entityId/>/<userId/>".replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId).replace("<userId/>", param.userId)
        $.ajax({
            type: 'POST',
            url: url,
            success: param.success,
            error: param.error,
            beforeSend: requestHelper.beforeSend()
        });
    },
    load: function (param) {
        var url = (environnement.UrlBase + "metadata/version/<versionId/>/<entityId/>").replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: url });
    }
});