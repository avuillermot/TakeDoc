var drawMap = {
    lc: null,
    data: function(canvas) {
        return {
            canvas: this.getJsonCanvas(),
            base64: canvas.toDataURL()
        }
    },
    getJsonCanvas: function () {
        if (this.lc == null) return null;
        var data = this.lc.getSnapshot();
        if (data == null) return "";
        return JSON.stringify(data);
    },
    loadJsonCanvas: function (data) {
        if (this.lc == null) return null;
        this.lc.loadSnapshot(data);
    },

    checkAll: function (shapes) {
        var containers = [];
        var contents = []
        for (var i = 0; i < shapes.length; i++) {
            if (shapes[i].className == 'Rectangle') containers.push(shapes[i]);
            else contents.push(shapes[i]);
        }

        // un conteneur ne doit pas être présent dans un autre conteneur
        for (var i = 0; i < containers.length; i++) {
            var current = containers[i];
        }

        // un contenu doit être présent dans un conteneur
        // tout les points doivent être présent dans le conteneur cible
        for (var i = 0; i < contents.length; i++) {
            var content = contents[i];
            var pt = content.data;
            pt.height = content.data.imageObject.height;
            pt.width = content.data.imageObject.width;

            var valid = true;
            var parent = null;
            for (var j = 0; j < containers.length; j++) {
                var container = containers[j];

                var minx = container.data.x;
                var maxx = container.data.x + container.data.width;
                var miny = container.data.y;
                var maxy = container.data.y + container.data.height;

                if (pt.x < minx) valid = false;
                else if (pt.x + pt.width > maxx) valid = false;
                else if (pt.y < miny) valid = false;
                else if (pt.y + pt.height > maxy) valid = false;

                if (valid) parent = container;
            }
        }
    },
    check: function (shapes, current) {
        var containers = [];
        for (var i = 0; i < shapes.length; i++) {
            if (shapes[i].className == 'Rectangle') containers.push(shapes[i]);
        }

        if (current.className != 'Rectangle') {
            // un contenu doit être présent dans un conteneur
            // tout les points doivent être présent dans le conteneur cible
            var content = current;
            var pt = {
                x: content.x,
                y: content.y,
                height: content.image.height,
                width: content.image.width
            };


            var valid = true;
            var parent = null;
            for (var j = 0; j < containers.length; j++) {
                var container = containers[j];

                var minx = container.data.x;
                var maxx = container.data.x + container.data.width;
                var miny = container.data.y;
                var maxy = container.data.y + container.data.height;

                if (pt.x < minx) valid = false;
                else if (pt.x + pt.width > maxx) valid = false;
                else if (pt.y < miny) valid = false;
                else if (pt.y + pt.height > maxy) valid = false;

                if (valid) parent = container;
            }
        }
        return parent != null;
    },
    init: function (context) {
        var backgroundImage = new Image()
        if (context.backgroundImage != null) backgroundImage.src = context.backgroundImage;

        this.lc = window.LC.init(
                $(context.elemSelector).get(0),
                {
                    imageURLPrefix: context.imageURLPrefix,
                    tools: [mapTools.chambre, mapTools.baignoire],
                    backgroundShapes: [
                        window.LC.createShape('Image', { x: 0, y: 0, image: backgroundImage, scale: 1 })
                    ]
                }
       );
        $(".lc-options.horz-toolbar").hide();
        $(".color-well").hide();
    }
};

var mapTools = {

    _rectangle: function (lc, context) {  // take lc as constructor arg

        var self = this;

        return {
            usesSimpleAPI: false,  // DO NOT FORGET THIS!!!
            name: context.name,
            iconName: context.icon,
            optionsStyle: 'stroke-width',

            didBecomeActive: function (lc) {

                var onPointerDown = function (pt) {
                    self.currentShape = window.LC.createShape('Rectangle',
                        {
                            x: pt.x, y: pt.y,
                            strokeWidth: context.width,  
                            strokeColor: context.borderColor,
                            fillColor: context.fillColor //'#CEE3F6'
                        }
                    );
                    lc.setShapesInProgress([self.currentShape]);
                    lc.repaintLayer('main');
                };

                var onPointerDrag = function (pt) {
                    self.currentShape.width = pt.x - self.currentShape.x;
                    self.currentShape.height = pt.y - self.currentShape.y;
                    lc.drawShapeInProgress(self.currentShape);
                };

                var onPointerUp = function (pt) {
                    lc.saveShape(self.currentShape);
                    lc.setShapesInProgress([]);
                    var t = lc.getSnapshot();
                };

                var onPointerMove = function (pt) {

                };

                // lc.on() returns a function that unsubscribes us. capture it.
                self.unsubscribeFuncs = [
                    lc.on('lc-pointerdown', onPointerDown),
                    lc.on('lc-pointerdrag', onPointerDrag),
                    lc.on('lc-pointerup', onPointerUp),
                    lc.on('lc-pointermove', onPointerMove),
                ];
            },

            willBecomeInactive: function (lc) {
                // call all the unsubscribe functions
                self.unsubscribeFuncs.map(function (f) { f() });
            }
        }
    },
    _image: function(lc, context) {
        var self = this;

        return {
            usesSimpleAPI: false,  // DO NOT FORGET THIS!!!
            name: context.name,
            iconName: context.name,
            optionsStyle: 'stroke-width',

            didBecomeActive: function(lc) {

                var onPointerDown = function(pt) {
                    var newImage = new Image()
                    newImage.src = context.image
                    self.currentShape = window.LC.createShape('Image', 
						{
						    x: pt.x, y: pt.y,
						    image: newImage,
						    code: context.code
						}
					);
                    lc.setShapesInProgress([self.currentShape]);
                    lc.repaintLayer('main');
                };
			  
                var onPointerDrag = function(pt) {
                    self.currentShape.x = pt.x;
                    self.currentShape.y = pt.y;
                    lc.drawShapeInProgress(self.currentShape);
                };

                var onPointerUp = function(pt) {
                    lc.saveShape(self.currentShape);
						
                    //alert(drawMap.check(lc.getSnapshot().shapes, self.currentShape));
                    lc.setShapesInProgress([]);
                    //drawMap.checkAfterDraw(lc.getSnapshot().shapes);
                };
					

                var onPointerMove = function(pt) {
					
                };

                // lc.on() returns a function that unsubscribes us. capture it.
                self.unsubscribeFuncs = [
					lc.on('lc-pointerdown', onPointerDown),
					lc.on('lc-pointerdrag', onPointerDrag),
					lc.on('lc-pointerup', onPointerUp),
					lc.on('lc-pointermove', onPointerMove),
                ];
            },

            willBecomeInactive: function(lc) {
                // call all the unsubscribe functions
                self.unsubscribeFuncs.map(function(f) { f() });
            }
        }
    },
    baignoire: function(lc) {
        var context = {
            icon: 'iv-baignoire',
            name: 'Baignoire',
            image: 'img/map/baignoire.png',
            code: 'BAIGNOIRE'
        };
        return mapTools._image(lc, context);
    },
    chambre: function (lc) {
        var context = {
            icon: 'bed',
            name: 'Chambre',
            color: 'blue',
            fillColor: '#CEE3F6',
            width: 2
        };
        return mapTools._rectangle(lc, context);
    }
}
