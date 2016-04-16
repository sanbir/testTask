
var lineChartData = ChartTools().GetTestChartData();

window.onload = function () {
    var ctx = document.getElementById("canvas").getContext("2d");
    myLine = new Chart(ctx).Line(lineChartData, {
        responsive: true
    });
}

var Pair = function (symbol) {
    this.Symbol = symbol;
   
    };

var getTestPairs = function () {
    var result = [];
    result.push(new Pair("usdeur"));
    result.push(new Pair("gbpusd"));
    result.push(new Pair("usdcad"));
    return result;
};


var updateChartData = function()
{
    myLiveChart.addData([Math.random() * 100, Math.random() * 100], ++latestLabel);
    // Remove the first point so we dont just add values forever
    myLiveChart.removeData();
}

var updateChartData = function()
{
    myLiveChart.datasets[1].points[indexToUpdate].value = Math.random() * 100;
    myLiveChart.update();
}
var ChartModel = function () {

    this.Pairs = ko.observableArray();

    this.Pairs(getTestPairs());
   
    this.hasClickedTooManyTimes = ko.pureComputed(function () {
        return this.numberOfClicks() >= 3;
    }, this);

    this.search = ko.observable(null);

    this.loadChart = function(pair)
    {
        myLine.destroy();
        var ctx = document.getElementById("canvas").getContext("2d");
        var lineChartData = ChartTools().GetTestChartData();
        myLine = new Chart(ctx).Line(lineChartData, {
            responsive: true
        });

    };

    this.filteredQuotes = ko.pureComputed(function () {
        if (!this.search() || this.search() == '') {
            return this.quotes();
        } else {
            return this.quotes().filter(function (el) {
                return el.Symbol().toLowerCase().indexOf(this.search().toLowerCase()) > -1;
            }, this);
          
        }
    }, this);
};



ko.applyBindings(new ChartModel(), document.getElementById("chart"));