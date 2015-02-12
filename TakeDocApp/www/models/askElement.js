function askCheckbox() {
	this.title = "Eau froide";
	this.description = "Température relevé au robinet aprs 30 secondes d'écoulement d'eau froide. Doit être supérieur à 20°C."
	this.label = "Température";
	this.observation = "";
}

function askText() {
	this.title = "Eau froide";
	this.description = "Température relevé au robinet aprs 30 secondes d'écoulement d'eau froide. Doit être supérieur à 20°C."
	this.typeData = "text";
	this.label = "Température";
	this.textValue = "valeur";
	this.observation = "";
	this.mandatory = true;
}

function askService() { }

askService.get = function () {

}