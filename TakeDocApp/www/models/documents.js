﻿function documents() {
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
            console.log("documents.prototype.create:success");
            document.DocumentId = arguments[0].DocumentId;
            document.DocumentCurrentVersion = arguments[0].DocumentCurrentVersion;

            var current = arguments[0];
            that.addPage("jpeg", that.Pages[0].imageURI, 0);
        },
        error: function () {
			onError();
            console.log("documents.prototype.create:error");
         }
    });
}

documentService.addPage = function (extension, fileStringFormat, index, onSuccess, onError) {
    console.log("documents.prototype.addPage:start");

    var urlAddPage = this.UrlBase + "Page/Add?userId=<userId/>&entityId=<entityId/>&versionId=<versionId/>&extension=<extension/>";
    urlAddPage = urlAddPage.replace("<userId/>", this.UserCreateData);
    urlAddPage = urlAddPage.replace("<entityId/>", this.EntityId);
    urlAddPage = urlAddPage.replace("<versionId/>", this.DocumentCurrentVersion);
    urlAddPage = urlAddPage.replace("<extension/>", extension);
    console.log("documents.prototype.addPage:url -> " + urlAddPage);
    var that = this;

    $.ajax({
        type: 'POST',
        url: urlAddPage,
        data: { '': fileStringFormat },
        success: function () {
            console.log("documents.prototype.addPage:success");
            if (index + 1 < that.Pages.length) that.addPage("jpeg", that.Pages[index + 1].imageURI, index + 1);
            else {
				that.setReceive(onSucces, onError);
			}

        },
        error: function () {
            console.log("documents.prototype.addPage:error");
			onError();
        }
    });
}

documentService.setReceive = function (onSuccess, onError) {
    console.log("documents.prototype.setReceivd:e:start");
    var urlSetReceive = this.UrlBase + "Document/SetReceive/<documentId/>";
    urlSetReceive = urlSetReceive.replace("<documentId/>", this.DocumentId);

    console.log("documents.prototype.setReceive:url -> " + urlSetReceive);
    var that = this;

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