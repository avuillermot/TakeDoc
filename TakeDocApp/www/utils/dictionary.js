var KeyValue = Backbone.Model.extend({
    defaults: {
        key: null,
        value: null
    }
});

var Dictionary = Backbone.Collection.extend({
    model: KeyValue,
    addKeyValue: function (key, value) {
        var exist = this.getByKey(key);
        if (exist == null) {
            var keyValue = new KeyValue();
            keyValue.set("key", key);
            debugger;
            keyValue.set("value", value);
            this.push(keyValue);
        }
        else alert("la clé existe");
    },
    getByKey: function (key) {
        var kvs = this.where({ key: key });
        if (kvs == null || kvs.length == 0) return null;
        return kvs[0];
    }
});