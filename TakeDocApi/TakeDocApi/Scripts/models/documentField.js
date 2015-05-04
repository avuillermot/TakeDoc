var documentField = Backbone.Model.extend({
    defaults: {
        //id: null,
        reference: null,
        label: null,
        index: null,
        inputType: null,
        isList: null,
        isAutocomplete: null,
        autoCompleteId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.FieldId);
        this.set("reference", current.Reference);
        this.set("label", current.Label);
        this.set("index", current.DisplayIndex);
        this.set("inputType", current.InputType);
        this.set("isList", current.IsList);
        this.set("isAutocomplete", current.IsAutocomplete);
        this.set("autoCompleteId", current.DataFieldAutoCompleteId);
        return this;
    }
});

var documentFields = Backbone.Collection.extend({
    model: documentField,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new documentField();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.url = this.urlBase;
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/TypeDocumentDataFields?$filter=TypeDocumentId eq guid'" + param.id + "' and EtatDeleteData eq false") }).always(param.always);
    }
});

var fieldValue = Backbone.Model.extend({
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

var fieldValues = Backbone.Collection.extend({
    model: fieldValue,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new fieldValue();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/DataFieldValues?$filter=DataFieldId eq guid'"+ param.id +"' and EtatDeleteData eq false") }).always(param.always);
    }
});

var fieldAutocomplete = Backbone.Model.extend({
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

var fieldAutocompletes = Backbone.Collection.extend({
    model: fieldAutocomplete,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new fieldAutocomplete();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        alert(1);
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/DataFieldAutocompletes?$filter=DataFieldAutoCompleteId eq guid'" + param.id + "' and EtatDeleteData eq false") }).always(param.always);
    }
});