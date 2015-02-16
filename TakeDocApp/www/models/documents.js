function documents() {
    this.DocumentId = null;
    this.EntityId = null;
    this.UserCreateData = "A90CEA2D-7599-437B-88D3-A5405BE3EF93";
    this.DocumentTypeId = "75E455BD-0717-4121-BFBB-1AF71DB4DB95";
    this.DocumentCurrentVersion = null;
    this.DocumentLabel = null;
    this.Pages = new Array();
    
    this.UrlBase = "http://192.168.0.100/takedoc/";
    //this.UrlBase = "http://localhost/takedoc/";
}

documents.prototype.create = function () {
    var that = this;
    console.log("documents.prototype.create:start");
    $.ajax({
        type: 'PUT',
        url: that.UrlBase+"odata/Documents(0)",
        data: {
            EntityId: this.EntityId, UserCreateData: this.UserCreateData, DocumentTypeId: this.DocumentTypeId, DocumentLabel: this.DocumentLabel
        },
        success: function () {

            console.log("documents.prototype.create:success");
            that.DocumentId = arguments[0].DocumentId;
            that.DocumentCurrentVersion = arguments[0].DocumentCurrentVersion;

            var current = arguments[0];
            that.addPage("jpeg", that.Pages[0].fileURI, 0);
        },
        error: function () {
            console.log("documents.prototype.create:error");
         }
    });
}

documents.prototype.addPage = function (extension, fileStringFormat, index) {
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
            if (index + 1 < that.Pages.length) that.addPage("jpeg", that.Pages[index + 1].fileURI, index + 1);
            else that.setReceive();

        },
        error: function () {
            console.log("documents.prototype.addPage:error");
        }
    });
}

/*documents.prototype.readPage = function (extension, fileName, index) {
    var that = this;
    console.log("documents.prototype.readPage:start");
    var fh = new fileHelper();

    var ok = function () {
        console.log("documents.prototype.readPage:ok:"+arguments[0]);
        that.addPage(extension, arguments[0], index);
    };

    console.log("documents.prototype.readPage:fileHelper.read");
    var promise = fh.read(fileName).done(ok);
}*/

documents.prototype.setReceive = function () {
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
            //that.callback("setReceive", "success");
        },
        error: function () {
            //that.callback("setReceive", "error");
            console.log("documents.prototype.setReceive:error");
        }
    });
}