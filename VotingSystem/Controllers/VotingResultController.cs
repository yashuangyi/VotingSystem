using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 获取分页内容列表.
        /// </summary>
        /// <param name="expertId">评委id.</param>
        /// <param name="pageNum">当前页码.</param>
        /// <param name="contentNumPerPage">每页显示内容数.</param>
        /// <returns>json.</returns>
        public ActionResult GetContentList(int expertId, int pageNum, int contentNumPerPage)
        {
            Expert expert = Db.Queryable<Expert>().Where(it => it.Id == expertId).Single();
            Project project = Db.Queryable<Project>().Where(it => it.Id == expert.ProjectId).Single();
            List<Content> list = new List<Content>();
            list = Db.Queryable<Content>().Where(it => it.ProjectId == project.Id).OrderBy(it => it.Id).ToList();
            int count = list.Count();
            list = list.Skip((pageNum - 1) * contentNumPerPage).Take(contentNumPerPage).ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                // 查询是否有值
                Record record = Db.Queryable<Record>().Where(it => it.ContentId == list[i].Id && it.ExpertId == expertId).Single();
                if (record != null && record.Value != null)
                {
                    list[i].Result = record.Value.ToString();
                }
            }

            return Json(new { code = 200, totalVoteNum = count, list }, JsonRequestBehavior.AllowGet);
        }
    }
}