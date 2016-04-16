


var Quote = function(symbol, change, changePercent, bid, ask)
{
    this.Symbol = ko.observable(symbol);
    this.Change = ko.observable(change);
    this.ChangePercent = ko.observable(changePercent);

    this.Bid = ko.observable(bid);
    this.Ask = ko.observable(ask);
};


var getTestQuotes = function ()
{
    var result = [];
    result.push(new Quote("usdeur", 23, 3, 3,6));
    result.push(new Quote("gbpusd", -3,- 2 ,31, 61));
    result.push(new Quote("usdcad", 53,5, 13, 16));
    return result;
};

var QuoteModel = function ()
{
    
    this.quotes = ko.observableArray();

    this.quotes(getTestQuotes());

    this.registerClick = function () {
        
    };

    this.resetClicks = function () {
     
    };

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
            /* var result = [];
             ko.utils.arrayForEach(this.quotes, (q) =>
             {
                 var newQ = $.extend({}, q);
                 if (newQ.Symbol().toLowerCase().indexOf(this.search().toLowerCase()) > -1)
                 {
                     result.push(newQ);
                 }
             });
             return result;*/
        }
    }, this);
};



ko.applyBindings(new QuoteModel(), document.getElementById("quotes"));