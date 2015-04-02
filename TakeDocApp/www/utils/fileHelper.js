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

fileHelper.copyOnServerTemp = function (versionId, entityId, success, error) {
    $.ajax({
        type: 'GET',
        url: environnement.UrlBase + "Print/Url/" + versionId + "/" + entityId,
        success: function () {
            success.apply(this, arguments);
        },
        error: function () {
            error.apply(this, arguments);
        }
    });
}

fileHelper.readUrl = function (versionId, entityId, success, error) {
    var onSuccess = function () {
        window.open(environnement.UrlBase + "Temp/Pdf/" + arguments[0]);
        success.apply(this,arguments);
    };
    var onError = function () {
        error.apply(this, arguments);
    };
    fileHelper.copyOnServerTemp(versionId, entityId, onSuccess, onError);
}

fileHelper.download = function (versionId, entityId, onSuccess, onError) {

    var transferFile = function (uri, filePath) {
        var transfer = new FileTransfer();
        transfer.download(
			uri,
			filePath,
			function (entry) {
			    var targetPath = entry.toURL().replace("file:///storage","");
			    alert(targetPath);
			    try {
			        window.plugins.fileOpener2.open(
                        targetPath, 'application/pdf',
                        { 
                            error : function(e) { 
                                //console.log('Error status: ' + e.status + ' - Error message: ' + e.message);
                                onError.apply(this, arguments);
                            },
                            success : function () {
                                //console.log('file opened successfully'); 
                                onSuccess.apply(this, arguments);
                            }
                        }
                    );
			        
			    }
			    catch (ex) {
			        alert("error opener 2");
			        onError.apply(this, arguments);
			    }
    		},
			function (error) {
			    onError.apply(this, arguments);
			}
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
    fileHelper.copyOnServerTemp(versionId, entityId, success, error);
}
