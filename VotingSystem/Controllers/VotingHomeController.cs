using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 投票主页界面的控制器.
    /// </summary>
    public class VotingHomeController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 进入投票主页界面.
        /// </summary>
        /// <returns>投票主页界面.</returns>
        public ActionResult VotingHome()
        {
            ViewBag.ExpertId = Session["expertId"];
            return View();
        }

        /// <summary>
        /// 初始化数据.
        /// </summary>
        /// <param name="expertId">评委Id.</param>
        /// <returns>json.</returns>
        public ActionResult ReadState(int expertId)
        {
            var login = Db.Queryable<Expert>().Where(it => it.Id == expertId).Single();
            if (login != null)
            {
                var project = Db.Queryable<Project>().Where(it => it.Id == login.ProjectId).Single();
                var projectStatus = project.Status;
                var expertName = login.Name;
                var expertIsVote = login.Status;
                return Json(new { code = 200, expertName, expertIsVote, projectStatus }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取待投票的项目信息.
        /// </summary>
        /// <param name="expertId">评委Id.</param>
        /// <returns>json.</returns>
        public ActionResult GetProjectDetail(int expertId)
        {
            var expert = Db.Queryable<Expert>().Where(it => it.Id == expertId).Single();
            int projectId = expert.ProjectId;
            var project = Db.Queryable<Project>().Where(it => it.Id == projectId).Single();
            return Json(new { code = 200, project }, JsonRequestBehavior.AllowGet);
        }
    }
}