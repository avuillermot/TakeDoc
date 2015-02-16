var routeHelper = function () {

};

routeHelper.prototype.getTemplatePath = function (feature) {
    return "features/" + feature + "/" + feature + ".html";
};

routeHelper.prototype.get = function (feature, abstract, parameters) {
    var params = (parameters == null) ? "" : parameters;
    return {
        url: "/" + feature + params,
        abstract: abstract,
        templateUrl: this.getTemplatePath(feature),
        controller: feature + 'Controller'
    }
};

routeHelper.prototype.urlParam = function (name) {
    var results = new RegExp('[\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
    return results[1] || 0;
};
