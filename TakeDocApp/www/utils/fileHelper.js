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