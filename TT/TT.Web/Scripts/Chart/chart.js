

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

var loadPairs = function()
{
    var def = $.Deferred();

    $.get("Chart/GetCurrencyList", function(data)
        {
            alert("success");
            def.resolve(data);
        })
        .done(function()
        {
            alert("second success");
        })
        .fail(function()
        {
            alert("error");
        })
        .always(function()
        {
            alert("finished");
        });

    return def;
};

/*var updateChartData = function()
{
    myLiveChart.addData([Math.random() * 100, Math.random() * 100], ++latestLabel);
    // Remove the first point so we dont just add values forever
    myLiveChart.removeData();
}

var updateChartData = function()
{
    myLiveChart.datasets[1].points[indexToUpdate].value = Math.random() * 100;
    myLiveChart.update();
}*/



var ChartModel = function ()
{
    var self = this;

    
    this.pairs = ko.observableArray([]);

    loadPairs().done(
        function(result)
        {
            for (var i = 0; i < result.length; i++)
            {
                self.pairs.push(new Pair(result[i]));
            }
        }
    );
   


    this.chart = null;

    this.loadChart = function (pair) {
        if (self.chart) {
            self.chart.destroy();
        }
        var ctx = document.getElementById("canvas").getContext("2d");
        var lineChartData = ChartTools().GetTestChartData();
        self.chart = new Chart(ctx).Line(lineChartData, {
            responsive: true
        });

    };

    this.selectedPair = ko.observable();

    this.selectedPair.subscribe(function(newValue)
    {
        self.loadChart(newValue);
    });

    this.selectedPair(this.pairs()[0]);

    this.hasClickedTooManyTimes = ko.pureComputed(function () {
        return this.numberOfClicks() >= 3;
    }, this);

    this.search = ko.observable(null);

    this.select = function(pair)
    {
        self.selectedPair(pair);
    }

  

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