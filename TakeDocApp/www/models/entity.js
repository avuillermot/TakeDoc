function entity(data) {
    if (data != null) {
        this.Id = data.EntityId;
        this.Reference = data.EntityReference
        this.Label = data.EntityLibelle;
    }
}

function entityService() {

}

entityService.get = function (userId) {
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