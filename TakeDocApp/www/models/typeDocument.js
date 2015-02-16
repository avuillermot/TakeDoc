function typeDocument() {
    this.TypeDocumentId = null;
    this.EntityId = null;
    this.TypeDocumentLibelle = null;
}

function typeDocumentService() {

}

typeDocumentService.prototype.get = function (entityId) {
    var retour = new Array();
    var add = new typeDocument();
    add.EntityId = null; add.TypeDocumentId = 1; add.TypeDocumentLibelle = "Note de frais";
    retour.push(add);

    add = new typeDocument();
    add.EntityId = null; add.TypeDocumentId = 2; add.TypeDocumentLibelle = "Facture";
    retour.push(add);

    add = new typeDocument();
    add.EntityId = null; add.TypeDocumentId = 2; add.TypeDocumentLibelle = "Note de service";
    retour.push(add);

    return retour;
}