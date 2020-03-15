using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 移动端投票结果界面的控制器.
    /// </summary>
    public class VotingResultController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 进入投票结果界面.
        /// </summary>
        /// <returns>投票结果界面.</returns>
        public ActionResult VotingResult()
        {
            return View();
        }
    }
}