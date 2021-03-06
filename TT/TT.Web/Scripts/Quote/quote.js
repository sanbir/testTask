﻿

var Quote = function(symbol, change, changePercent, bid, ask)
{
    this.Symbol = ko.observable(symbol);
    this.ChangePoints = ko.observable(change);
    this.ChangePercent = ko.observable(changePercent);

    this.Bid = ko.observable(bid);
    this.BidColor = ko.observable("black");

    this.Ask = ko.observable(ask);
    this.AskColor = ko.observable("black");
};


var wsImpl = window.WebSocket || window.MozWebSocket;
// create a new websocket and connect
var ws = new wsImpl('ws://109.111.185.164:8082/');//'ws://localhost:8082/');
ws.onmessage = function (evt)
{
    var quotes = JSON.parse(evt.data);

    for (var i = 0; i < quotes.length; i++)
    {
        var newQ = ko.mapping.fromJS(quotes[i]);
        var found = false;

        for (var j = 0; j < quoteModel.quotes().length; j++)
        {
            var oldQ = quoteModel.quotes()[j];
            if (newQ.Symbol() == oldQ().Symbol())
            {
                var bidColor = 'black';
                if (newQ.Bid() > oldQ().Bid())
                {
                    bidColor = 'green';
                }
                if (newQ.Bid() < oldQ().Bid()) {
                    bidColor = 'red';
                }

                var askColor = 'black';
                if (newQ.Ask() > oldQ().Ask()) {
                    askColor = 'green';
                }
                if (newQ.Ask() < oldQ().Ask()) {
                    askColor = 'red';
                }

                newQ.BidColor = ko.observable(bidColor);
                newQ.AskColor = ko.observable(askColor);

                oldQ(newQ);
                found = true;
            }
        }
        if (!found)
        {
            newQ.BidColor = ko.observable('black');
            newQ.AskColor = ko.observable('black');
            quoteModel.quotes.push(ko.observable(newQ));
        }
    }

};


var QuoteModel = function ()
{
    
    this.quotes = ko.observableArray();

    this.quotes([]);
    

    this.hasClickedTooManyTimes = ko.pureComputed(function () {
        return this.numberOfClicks() >= 3;
    }, this);

    this.search = ko.observable(null);


    this.sendFilterTimeout = null;

    this.search.subscribe(function(newValue)
    {
        if (self.sendFilterTimeout)
        {
            clearTimeout(self.sendFilterTimeout);
        }
        self.sendFilterTimeout = setTimeout(function () { ws.send(newValue); }, 3000);
    });



    this.filteredQuotes = ko.pureComputed(function()
    {
        if (!this.search() || this.search() == '')
        {
            return this.quotes();
        } else
        {
            return this.quotes().filter(function(el)
            {
                return el().Symbol().toLowerCase().indexOf(this.search().toLowerCase()) > -1;
            }, this);
        }
    }, this);
};

var quoteModel = new QuoteModel();



ko.applyBindings(quoteModel, document.getElementById("quotes"));