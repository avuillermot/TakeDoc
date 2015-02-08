function userTk() {
    this.FirstName = "Alexandre";
    this.LastName = "Vuillermot";
    this.Email = "avuillermot@hotmail.com";
    this.Telephone = "0380520356";
    this.Id = "A90CEA2D-7599-437B-88D3-A5405BE3EF93";
    this.Entitys = new Array();
    this.Entitys.push({ Id: "55C72E33-8864-4E0E-9BC8-C82378B2BF8C", Reference: "MASTER", Label: "MASTER" });
    this.CurrentEntity = this.Entitys[0];
    this.IsLog = false;
};

function userTkService() {

}

userTkService.logon = function (login, pwd) {
    var retour = new userTk();
    retour.Entitys = entityService.get(login);
    retour.IsLog = true;
    return retour;
};