using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 主页控制器.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 返回主页界面.
        /// </summary>
        /// <returns>主页界面.</returns>
        public ActionResult Home()
        {
            return View();
        }
    }
}