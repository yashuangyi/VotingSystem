using SqlSugar;
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
    }
}