function fileHelper() {

}

fileHelper.prototype.read = function (fileName) {
    var dfd = new $.Deferred();

    var fn = function () {
        console.log("fileHelper.prototype.read:start " + fileName);

        var error = function () {
            console.log("fileHelper.prototype.read:error");
        };

        var gotFile = function (fileEntry) {
            console.log("fileHelper.prototype.read:gotFile start");
            fileEntry.file(function (file) {
                console.log("fileHelper.prototype.read:gotFile->fileEntry.file");
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    var content = e.target.result;
                    console.log(content);
                    dfd.resolve(content);
                }

                reader.readAsBinaryString(file);
            });
        };
        window.resolveLocalFileSystemURL(fileName, gotFile, error);
    };
    fn();

    return dfd.promise();
}

fileHelper.readUrl = function (versionId, entityId) {
    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + "Print/Url/"+versionId+"/"+entityId,
        success: function () {
            window.open(environnement.UrlBase+"Temp/Pdf/"+arguments[0]);
        },
        error: function () {
            //onError();
        }
    });
}

fileHelper.download = function (versionId, entityId) {
    var fileTransfer = new FileTransfer();
    var uri = encodeURI(environnement.UrlBase + "Print/Url/"+versionId+"/"+entityId);

    fileTransfer.download(
        uri,
        filePath,
        function (entry) { // success
            console.log("download complete: " + entry.fullPath);
        },
        function (error) { // error
            console.log("download error source " + error.source);
            console.log("download error target " + error.target);
            console.log("upload error code" + error.code);
        },
        false,
        {
            headers: {
                "Authorization": "Basic dGVzdHVzZXJuYW1lOnRlc3RwYXNzd29yZA=="
            }
        }
    );
}
