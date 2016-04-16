﻿

var Quote = function(symbol, change, changePercent, bid, ask)
{
    this.Symbol = ko.observable(symbol);
    this.ChangePoints = ko.observable(change);
    this.ChangePercent = ko.observable(changePercent);

    this.Bid = ko.observable(bid);
    this.Ask = ko.observable(ask);
};


var getTestQuotes = function ()
{
    var result = [];
    result.push(ko.observable(new Quote("usdeur".toUpperCase(), 23, 3, 3,6)));
    result.push(ko.observable(new Quote("gbpusd".toUpperCase(), -3, -2, 31, 61)));
    result.push(ko.observable(new Quote("usdcad".toUpperCase(), 53, 5, 13, 16)));
    return result;
};


var wsImpl = window.WebSocket || window.MozWebSocket;
// create a new websocket and connect
var ws = new wsImpl('ws://localhost:8082/');
ws.onmessage = function (evt)
{
    var quotes = JSON.parse(evt.data);

    for (var i = 0; i < quotes.length; i++)
    {
        for (var j = 0; j < quoteModel.quotes().length; j++)
        {
            if (quotes[i].Symbol == quoteModel.quotes()[j]().Symbol())
            {
                quoteModel.quotes()[j](ko.mapping.fromJS(quotes[i]));
            }
        }
    }

};


var QuoteModel = function ()
{
    
    this.quotes = ko.observableArray();

    this.quotes(getTestQuotes());
    

    this.hasClickedTooManyTimes = ko.pureComputed(function () {
        return this.numberOfClicks() >= 3;
    }, this);

    this.search = ko.observable(null);

    this.filteredQuotes = ko.pureComputed(function()
    {
        if (!this.search() || this.search() == '')
        {
            return this.quotes();
        } else
        {
            return this.quotes().filter(function(el)
            {
                return el.Symbol().toLowerCase().indexOf(this.search().toLowerCase()) > -1;
            }, this);
        }
    }, this);
};

var quoteModel = new QuoteModel();



ko.applyBindings(quoteModel, document.getElementById("quotes"));