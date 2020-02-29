using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;
using System.Collections.Generic;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 投票项目iframe的控制器.
    /// </summary>
    public class ProjectController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 返回投票项目管理界面.
        /// </summary>
        /// <returns>投票项目管理界面.</returns>
        public ActionResult Project()
        {
            return View();
        }

        /// <summary>
        /// 获取项目列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <returns>项目列表.</returns>
        public ActionResult GetProjectList(int page, int limit)
        {
            // 须返回数据条数
            int count = Db.Queryable<Project>().Count();

            // 分页操作，Skip()跳过前面数据项
            List<Project> list = Db.Queryable<Project>().Skip((page - 1) * limit).Take(limit).ToList();

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}