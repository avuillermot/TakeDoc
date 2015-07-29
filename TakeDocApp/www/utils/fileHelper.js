function fileHelper() {

}

fileHelper.read = function (fileName) {
    var dfd = new $.Deferred();

    var fn = function () {
        var error = function () {
            alert("Impossible de lire le fichier : " + fileName);
        };
        if (environnement.isApp == true) {
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
        else dfd.resolve("file4564564564564564665");
    };
    fn();

    return dfd.promise();
}

fileHelper.copyOnServerTemp = function (id, entityId, type, success, error) {
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

fileHelper.download = function (id, entityId, type, onSuccess, onError) {

    var transferFile = function (uri, filePath) {
        var transfer = new FileTransfer();
        transfer.download(
			uri,
			filePath,
			function (entry) {
			    var targetPath = entry.toURL();
			    var extension = targetPath.split('.').pop();

			    var typeMime = "";
			    if (extension == "pdf") typeMime = "application/pdf";
			    else if (extension == "jpg") typeMime = "image/jpeg";
                else if (extension == "jpeg") typeMime = "image/jpeg";

			    try {
			        cordova.plugins.fileOpener2.open(
                        targetPath, typeMime,
                        { 
                            error : function(e) { 
                                onError.apply(this, arguments);
                            },
                            success : function () {
                                onSuccess.apply(this, arguments);
                            },
                            beforeSend: requestHelper.beforeSend()
                        }
                    );
			        
			    }
			    catch (ex) {
			        onError.apply(this, arguments);
			    }
    		},
			function (error) {
			    onError.apply(this, arguments);
			},
            true // for accept all certificate on https
	    );
    };

    var getFolder = function (fileSystem, folderName, success, fail) {
        fileSystem.root.getDirectory(folderName, { create: true, exclusive: false }, success, fail)
    };
        
    var success = function () {
        var fileName = arguments[0];
        var uri = environnement.UrlBase + "Temp/Pdf/" + fileName;
        var getFilesystem = function (fileSystem) {
            getFolder(fileSystem, "",
                function (folder) {
                    filePath = folder.toURL() + fileName;
                    transferFile(uri, filePath);
                }, function () {
                    onError.apply(this, arguments);
                }
            );
        };

        var fail = function () {
            onError.apply(this, arguments);
        };

        window.requestFileSystem(LocalFileSystem.TEMPORARY, 0, getFilesystem, fail);
    };
    
    var error = function () {
        onError.apply(this, arguments);
    };
    fileHelper.copyOnServerTemp(id, entityId, type, success, error);
}
