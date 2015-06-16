var DataField = Backbone.Model.extend({
    defaults: {
        id: null,
        reference: null,
        entityId: null,
        label: null,
        deleted: null,
        typeId: null,
        isList: null,
        isAutocomplete: null,
        autoCompleteId: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DataFieldId);
        this.set("reference", current.DataFieldReference);
        this.set("entityId", current.EntityId);
        this.set("label", current.DataFieldLabel);
        this.set("deleted", current.EtatDeleteData);
        this.set("typeId", current.DataFieldTypeId);
        this.set("isList", current.IsList);
        this.set("isAutocomplete", current.IsAutocomplete);
        this.set("autoCompleteId", current.DataFieldAutoCompleteId);
        return this;
    }
});

var DataFields = Backbone.Collection.extend({
    model: DataField,
    parse: function () {
        var data = arguments[0].value;
        var arr = new Array();
        for (var i = 0; i < data.length; i++) {
            var current = new DataField();
            arr.push(current.parse(data[i]));
        }
        return arr;
    },
    load: function (param) {
        this.url = this.urlBase;
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/DataFields?$filter=EntityId eq guid'" + param.entityId + "' and EtatDeleteData eq false") }).always(param.always);
    }
});

var DataFieldType = Backbone.Model.extend({
    defaults: {
        id: null,
        inputType: null
    },
    parse: function () {
        var current = arguments[0];
        this.set("id", current.DataFieldTypeId);
        this.set("inputType", current.DataFieldInputType);
        return this;
    }
});

var DataFieldTypes = Backbone.Collection.extend({
    model: DataFieldType,
    parse: function () {
        var data = arguments[0].value;
        for (var i = 0; i < data.length; i++) {
            var current = new DataFieldType();
            this.models.push(current.parse(data[i]));
            this.length = this.models.length;
        }
    },
    load: function (param) {
        this.fetch({ reset: true, success: param.success, error: param.error, url: (environnement.UrlBase + "odata/DataFieldTypes") });
    }
});