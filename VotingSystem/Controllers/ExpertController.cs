using System.Web.Mvc;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 评委管理iframe的控制器.
    /// </summary>
    public class ExpertController : Controller
    {
        /// <summary>
        /// 返回评委管理界面.
        /// </summary>
        /// <returns>评委管理界面.</returns>
        public ActionResult Expert()
        {
            return View();
        }
    }
}