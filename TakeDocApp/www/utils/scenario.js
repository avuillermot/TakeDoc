var scenario = function () {
    this.stories = new Array();
    this.currentStory = null;
    this.currentStepIndex = 0;
};

scenario.prototype.init = function () {
    this.stories.push({ name: arguments[0], steps: arguments[1] });
};

scenario.prototype.start = function (storyName) {
    var that = this;
    that.currentStory = null;
    $.each(this.stories, function (index, value) {
        if (value.name === storyName){
            that.currentStory = value;
        }
    });
    
    if (this.currentStory == null) return null;
    this.currentStepIndex = 0;
    var step = this.currentStory.steps[this.currentStepIndex];
    return step;
};

scenario.prototype.next = function () {
    var step = this.currentStory.steps[this.currentStepIndex++];
    return step;
};

scenario.prototype.nextUrl = function () {
    var step = this.next();
    return step.to.substr(0, step.to.indexOf("@"));
};


