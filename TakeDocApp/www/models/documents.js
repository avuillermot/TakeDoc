/*var CreateDocumentTk = Backbone.Model.extend({
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
*/