using System;
using System.Collections.Generic;
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

      /*  public JsonResult GetCurrencyList()
        {
            _quoteRepository.Get()
        }
*/
    }
}