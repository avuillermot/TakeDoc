function entity() {
    this.EntityId = null;
    this.EntityLabel = "";
}

function entityService() {

}

entityService.prototype.get = function (userId) {
    var retour = new Array();
    var add = new entity();
    add.EntityId = null; add.EntityLabel = "Altran";
    retour.push(add);

    var add = new entity();
    add.EntityId = null; add.EntityLabel = "PSI";
    retour.push(add);

    var add = new entity();
    add.EntityId = null; add.EntityLabel = "April Tech.";
    retour.push(add);

    return retour;
}