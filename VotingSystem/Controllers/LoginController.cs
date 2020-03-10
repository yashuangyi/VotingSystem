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
        /// 进入后台登录界面.
        /// </summary>
        /// <returns>后台登录界面.</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 后台登录校验.
        /// </summary>
        /// <param name="admin">后台登录信息.</param>
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
                var login = Db.Queryable<Admin>().Where(it => it.Account == account && it.Password == password).Single();
                if (login != null)
                {
                    return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 进入评委登录界面.
        /// </summary>
        /// <returns>评委登录界面.</returns>
        public ActionResult ExpertLogin()
        {
            return View();
        }

        /// <summary>
        /// 评委登录校验.
        /// </summary>
        /// <param name="account">评委登录账号.</param>
        /// <param name="password">评委登录密码.</param>
        /// <returns>状态码.</returns>
        public ActionResult ExpertCheck(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                return Json(new { code = 401 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var login = Db.Queryable<Expert>().Where(it => it.Account == account && it.Password == password).Single();
                if (login != null)
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