

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

var getShortTime = function(time)
{
    var result = (time.getMonth() + 1) + "-" + time.getDate() + "-" + time.getFullYear() + "   " +
    time.getHours() + ":" + time.getMinutes();

    return result;
}


var server =  {
            loadPairs: function () {
                var def = $.Deferred();

                $.ajax({
                    url: "Chart/GetCurrencyList",
                    success: function (data) {
                       def.resolve(data);
                    },
                    error: function (a, b, c) {
                        alert("error");
                    },
                    contentType: "application/json; charset=utf-8"
                });


                return def;
            },
            loadChartData: function(symbol)
            {
                var def = $.Deferred();

                $.ajax({
                    url: "Chart/GetChartData",
                    success: function (response)
                    {
                        response = ChartTools().multiSparse(response, 15);

                        var labels = [];
                        var data = [];

                        for (var j = 0; j < response.length; j++)
                        {
                            var parsedTime = new Date(response[j].Time.match(/\d+/)[0] * 1);
                            labels.push(parsedTime.toLocaleTimeString());
                            data.push(response[j].Bid);
                        }

                        var result = {
                            labels: labels,
                            datasets: [
                                {
                                    label: "test dataset",
                                    fillColor: "rgba(220,220,220,0.2)",
                                    strokeColor: "rgba(220,220,220,1)",
                                    pointColor: "rgba(220,220,220,1)",
                                    pointStrokeColor: "#fff",
                                    pointHighlightFill: "#fff",
                                    pointHighlightStroke: "rgba(220,220,220,1)",
                                    data: data
                                }
                            ]

                        };

                        def.resolve(result);
                        
                    },
                    error: function (a, b, c) {
                        alert("error");
                    },
                    data: symbol,
                    contentType: "application/json; charset=utf-8"
                });


                return def;
            }
            
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

   

    this.load = function()
    {
        var def = $.Deferred();

       server.loadPairs().done(
       function (result) {
           for (var i = 0; i < result.length; i++) {
               self.pairs.push(new Pair(result[i]));
           }

           if (self.pairs().length > 0)
               self.selectedPair(self.pairs()[0]);

           def.resolve();
       }
   );

        return def;
    };


    this.chart = null;

    this.loadChart = function (pair) {
        if (self.chart) {
            self.chart.destroy();
        }

        server.loadChartData(pair).done(function (result)   //ChartTools().GetTestChartData();
        {
            var lineChartData = result;
            var ctx = document.getElementById("canvas").getContext("2d");
            self.chart = new Chart(ctx).Line(lineChartData, {
                responsive: true
            });
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


var chartModel = new ChartModel();

chartModel.load().done(function()
    {
    ko.applyBindings(chartModel, document.getElementById("chart"));
    }
);

