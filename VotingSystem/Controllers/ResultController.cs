using System.Collections.Generic;
using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 投票结果iframe的控制器.
    /// </summary>
    public class ResultController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 返回投票结果界面.
        /// </summary>
        /// <returns>投票结果界面.</returns>
        public ActionResult Result()
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
        /// 获取内容列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <param name="search">搜索字符串.</param>
        /// <returns>项目列表.</returns>
        public ActionResult GetResultList(int page, int limit, string search = null)
        {
            // 须返回数据条数
            int count = Db.Queryable<Content>().Count();
            List<ContentDTO> list = null;

            // 分页操作，Skip()跳过前面数据项
            if (string.IsNullOrEmpty(search) || search == "请选择待查询项目")
            {
                list = Db.Queryable<Project, Content>((project, content) => new object[]
                {
                    JoinType.Inner,
                    content.ProjectId == project.Id,
                }).Select((project, content) => new ContentDTO
                {
                    Id = content.Id,
                    ProjectId = content.ProjectId,
                    Number = content.Number,
                    Name = content.Name,
                    Score = content.Score,
                    Result = content.Result,
                    Method = project.Method,
                    ProjectName = project.Name,
                    Progress = project.ExpertCount.ToString(),
                    HasVote = project.HasVote,
                }).Skip((page - 1) * limit).Take(limit).ToList();
            }
            else
            {
                list = Db.Queryable<Project, Content>((project, content) => new object[]
                {
                    JoinType.Inner,
                    content.ProjectId == project.Id && project.Name == search,
                }).Select((project, content) => new ContentDTO
                {
                    Id = content.Id,
                    ProjectId = content.ProjectId,
                    Number = content.Number,
                    Name = content.Name,
                    Score = content.Score,
                    Result = content.Result,
                    Method = project.Method,
                    ProjectName = project.Name,
                    Progress = project.ExpertCount.ToString(),
                    HasVote = project.HasVote,
                }).Skip((page - 1) * limit).Take(limit).ToList();
            }

            foreach (var item in list)
            {
                item.Progress = item.HasVote + " / " + item.Progress;
                item.TicketsNum = item.FirstPrizeNum + " / " + item.SecondPrizeNum + " / " + item.ThirdPrizeNum + " / " + item.GiveupNum;
            }

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}