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

var MetaDataFile = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        name: null,
        path: null,
        data: null,
        extension: null,
        mimeType: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.id);
        this.set("reference", current.reference);
        this.set("id", current.id);
        this.set("name", current.name);
        this.set("path", current.path);
        this.set("data", current.data);
        this.set("extension", current.extension);
        this.set("mimeType", current.mimeType);
        return this;
    }
});

var MetaDataValues = Backbone.Collection.extend({
    model: MetaDataValue,
    parse: function () {
        var data = arguments[0];
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                var current = new MetaDataValue();
                this.models.push(current.parse(data[i]));
                this.length = this.models.length;
            }
        }
    }
});

var MetaData = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        name: null,
        key: null,
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
        autoCompleteReference: null,
        file: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.id);
        this.set("index", current.index);
        this.set("name", current.name);
        this.set("value", current.value);
        this.set("text", current.text);
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
        if (current.valueList != null) {
            var values = new MetaDataValues();
            values.parse(current.valueList);
            this.set("valueList", values);
        }
        if (current.file != null) {
            var file = new MetaDataFile();
            file.parse(current.file);
            this.set("file", file);
        }
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

            if (current.get("htmlType") == "list") {
                debugger;
                var value = current.get("value");
                // we have the value, we find text
                var values = current.get("valueList").where({ key: value });
                if (values.length > 0) current.set("text", values[0].get("text"));
            }
            
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
        var nbFile = this.where({ type: "file" }).length;
        var count = 0;

        var that = this;
        var parameters = arguments;
        var fnSave = function (myModels, context) {
            var arr = new Array();
            arr.push({versionId: context.versionId});
            arr.push(myModels);
            arr.push("");
            var data = JSON.stringify(arr);
            var myUrl = environnement.UrlBase + "DocumentComplete/<userId/>/<entityId/>/<startWorkflow/>".replace("<userId/>", context.userId)
                .replace("<entityId/>", context.entityId)
                .replace("<versionId/>", context.versionId)
                .replace("<startWorkflow/>", context.startWorkflow);
            $.ajax({
                type: 'POST',
                url: myUrl,
                data: { '': data },
                beforeSend: requestHelper.beforeSend(),
                success: function () {
                    onSucces.apply(that, arguments);
                },
                error: function () {
                    onError.apply(that, arguments);
                }
            });
        };

        this.each(function (model, index) {
            if (model.get("type") == "date") {
                model.set("value", moment(model.get("value")).format("YYYY-MM-DD"));
            }
            else if (model.get("type") == "file") {
                var hasFile = model.get("file").get("path") != null && model.get("file").get("path") != "";
                count++;
                if (hasFile) {
                    var currentModel = model;
                    fileHelper.read(currentModel.get("file").get("path")).then(function (fileBin) {
                        if (fileBin != null) {
                            var metaFile = currentModel.get("file");
                            metaFile.set("data", fileBin);
                        }
                        if (count == nbFile) fnSave(that.models, ctx);
                    });
                }
                else {
                    var metaFile = model.get("file");
                    metaFile.set("data", null);
                    if (count == nbFile) fnSave(that.models, ctx);
                }
            }
        });
        if (nbFile == 0) fnSave(this.models, ctx);
    },
    load: function (param) {
        var url = (environnement.UrlBase + "metadata/version/<versionId/>/<entityId/>").replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId);
        this.fetch({ success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: url });
    }
});