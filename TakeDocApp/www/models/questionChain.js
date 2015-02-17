function questionChain() {
    this.Items = new Array();
}

function questionChainService() {

}

questionChainService.get = function (chainId, entityId) {
    var retour = new questionChain();
    retour.Items = questionChainItemService.get(chainId, entityId);
    return retour;
}
