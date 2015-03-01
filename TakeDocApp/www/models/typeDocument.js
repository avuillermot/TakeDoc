function typeDocument() {
    this.TypeDocumentId = null;
    this.EntityId = null;
    this.TypeDocumentLibelle = null;
}

function typeDocumentService() {

}

typeDocumentService.get = function (entityId, success, error) {

    var url = environnement.UrlBase + "odata/TypeDocuments?$filter=EntityId eq guid'"+entityId+"'";
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error
    });
}