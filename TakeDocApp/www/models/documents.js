function documents() {
    this.DocumentId = null;
    this.EntityId = null;
    this.UserCreateData = null;
    this.DocumentTypeId = null;
    this.DocumentCurrentVersion = null;
    this.DocumentLabel = null;
    this.Pages = null;
}

function documentService() {

}

documentService.create = function (document, onSuccess, onError) {
    alert("create doc");
	if (document.Pages == null || document.Pages.length == 0) throw new Error("Aucune page n'est disponible.");
    var that = this;
    console.log("documents.prototype.create:start");
    $.ajax({
        type: 'PUT',
        url: environnement.UrlBase+"odata/Documents(0)",
        data: {
            EntityId: document.EntityId, UserCreateData: document.UserCreateData, DocumentTypeId: document.DocumentTypeId, DocumentLabel: document.DocumentLabel
        },
        success: function () {
            alert("create doc ok");
            console.log("documents.prototype.create:success");
            document.DocumentId = arguments[0].DocumentId;
            document.DocumentCurrentVersion = arguments[0].DocumentCurrentVersion;

            var current = arguments[0];
            that.addPage(document, "jpeg", 0, onSuccess, onError);
        },
        error: function () {
            alert("create error");
			onError();
            console.log("documents.prototype.create:error");
         }
    });
}

documentService.addPage = function (document, extension, index, onSuccess, onError) {
    console.log("documents.prototype.addPage:start");
    var urlAddPage = environnement.UrlBase + "Page/Add?userId=<userId/>&entityId=<entityId/>&versionId=<versionId/>&extension=<extension/>";
    urlAddPage = urlAddPage.replace("<userId/>", document.UserCreateData);
    urlAddPage = urlAddPage.replace("<entityId/>", document.EntityId);
    urlAddPage = urlAddPage.replace("<versionId/>", document.DocumentCurrentVersion);
    urlAddPage = urlAddPage.replace("<extension/>", extension);
    console.log("documents.prototype.addPage:url -> " + urlAddPage);
    alert("add page " + index + "url:" + urlAddPage);

    var that = this;
    $.ajax({
        type: 'POST',
        url: urlAddPage,
        data: { '': document.Pages.at(index).get("imageURI") },
        success: function () {
            alert("add page ok" + index);
            console.log("documents.prototype.addPage:success");
            if (index + 1 < that.Pages.length) that.addPage("jpeg", index++, onSuccess, onError);
            else {
                that.setReceive(onSucces, onError);
			}

        },
        error: function () {
            alert("add page error" + index);
            console.log("documents.prototype.addPage:error");
			onError();
        }
    });
}

documentService.setReceive = function (document, onSuccess, onError) {
    console.log("documents.prototype.setReceivd:e:start");
    alert("set receive");
    var urlSetReceive = environnement.UrlBase + "Document/SetReceive/<documentId/>";
    urlSetReceive = urlSetReceive.replace("<documentId/>", document.DocumentId);

    console.log("documents.prototype.setReceive:url -> " + urlSetReceive);

    $.ajax({
        type: 'GET',
        url: urlSetReceive,
        success: function () {
            console.log("documents.prototype.setReceive:success");
			onSucces();
        },
        error: function () {
            console.log("documents.prototype.setReceive:error");
			onError();
        }
    });
}