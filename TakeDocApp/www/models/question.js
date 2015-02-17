function typeQuestion() {

}

typeQuestion.text = "text";
typeQuestion.temperature = "temperature";

function question() {
    this.typeData = typeQuestion.text;
    this.title = null;
    this.description = null;
    this.label = null;
    this.textValue = null;
    this.observation = null;
    this.mandatory = false;
}

function questionService() {

}

questionService.createCheckbox = function (title, description, label, observation, typeData) {
    var retour = new question();
    retour.title = title;
    retour.description = description;
    retour.label = label;
    retour.observation = observation;
    return retour;
}

questionService.createText = function (title, description, label, textValue, mandatory, observation, typeData) {
    var retour = new question();
    retour.title = title;
    retour.description = description;
    retour.typeData = typeData;
    retour.label = label;
    retour.textValue = textValue;
    retour.observation = observation;
    retour.mandatory = mandatory;
    return retour;
}

function askCheckbox() {
	this.title = "Eau froide";
	this.description = "Température relevé au robinet aprs 30 secondes d'écoulement d'eau froide. Doit être supérieur à 20°C."
	this.label = "Température";
	this.observation = "";
}

function askText() {
	this.title = "Eau froide";
	this.description = "Température relevé au robinet aprs 30 secondes d'écoulement d'eau froide. Doit être supérieur à 20°C."
	this.typeData = typeQuestion.text;
	this.label = "Température";
	this.textValue = "valeur";
	this.observation = "";
	this.mandatory = true;
}

function askService() { }

askService.get = function () {

}