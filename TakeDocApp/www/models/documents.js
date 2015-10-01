var CreateDocumentTk = Backbone.Model.extend({
    defaults: {
            DocumentId: null,
            EntityId: null,
            UserCreateData: null,
            DocumentTypeId: null,
            DocumentPageNeed: null,
            DocumentCurrentVersionId: null,
            DocumentLabel: null,
            Extension: "png",
            Metadatas: null
        }
});

function documentService() {

}

documentService.create = function (document, onSuccess, onError) {
    var req = new DocumentComplete();
    var context = {
        userId: document.get("UserCreateData"),
        typeDocumentId: document.get("DocumentTypeId"),
        entityId: document.get("EntityId"),
        label: document.get("DocumentLabel"),
        success: onSuccess,
        error: onError
    };
    req.add(context);
}

documentService.SetArchive = function (param, onSuccess, onError) {
    var url = environnement.UrlBase + "Document/SetArchive/<documentId/>/<userId/>";
    url = url.replace("<documentId/>", param.documentId);
    url = url.replace("<userId/>", param.userId);

    $.ajax({
        type: 'POST',
        url: url,
        beforeSend: requestHelper.beforeSend(),
        success: function () {
            
        },
        error: function () {

        }
    });
}
