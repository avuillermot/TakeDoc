var scenario = function () {
    this.stories = new Array();
    this.currentStory = null;
};

scenario.prototype.init = function () {
    this.stories.push({ name: arguments[0], steps: arguments[1] });
};

scenario.prototype.start = function (storyName) {
    var that = this;
    $.each(this.stories, function (index, value) {
        if (value.name === storyName){
            that.currentStory = value;
        }
    });
    
    var step = this.currentStory.steps[0];
    return step;
};

scenario.prototype.next = function () {
    var step = null;
    $.each(this.currentStory.steps, function (index, value) {
        if (window.location.href.indexOf(value.from) > -1) {
            step = value;
        }
    });
    return step;
};


