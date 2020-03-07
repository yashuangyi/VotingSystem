using SqlSugar;
using System.Collections.Generic;
using System.Web.Mvc;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 评委管理iframe的控制器.
    /// </summary>
    public class ExpertController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 返回评委管理界面.
        /// </summary>
        /// <returns>评委管理界面.</returns>
        public ActionResult Expert()
        {
            return View();
        }

        /// <summary>
        /// 加载下拉框.
        /// </summary>
        /// <returns>下拉框元素列表.</returns>
        public ActionResult ShowChoice()
        {
            List<Project> all = Db.Queryable<Project>().ToList();
            List<string> choice = new List<string>();
            foreach (Project project in all)
            {
                choice.Add(project.Name);
            }

            return Json(new { code = 200, choice }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取评委列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">搜索字符串.</param>
        /// <returns>项目列表.</returns>
        public ActionResult GetExpertList(int page, int limit, string search = null)
        {
            // 须返回数据条数
            int count = Db.Queryable<Expert>().Count();

            // 分页操作，Skip()跳过前面数据项
            List<ExpertDTO> list = Db.Queryable<Project, Expert>((project, expert) => new object[]
            {
                JoinType.Inner,
                project.Id == expert.ProjectId,
            }).Select((project, expert) => new ExpertDTO
            {
                Id = expert.Id,
                ProjectId = expert.ProjectId,
                Status = expert.Status,
                Name = expert.Name,
                Account = expert.Account,
                Password = expert.Password,
                CodePath = expert.CodePath,
                ProjectName = project.Name,
            }).Skip((page - 1) * limit).Take(limit).ToList();

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}