// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;
using System.Web.Mvc;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 登录界面的控制器.
    /// </summary>
    public class LoginController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 进入登录界面.
        /// </summary>
        /// <returns>登录界面.</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录校验.
        /// </summary>
        /// <param name="admin">登录信息.</param>
        /// <returns>状态码.</returns>
        public ActionResult Check(Admin admin)
        {
            var account = admin.Account;
            var password = admin.Password;
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                return Json(new { code = 401 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var login = Db.Queryable<Admin>().InSingle(account);
                if (login != null && login.Password == password)
                {
                    return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}