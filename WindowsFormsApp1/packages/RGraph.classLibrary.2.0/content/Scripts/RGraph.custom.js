var rgraphclass = (function () {

    var rgraphclass = function (url, dataToSend, graphType, canvasId, graphTitle, flag3d) {
        /// <signature>
        /// <summary>Constructor which on creating object draws graph itself;</summary>
        /// <param name="url">url of webservice</param>
        /// <param name="dataToSend">JSON data as parameter to webservice</param>
        /// <param name="graphType" type="string">Type of graph: Line,Pie,Bar,Donut(only 3d) etc.</param>
        /// <param name="canvasId" type="string">Canvas element Id</param>
        /// <param name="graphTitle" type="string">Graph Title</param>
        /// <param name="flag3d" type="string">True for 3d else false</param>
        /// </signature>
        this.url = url;
        this.dataToSend = dataToSend;
        this.graphType = graphType;
        this.canvasId = canvasId;
        this.graphTitle = graphTitle;
        this.is3d = flag3d;//boolean
    
        this.draw();
    };

    rgraphclass.prototype.draw = function () {

        var self = this;
        var ajax = new webservice(self.url, self.dataToSend).getData();

        $.when(ajax).then(function (result) {

            new graph(result.d, self.canvasId, self.graphTitle, self.is3d, self.graphType);
        });

    };

    return rgraphclass;
})();



var webservice = (function () {

    var webservice = function (url, dataToSend) {
        this.url = url;
        this.data = dataToSend;
    };

    webservice.prototype.getData = function () {

        var def = $.ajax({
            url: this.url,
            type: "post",
            data:this.data,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
        return def;
    };;

    return webservice;

})();


var graph = (function () {


    var graph = function (response, canvasId, graphTitle, flag, graphType) {

        this.response = response;
        this.canvasId = canvasId;
        this.graphTitle = graphTitle;
        this.is3d = flag;//boolean
        this.graphType = graphType;
        this.graphdata = "";
        this.chartData = "";
        this.labelData = "";
        this.colorData = "";
        this.opt = "";

        this.draw();
    };



    graph.prototype.set = function () {

        this.opt = {

            tooltips: this.labelData,
            labels: this.labelData,
            colors: this.colorData,
            textAccessible: false,
            gutterBottom: 120,
            gutterTop: 120,
            gutterLeft: 120,
            gutterRight: 120



        };
    }




    graph.prototype.draw = function () {

        this.graphdata = this.response;

        this.chartData = eval(this.graphdata.Data);
        this.labelData = eval(this.graphdata.Label);
        this.colorData = eval(this.graphdata.Color);
        this.set();
        this.opt.title = this.graphTitle;
        this.opt.shadow = false;
        if (this.is3d) {
            switch (this.graphType) {
                case 'Pie': this.opt.variant = 'pie3d'; break;
                case 'Donut': this.opt.variant = 'donut3d'; this.graphType = "Pie"; break;
                case 'Rose': this.opt.variant = 'stacked3d'; break;
                default: this.opt.variant = '3d';

            }


        }



        var g = new RGraph[this.graphType]({
            id: this.canvasId,
            data: this.chartData,
            options: this.opt
        }).draw();

    }

    return graph;
})();




