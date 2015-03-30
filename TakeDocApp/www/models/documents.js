var Picture = Backbone.Model.extend({
    defaults: {
        id: null,
        imageURI: null,
        state: null,
        pageNumber: null,
        rotation: 0
    }
});

var Pictures = Backbone.Collection.extend({
    model: Picture
});

var TkDocument = Backbone.Model.extend({
    defaults: {
            DocumentId: null,
            EntityId: null,
            UserCreateData: null,
            DocumentTypeId: null,
            DocumentCurrentVersionId: null,
            DocumentLabel: null,
            Pages: new Pictures(),
            Extension: "jpeg",
            CurrentVersionId: null,
            Metadatas: null
        }
});

function documentService() {

}

documentService.create = function (document, onSuccess, onError) {
	if (document.Pages == null || document.Pages.length == 0) throw new Error("Aucune page n'est disponible.");
    console.log("documents.prototype.create:start");
    $.ajax({
	        type: 'PUT',
	        url: environnement.UrlBase+"odata/Documents(0)",
	    data: {
	        EntityId: document.get("EntityId"), UserCreateData: document.get("UserCreateData"), DocumentTypeId: document.get("DocumentTypeId"), DocumentLabel: document.get("DocumentLabel")
        },
        success: function () {
            document.set("DocumentId", arguments[0].DocumentId);
            document.set("DocumentCurrentVersionId", arguments[0].DocumentCurrentVersionId);
            var current = arguments[0];
            documentService.addPage(document, 1, onSuccess, onError);
        },
        error: function () {
			onError();
         }
    });
}

documentService.addPage = function (document, index, onSuccess, onError) {
    console.log("documents.prototype.addPage:start");
    var currentPage = document.Pages.where({ pageNumber: index })[0];
    var nextPages = document.Pages.where({ pageNumber: index + 1 });

    var urlAddPage = environnement.UrlBase + "Page/Add?userId=<userId/>&entityId=<entityId/>&versionId=<versionId/>&extension=<extension/>&rotation=<rotation/>";
    urlAddPage = urlAddPage.replace("<userId/>", document.get("UserCreateData"));
    urlAddPage = urlAddPage.replace("<entityId/>", document.get("EntityId"));
    urlAddPage = urlAddPage.replace("<versionId/>", document.get("DocumentCurrentVersionId"));
    urlAddPage = urlAddPage.replace("<extension/>", document.get("Extension"));
    urlAddPage = urlAddPage.replace("<rotation/>", currentPage.get("rotation"));
    $.ajax({
        type: 'POST',
        url: urlAddPage,
        data: { '': currentPage.get("imageURI") },
        success: function () {
            if (nextPages.length > 0) documentService.addPage(document, index + 1, onSuccess, onError);
            else {
                documentService.SetIncomplete(document, onSuccess, onError);
			}

        },
        error: function () {
			onError();
        }
    });
}

documentService.SetIncomplete = function (document, onSuccess, onError) {
    console.log("documents.prototype.SetIncomplete:start");
    var url = environnement.UrlBase + "Document/SetIncomplete/<documentId/>/<userId/>";
    url = url.replace("<documentId/>", document.get("DocumentId"));
    url = url.replace("<userId/>", document.get("UserCreateData"));

    $.ajax({
        type: 'GET',
        url: url,
        success: function () {
            documentService.getMetaData(document, onSuccess, onError);
        },
        error: function () {
			onError();
        }
    });
}

documentService.getMetaData = function (document, onSuccess, onError) {
    metas = new Metadatas("byVersion", document.get("DocumentCurrentVersionId"), document.get("EntityId"));
    var fn = function (collection) {
       document.Metadatas = collection;
       onSuccess();
    };
    metas.fetch({ success: fn, error: onError } );
}

/*documentService.loadByRef = function (reference, onSuccess, onError) {
    var url = environnement.UrlBase + "odata/Documents?$filter=DocumentReference eq '<reference/>'";
    url = url.replace("<reference/>", reference);

    $.ajax({
        type: 'GET',
        url: url,
        success: function () {
            if (arguments[0] == null || arguments[0].value == null) onSuccess(null);
            var current = arguments[0].value[0];
            documentService.getMetaData(current);
        },
        error: function () {
            onError();
        }
    });
}*/