var drawMap = {
    canvas: null,

    drawLine: function (points) {
        var ctx = this.canvas.getContext("2d");
        ctx.beginPath();
        ctx.moveTo(points[0].x, points[0].y);
        ctx.lineTo(points[1].x, points[1].y);
        ctx.stroke();
    }
}
