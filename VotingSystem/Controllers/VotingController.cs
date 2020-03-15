using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 移动端投票界面的控制器.
    /// </summary>
    public class VotingController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 进入评分界面.
        /// </summary>
        /// <param name="expertId">评委id.</param>
        /// <returns>评分界面.</returns>
        public ActionResult Score(int expertId)
        {
            ViewBag.expertId = expertId;
            return View();
        }

        /// <summary>
        /// 进入投票界面.
        /// </summary>
        /// <param name="expertId">评委id.</param>
        /// <returns>投票界面.</returns>
        public ActionResult Vote(int expertId)
        {
            ViewBag.expertId = expertId;
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

        }

        /// <summary>
        /// 新增或修改评分.
        /// </summary>
        /// <param name="contentId">内容id.</param>
        /// <param name="expertId">评委id.</param>
        /// <param name="value">评分值.</param>
        /// <returns>json.</returns>
        public ActionResult ChangeScoreValue(int contentId, int expertId, int value)
        {

        }

        /// <summary>
        /// 删除评分.
        /// </summary>
        /// <param name="contentId">内容id.</param>
        /// <param name="expertId">评委id.</param>
        /// <returns>json.</returns>
        public ActionResult DeleteScore(int contentId, int expertId)
        {

        }

        /// <summary>
        /// 提交评分.
        /// </summary>
        /// <param name="expertId">评委id.</param>
        /// <returns>json.</returns>
        public ActionResult SubmitScore(int expertId)
        {

        }
    }
}