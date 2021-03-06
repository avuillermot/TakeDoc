﻿var DocumentField = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        label: null,
        index: null,
        inputType: null,
        inputTypeLabel: null,
        isList: null,
        isAutocomplete: null,
        autoCompleteId: null,
        mandatory: null,
        deleted: null,
        entityId: null
    },
    create: function (id, reference, entityId, label, inputType, index) {
        this.set("id", id);
        this.set("reference", reference);
        this.set("label", label);
        this.set("index", index);
        this.set("inputType", inputType);
        this.set("inputTypeLabel", this.getInputTypeLabel(inputType));
        this.set("isList", false);
        this.set("isAutocomplete", false);
        this.set("autoCompleteId", false);
        this.set("mandatory", false);
        this.set("deleted", false);
        this.set("entityId", entityId);
        return this;
    },
    getInputTypeLabel: function() {
        if (this.get("inputType") == "textarea") return "Texte long";
        else if (this.get("inputType") == "text") return "Texte court";
        else if (this.get("inputType") == "number") return "Nombre";
        else if (this.get("inputType") == "number") return "Nombre";
        else if (this.get("inputType") == "date") return "Date";
        else if (this.get("inputType") == "ion-toggle") return "Oui/Non";
        else if (this.get("inputType") == "barcode") return "Code barre";
        else if (this.get("inputType") == "signature") return "Signature";
        else return this.get("inputType");
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.FieldId);
        this.set("reference", current.Reference);
        this.set("label", current.Label);
        this.set("index", current.DisplayIndex);
        this.set("inputType", current.InputType);
        this.set("inputTypeLabel", this.getInputTypeLabel(current.InputType));
        this.set("isList", (current.IsList == 1));
        this.set("isAutocomplete", (current.IsAutocomplete == 1));
        this.set("autoCompleteId", current.DataFieldAutoCompleteId);
        this.set("mandatory", current.Mandatory);
        this.set("deleted", current.EtatDeleteData);
        this.set("entityId", current.EntityId);
        return this;
    }
});

var DocumentFields = Backbone.Collection.extend({
    model: DocumentField,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new DocumentField

            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    getLastIndex: function () {
        var back = -1;
        for (var i = 0; i < this.models.length; i++) {
            var ko = this.models[i].get("inputType") == 'signature' || this.models[i].get("inputType") == 'map';
            if (ko == false && this.models[i].get("index") > back) back = this.models[i].get("index");
        }
        if (back < 0) back = 0;
        return back;
    },
    load: function (param) {
        this.url = this.urlBase;

        var andDeleted = "";
        if (andDeleted != null) "and EtatDeleteData eq " + param.deleted;
        this.fetch({ reset: true, success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: (environnement.UrlBase + "odata/TypeDocumentDataFields?$filter=TypeDocumentId eq guid'" + param.id + "' " + andDeleted) }).always(param.always);
    },
    remove: function (key, value) {
        var arr = new Array();
        for (var i = 0; i < this.models.length; i++) {
            if (this.models[i].get(key) != value) arr.push(this.models[i]);
        }
        this.models = arr;
        this.length = arr.length;
    }
});

var FieldValue = Backbone.Model.extend({
    defaults: {
        id: null,
        index: null,
        reference: null,
        key: null,
        text: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DataFieldId);
        this.set("index", current.DataFieldValueIndex);
        this.set("reference", current.DataFieldValueReference);
        this.set("key", current.DataFieldValueKey);
        this.set("text", current.DataFieldValueText);
        return this;
    }
});

var FieldValues = Backbone.Collection.extend({
    model: FieldValue,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new FieldValue();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.fetch({ reset: true, success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: (environnement.UrlBase + "odata/DataFieldValues?$filter=DataFieldId eq guid'" + param.id + "' and EtatDeleteData eq false") }).always(param.always);
    }
});

var FieldAutocomplete = Backbone.Model.extend({
    defaults: {
        id: null,
        url: null,
        placeHolder: null,
        title: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DataFieldAutoCompleteId);
        this.set("url", current.DataFieldAutoCompleteUrl);
        this.set("placeHolder", current.DataFieldAutoCompletePlaceHolder);
        this.set("title", current.DataFieldAutoCompleteTitle);
        return this;
    }
});

var FieldAutocompletes = Backbone.Collection.extend({
    model: FieldAutocomplete,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new FieldAutocomplete();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.fetch({ reset: true, success: param.success, error: param.error, beforeSend: requestHelper.beforeSend(), url: (environnement.UrlBase + "odata/AutoCompletes?$filter=DataFieldAutoCompleteId eq guid'" + param.id + "' and EtatDeleteData eq false") }).always(param.always);
    }
});