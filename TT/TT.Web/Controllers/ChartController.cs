using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TT.DAL.Repository;

namespace TT.Web.Controllers
{
    public class ChartController : Controller
    {
        private IQuoteRepository _quoteRepository;

        public ChartController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public ChartController(): this(new QuoteRepository())
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 60*60*24*10)]
        public JsonResult GetCurrencyList()
        {
            var currencyList = _quoteRepository.GetCurrencyList();
            return Json(currencyList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartData(String symbol)
        {
            var chartData = _quoteRepository.Get(symbol, DateTime.UtcNow.AddHours(-1));
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

    }
}