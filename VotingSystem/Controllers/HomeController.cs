using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 主页控制器.
    /// </summary>
    public class HomeController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// echarts对象.
        /// </summary>
        public class Echarts
        {
            /// <summary>
            /// Gets or sets 值.
            /// </summary>
            public int value { get; set; }

            /// <summary>
            /// Gets or sets 名称.
            /// </summary>
            public string name { get; set; }
        }

        /// <summary>
        /// 返回主页界面.
        /// </summary>
        /// <returns>主页界面.</returns>
        public ActionResult Home()
        {
            ViewBag.AdminId = Session["adminId"];
            return View();
        }

        /// <summary>
        /// 返回首页界面.
        /// </summary>
        /// <returns>首页界面.</returns>
        public ActionResult HomePage()
        {
            return View();
        }

        /// <summary>
        /// 初始化数据.
        /// </summary>
        /// <param name="adminId">管理员Id.</param>
        /// <returns>json.</returns>
        public ActionResult ReadState(int adminId)
        {
            var login = Db.Queryable<Admin>().Where(it => it.Id == adminId).Single();
            if (login != null)
            {
                var adminName = login.Name;
                var adminPhoto = login.PhotoPath;
                var adminPower = login.Power;
                return Json(new { code = 200, adminName, adminPhoto, adminPower }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取echarts所需信息.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult GetEcharts()
        {
            var projects = Db.Queryable<Project>().Where(it => it.Status == "进行中").OrderBy(it => it.Id, OrderByType.Desc).ToList();
            if (projects[0] != null)
            {
                List<Echarts> list = new List<Echarts>();
                int voteNum = projects[0].HasVote;
                int noVoteNum = projects[0].ExpertCount - voteNum;
                Echarts vote = new Echarts
                {
                    value = voteNum,
                    name = "已评审",
                };
                Echarts noVote = new Echarts
                {
                    value = noVoteNum,
                    name = "未评审",
                };
                list.Add(vote);
                list.Add(noVote);
                return Json(new { code = 200, data = list, count=list.Count() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = 400 }, JsonRequestBehavior.AllowGet);
        }
    }
}