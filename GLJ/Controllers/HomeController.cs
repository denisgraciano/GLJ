using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GLJ.Models;

namespace GLJ.Controllers
{
    public class HomeController : BaseControllerGeneric<ProcessData>
    {
        [AuthorizeOnion]
        public ActionResult Index()
        {
            return View();
        }

    }
}
