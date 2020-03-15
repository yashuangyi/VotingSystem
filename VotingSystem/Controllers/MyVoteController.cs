using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 移动端我的评审记录界面的控制器.
    /// </summary>
    public class MyVoteController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 进入评审记录界面.
        /// </summary>
        /// <returns>评审界面.</returns>
        public ActionResult MyVote()
        {
            return View();
        }
    }
}