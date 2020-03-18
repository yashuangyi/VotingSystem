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

            // 为了防止某些皮玩家投票完成后按返回键再改数据，这里做多一次判断
            if (expert.Status == "已投票")
            {
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }

            Project project = Db.Queryable<Project>().Where(it => it.Id == expert.ProjectId).Single();
            List<Content> list = new List<Content>();
            list = Db.Queryable<Content>().Where(it => it.ProjectId == project.Id).OrderBy(it => it.Id).ToList();
            int count = list.Count();
            list = list.Skip((pageNum - 1) * contentNumPerPage).Take(contentNumPerPage).ToList();
            int hasVoteNum = Db.Queryable<Record>().Where(it => it.ExpertId == expertId && it.Value != null).ToList().Count();
            for (int i = 0; i < list.Count(); i++)
            {
                // 查询是否有值
                Record record = Db.Queryable<Record>().Where(it => it.ContentId == list[i].Id && it.ExpertId == expertId).Single();
                if (record != null && record.Value != null)
                {
                    list[i].Result = record.Value.ToString();
                }
            }

            return Json(new { code = 200, totalVoteNum = count, list, hasVoteNum }, JsonRequestBehavior.AllowGet);
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
            bool isAdd = false;
            Record record = Db.Queryable<Record>().Where(it => it.ContentId == contentId && it.ExpertId == expertId).Single();
            if (record != null)
            {
                record.Value = value;
                Db.Updateable(record).ExecuteCommand();
            }
            else
            {
                Record newRecord = new Record
                {
                    ContentId = contentId,
                    ExpertId = expertId,
                    Value = value,
                };
                Db.Insertable(newRecord).ExecuteCommand();
                isAdd = true;
            }

            return Json(new { code = 200, isAdd }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除评分.
        /// </summary>
        /// <param name="contentId">内容id.</param>
        /// <param name="expertId">评委id.</param>
        /// <returns>json.</returns>
        public ActionResult DeleteScore(int contentId, int expertId)
        {
            bool isDelete = false;
            Record record = Db.Queryable<Record>().Where(it => it.ContentId == contentId && it.ExpertId == expertId).Single();
            if (record != null)
            {
                Db.Deleteable(record).ExecuteCommand();
                isDelete = true;
            }

            return Json(new { code = 200, isDelete }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交评分.
        /// </summary>
        /// <param name="expertId">评委id.</param>
        /// <returns>json.</returns>
        public ActionResult SubmitScore(int expertId)
        {
            var expert = Db.Queryable<Expert>().Where(it => it.Id == expertId).Single();
            var project = Db.Queryable<Project>().Where(it => it.Id == expert.ProjectId).Single();
            expert.Status = "已投票";
            Db.Updateable(expert).ExecuteCommand();
            project.HasVote++;
            Db.Updateable(project).ExecuteCommand();
            var recordList = Db.Queryable<Record>().Where(it => it.ExpertId == expertId).ToList();
            foreach (Record record in recordList)
            {
                var content = Db.Queryable<Content>().Where(it => it.Id == record.ContentId).Single();
                content.Score += (int)record.Value;
                Db.Updateable(content).ExecuteCommand();
            }

            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}