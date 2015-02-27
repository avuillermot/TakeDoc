/*{
    "Entitys": [
        {
            "EntityId": "55c72e33-8864-4e0e-9bc8-c82378b2bf8c",
            "UserTkId": "a90cea2d-7599-437b-88d3-a5405be3ef93",
            "EntityReference": "MASTER",
            "EntityLibelle": "MASTER",
            "EtatDeleteData": false
        }
    ],
    "UserTkId": "a90cea2d-7599-437b-88d3-a5405be3ef93",
    "UserTkReference": "ROOT",
    "UserTkLastName": "System",
    "UserTkFirstName": "Root",
    "UserTkLogin": "eleonore",
    "UserTkPassword": "password",
    "UserTkExternalAccount": false,
    "UserTkEmail": "avuillermot@hotmail.com"
}*/
function userTk(user) {
    if (user != null) {
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        that = this;
        $.each(user.Entitys, function(index, value) {
            if (value.EtatDeleteData == false){
                var myEntity =  new entity(value);
                that.Entitys.push(myEntity);
            }
        });
        this.IsLog = true;
    }
};

function userTkService() {

}

userTkService.logon = function (login, pwd, success, error) {
    var fn = function () {
        var user = new userTk(arguments[0]);
        success(user);
    };
    var url = environnement.UrlBase + "identity/logon/{login}/{password}";
    url = url.replace("{login}",login);
    url = url.replace("{password}", pwd);
    $.ajax({
        type: 'GET',
        url: url/*,
        success: fn,
        error: error*/
    }).done(success).fail(error);
};