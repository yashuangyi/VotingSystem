using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 管理员控制器.
    /// </summary>
    public class AdminController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 返回管理员信息界面.
        /// </summary>
        /// <returns>管理员信息界面.</returns>
        public ActionResult Admin()
        {
            return View();
        }

        /// <summary>
        /// 获取管理员列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <returns>管理员列表.</returns>
        public ActionResult GetAdminList(int page, int limit)
        {
            // 须返回数据条数
            int count = Db.Queryable<Admin>().Count();

            // 分页操作，Skip()跳过前面数据项
            List<Admin> list = Db.Queryable<Admin>().Skip((page - 1) * limit).Take(limit).ToList();

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传图片.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult UploadPic()
        {
            string photoPath = string.Empty;
            string photoName = string.Empty;
            string msg = string.Empty;
            HttpPostedFileWrapper file = (HttpPostedFileWrapper)Request.Files[0];
            photoName = file.FileName;
            if (string.IsNullOrEmpty(photoName))
            {
                msg = "无效文件，请重新上传！";
                return Json(new { photoPath, msg, code = 400, photoName = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 获得当前时间的string类型
                string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                string path = "/Source/headPhoto/";
                string uploadPath = Server.MapPath("~/" + path);
                string ext = Path.GetExtension(photoName);
                string savePath = uploadPath + name + ext;
                file.SaveAs(savePath);
                photoPath = path + name + ext;
                msg = "上传成功！";
                return Json(new { photoPath, msg, code = 200, photoName }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 新建管理员账户.
        /// </summary>
        /// /// <param name="admin">传入数据.</param>
        /// <returns>Json.</returns>
        public ActionResult AddAdmin(Admin admin)
        {
            var isExist = Db.Queryable<Admin>().Where(it => it.Account == admin.Account).Single();
            if (isExist != null)
            {
                return Json(new { code = 402 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 自增列用法
                int adminId = Db.Insertable(admin).ExecuteReturnIdentity();
                admin.Id = adminId;
                Db.Updateable(admin).ExecuteCommand();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改投票项目.
        /// </summary>
        /// <param name="admin">账户.</param>
        /// <returns>Json.</returns>
        public ActionResult EditAdmin(Admin admin)
        {
            Db.Updateable(admin).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除账户.
        /// </summary>
        /// <param name="adminId">账户编号.</param>
        /// <returns>Json.</returns>
        public ActionResult DelAdmin(int adminId)
        {
            Db.Deleteable<Admin>().Where(it => it.Id == adminId).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}