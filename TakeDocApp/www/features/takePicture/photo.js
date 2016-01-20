var photo = {
    moveUp: function (myDoc, pageId) {
        var size = myDoc.pages.length;
        var page = myDoc.pages.where({ id: pageId });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex > 1) {
            var pageToMove = myDoc.pages.where({ pageNumber: currentIndex - 1 });
            page[0].set('pageNumber', currentIndex - 1);
            pageToMove[0].set('pageNumber', currentIndex);

            page[0].setUpdate();
            pageToMove[0].setUpdate();
        }
    },
    moveDown: function (myDoc, pageId) {
        var size = myDoc.pages.length;
        var page = myDoc.pages.where({ id: pageId });
        var currentIndex = page[0].get('pageNumber');
        if (currentIndex < size) {
            var pageToMove = myDoc.pages.where({ pageNumber: currentIndex + 1 });
            page[0].set('pageNumber', currentIndex + 1);
            pageToMove[0].set('pageNumber', currentIndex);

            page[0].setUpdate();
            pageToMove[0].setUpdate();
        }
    },
    rotate: function (myDoc, pageId) {
        var elem = angular.element("#img-page-" + pageId);
        var prefix = "take-picture-rotate";
        var r000 = elem.hasClass(prefix + "000");
        var r090 = elem.hasClass(prefix + "090");
        var r180 = elem.hasClass(prefix + "180");
        var r270 = elem.hasClass(prefix + "270");

        elem.removeClass(prefix + "000");
        elem.removeClass(prefix + "090");
        elem.removeClass(prefix + "180");
        elem.removeClass(prefix + "270");

        var rotation = 0;
        if (r000) {
            elem.addClass(prefix + "090");
            rotation = 90;
        }
        else if (r090) {
            elem.addClass(prefix + "180");
            rotation = 180;
        }
        else if (r180) {
            elem.addClass(prefix + "270");
            rotation = 270;
        }
        else if (r270) {
            elem.addClass(prefix + "000");
            rotation = 0;
        }
        var page = myDoc.pages.where({ id: pageId });
        page[0].set('rotation', rotation);
        page[0].setUpdate();
    },
    delete: function (myDoc, pageId) {
        var size = myDoc.pages.length;
        var page = myDoc.pages.where({ id: pageId });
        page[0].set('action', 'DELETE');
        this.numeroter(myDoc, 1, size);
    },
    numeroter: function (myDoc, startIndex, size) {
        var toDelete = myDoc.pages.where({ action: 'DELETE' });
        $.each(toDelete, function (index, value) {
            value.set('pageNumber', '-1');
        });

        var index = startIndex;
        var nb = startIndex;
        while (index <= size + 10) {
            var page = myDoc.pages.where({ pageNumber: index });
            if (page.length > 0) {
                page[0].set('pageNumber', nb++);
                page[0].setUpdate();
            }
            index++;
        }
    },
    enlarge: function (myDoc, pageId) {
        var page = myDoc.pages.where({ id: pageId })[0];

        var elem = angular.element("#img-page-" + pageId);
        var prefix = "take-picture-rotate";
        var cssRotation = prefix + "000";
        if (elem.hasClass(prefix + "090")) cssRotation = prefix + "090";
        if (elem.hasClass(prefix + "180")) cssRotation = prefix + "180";
        if (elem.hasClass(prefix + "270")) cssRotation = prefix + "270";

        return {
            img: "<img style='width:100%;height:100%' class='" + cssRotation + "' src='" + page.get("base64Image") + "' />",
            pageNumber: page.get("pageNumber")
        };
    }
}
