using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingSystem.Controllers
{
    public class VotingHomeController : Controller
    {
        // GET: VotingHome
        public ActionResult VotingHome()
        {
            return View();
        }
    }
}