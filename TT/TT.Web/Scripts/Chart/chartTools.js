﻿var ChartTools = function()
{
    var result = {};

    result.sparse = function(marketData)
    {
        var result = [];
        for (var i = 0; i < marketData.length - 1; i = i + 2)
        {
            var newValue = (marketData[i].Bid + marketData[i + 1].Bid) / 2;
            result.push(new MarketData(marketData[i].Time, newValue));
        }
        return result;
    };

    result.multiSparse = function(marketData, pointsLimit)
    {
        while (marketData.length > pointsLimit)
        {
            marketData = result.sparse(marketData);
        }

        return marketData;
    };

    result.GetTestChartData = function()
    {
        var random = function () { return Math.round(Math.random()*2 - 1) }; //from -1 to 1
        
        var d = 0;

        var getNext = function(current)
        {
            d += random();
            return current + d;
        }


        var init = function ()
        {
            result = [];
            result.push(new MarketData(0, 50));
            for (var i = 1; i < 720; i++) {
                var newBid = getNext(result[i - 1].Bid);
                result.push(new MarketData(i, newBid));
            }
            return result;
        }

        var marketData = init();

        
        marketData = result.multiSparse(marketData, 5);

        var labels = [];
        var data = [];

        for (var j = 0; j < marketData.length; j++)
        {
            labels.push(marketData[j].Date);
            data.push(marketData[j].Bid);
        }
        
        
        return {
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
    }

    
    return result;
}