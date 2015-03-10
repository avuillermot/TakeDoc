function documents() {
    this.DocumentId = null;
    this.EntityId = null;
    this.UserCreateData = null;
    this.DocumentTypeId = null;
    this.DocumentCurrentVersionId = null;
    this.DocumentLabel = null;
    this.Pages = null;
    this.Extension = "jpeg";
    this.CurrentVersionId = null;
}

function documentService() {

}

documentService.create = function (document, onSuccess, onError) {
	if (document.Pages == null || document.Pages.length == 0) throw new Error("Aucune page n'est disponible.");
    console.log("documents.prototype.create:start");
    $.ajax({
        type: 'PUT',
        url: environnement.UrlBase+"odata/Documents(0)",
        data: {
            EntityId: document.EntityId, UserCreateData: document.UserCreateData, DocumentTypeId: document.DocumentTypeId, DocumentLabel: document.DocumentLabel
        },
        success: function () {
            document.DocumentId = arguments[0].DocumentId;
            document.DocumentCurrentVersionId = arguments[0].DocumentCurrentVersionId;
            var current = arguments[0];
            documentService.addPage(document, 1, onSuccess, onError);
        },
        error: function () {
            debugger;
			onError();
         }
    });
}

documentService.addPage = function (document, index, onSuccess, onError) {
    console.log("documents.prototype.addPage:start");
    var currentPage = document.Pages.where({ pageNumber: index })[0];
    var nextPages = document.Pages.where({ pageNumber: index + 1 });

    var urlAddPage = environnement.UrlBase + "Page/Add?userId=<userId/>&entityId=<entityId/>&versionId=<versionId/>&extension=<extension/>&rotation=<rotation/>";
    urlAddPage = urlAddPage.replace("<userId/>", document.UserCreateData);
    urlAddPage = urlAddPage.replace("<entityId/>", document.EntityId);
    urlAddPage = urlAddPage.replace("<versionId/>", document.DocumentCurrentVersionId);
    urlAddPage = urlAddPage.replace("<extension/>", document.Extension);
    urlAddPage = urlAddPage.replace("<rotation/>", currentPage.get("rotation"));
    $.ajax({
        type: 'POST',
        url: urlAddPage,
        data: { '': currentPage.get("imageURI") },
        success: function () {
            if (nextPages.length > 0) documentService.addPage(document, index + 1, onSuccess, onError);
            else {
                documentService.setReceive(document, onSuccess, onError);
			}

        },
        error: function () {
			onError();
        }
    });
}

documentService.setReceive = function (document, onSuccess, onError) {
    console.log("documents.prototype.setReceive:start");
    var urlSetReceive = environnement.UrlBase + "Document/SetReceive/<documentId/>";
    urlSetReceive = urlSetReceive.replace("<documentId/>", document.DocumentId);

    $.ajax({
        type: 'GET',
        url: urlSetReceive,
        success: function () {
			onSuccess();
        },
        error: function () {
			onError();
        }
    });
}

documentService.getMetaData = function (document, onSuccess, onError) {
    var myUrl = environnement.UrlBase + "MetaData/Version/{versionId}/{entityId}";
    myUrl = urlSetReceive.replace("<versionId/>", document.DocumentCurrentVersionId);
    myUrl = urlSetReceive.replace("<entityId/>", document.EntityId);
    $.ajax({
        type: 'GET',
        url: myUrl,
        success: function () {
            onSuccess();
        },
        error: function () {
            onError();
        }
    });
}