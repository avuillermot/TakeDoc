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

documentService.create = function () {
    var that = this;
    console.log("documents.prototype.create:start");
    $.ajax({
        type: 'PUT',
        url: environnement.UrlBase+"odata/Documents(0)",
        data: {
            EntityId: this.EntityId, UserCreateData: this.UserCreateData, DocumentTypeId: this.DocumentTypeId, DocumentLabel: this.DocumentLabel
        },
        success: function () {
            alert("ok");
            console.log("documents.prototype.create:success");
            that.DocumentId = arguments[0].DocumentId;
            that.DocumentCurrentVersion = arguments[0].DocumentCurrentVersion;

            var current = arguments[0];
            that.addPage("jpeg", that.Pages[0].imageURI, 0);
        },
        error: function () {
            alert("error");
            console.log("documents.prototype.create:error");
         }
    });
}

documentService.addPage = function (extension, fileStringFormat, index) {
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
            else that.setReceive();

        },
        error: function () {
            console.log("documents.prototype.addPage:error");
        }
    });
}

documentService.setReceive = function () {
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
        },
        error: function () {
            console.log("documents.prototype.setReceive:error");
        }
    });
}