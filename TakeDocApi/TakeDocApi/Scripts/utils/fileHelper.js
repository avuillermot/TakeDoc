function fileHelper() {

}

fileHelper.read = function (fileName) {
    var dfd = new $.Deferred();

    var fn = function () {
        var error = function () {
            alert("Impossible de lire le fichier : " + fileName);
        };
        
        var gotFile = function (fileEntry) {
            fileEntry.file(function (file) {
            var reader = new FileReader();
            reader.onloadend = function (e) {
                var content = e.target.result;
                    dfd.resolve(content);
                }

                reader.readAsDataURL(file);
            });
        };
        window.resolveLocalFileSystemURL(fileName, gotFile, error);
    }

    fn();

    return dfd.promise();
}

fileHelper.copyOnServerTemp = function (id, entityId, type, success, error) {
    debugger;
    var url = environnement.UrlBase + "Print/Url/Document/" + id + "/" + entityId;
    if (type == "medatafile") url = environnement.UrlBase + "Print/Url/File/" + id + "/" + entityId;
    $.ajax({
        type: 'GET',
        url: url,
        beforeSend: requestHelper.beforeSend(),
        success: function () {
            success.apply(this, arguments);
        },
        error: function () {
            error.apply(this, arguments);
        }
    });
}

fileHelper.readDocumentUrl = function (versionId, entityId, success, error) {
    var onSuccess = function () {
        window.open(environnement.UrlBase + "Temp/Pdf/" + arguments[0]);
        success.apply(this,arguments);
    };
    var onError = function () {
        error.apply(this, arguments);
    };
    fileHelper.copyOnServerTemp(versionId, entityId, "document", onSuccess, onError);
}

fileHelper.readFileUrl = function (mdFileId, entityId, success, error) {
    var onSuccess = function () {
        window.open(environnement.UrlBase + "Temp/Pdf/" + arguments[0]);
        success.apply(this, arguments);
    };
    var onError = function () {
        error.apply(this, arguments);
    };
    fileHelper.copyOnServerTemp(mdFileId, entityId, "medatafile", onSuccess, onError);
}