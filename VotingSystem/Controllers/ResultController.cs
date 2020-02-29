using System.Web.Mvc;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 投票结果iframe的控制器.
    /// </summary>
    public class ResultController : Controller
    {
        /// <summary>
        /// 返回投票结果界面.
        /// </summary>
        /// <returns>投票结果界面.</returns>
        public ActionResult Result()
        {
            return View();
        }
    }
}