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
            DocumentPageNeed: null,
            DocumentCurrentVersionId: null,
            DocumentLabel: null,
            Pages: new Pictures(),
            Extension: "png",
            CurrentVersionId: null,
            Metadatas: null
        }
});

function documentService() {

}

documentService.create = function (document, onSuccess, onError) {
	if (document.get("DocumentPageNeed") == true && (document.Pages == null || document.Pages.length <= 0)) throw new Error("Une photographie est requise.");
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
            if (document.Pages != null && document.Pages.length > 0) documentService.addPage(document, 1, onSuccess, onError);
            else documentService.SetIncomplete(document, onSuccess, onError);
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
    metas = new Metadatas();
    var fn = function (collection) {
       document.Metadatas = collection;
       onSuccess();
    };
    var param = {
        versionId:  document.get("DocumentCurrentVersionId"),
        entityId:  document.get("EntityId"),
        success: fn, 
        error: onError
    };
    metas.load(param);
}