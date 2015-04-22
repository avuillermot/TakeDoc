var Page = Backbone.Model.extend({
    defaults: {
        index: null,
        base64Image: null
    }
});

var Pages = Backbone.Collection.extend({
    model: Page,

    load: function (param) {
        var url = (environnement.UrlBase + "page/image/<versionId/>/<entityId/>/<userId/>").replace("<versionId/>", param.versionId).replace("<entityId/>", param.entityId).replace("<userId/>", param.userId);
        $.ajax({
            type: 'GET',
            url: url,
            success: param.success,
            error: param.error
        });
    }
});
