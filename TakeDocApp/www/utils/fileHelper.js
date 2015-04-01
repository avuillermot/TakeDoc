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

    var uri = environnement.UrlBase + "Temp/Pdf/9c8fd677-a5e3-48cd-95d1-9604b0abf8a3.pdf";
    var fileName = "/Download/avt.pdf";

    var transferFile = function (uri, filePath) {
        alert("transfert");
        var transfer = new FileTransfer();
        transfer.download(
			uri,
			filePath,
			function (entry) {
			    var targetPath = entry.toURL();
			    if (device.platform == "Win32NT") {
			        targetPath = entry.fullPath;
			    }

			},
			function (error) {
			    alert("download error source " + error.source);
			    alert("download error target " + error.target);
			    alert("upload error code" + error.code);
			}
	    );
    };

    var getFolder = function (fileSystem, folderName, success, fail) {
        alert("gotFolder");
        fileSystem.root.getDirectory(folderName, { create: true, exclusive: false }, success, fail)
    };
    
    var getFilesystem = function (fileSystem) {
        alert("gotFS");

        if (device.platform === "Android") {
            alert("android");
            getFolder(fileSystem, "",
                function (folder) {
                    alert(folder.toURL());
                    filePath = folder.toURL() + "/Download/avt.pdf";
                    alert(filePath);
                    transferFile(uri, filePath)
                }, function () {
                    alert("failed to get folder");
                }
            );
        } else {
            var filePath;
            var urlPath = fileSystem.root.toURL();
            if (device.platform == "Win32NT") {
                urlPath = fileSystem.root.fullPath;
            }
            if (parseFloat(device.cordova) <= 3.2) {
                filePath = urlPath.substring(urlPath.indexOf("/var")) + "/" + fileName;
            } else {
                filePath = urlPath + "/" + fileName;
            }
            transferFile(uri, filePath)
        }
    };

    var fail = function () {
        alert("fail");
    };
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, getFilesystem, fail);
  
}
