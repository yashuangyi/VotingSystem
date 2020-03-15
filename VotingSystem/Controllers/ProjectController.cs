using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using VotingSystem.DB;
using VotingSystem.Models;
using VotingSystem.Util;

namespace VotingSystem.Controllers
{
    /// <summary>
    /// 投票项目iframe的控制器.
    /// </summary>
    public class ProjectController : Controller
    {
        private static readonly SqlSugarClient Db = DataBase.CreateClient();

        /// <summary>
        /// 返回投票项目管理界面.
        /// </summary>
        /// <returns>投票项目管理界面.</returns>
        public ActionResult Project()
        {
            return View();
        }

        /// <summary>
        /// 获取项目列表.
        /// </summary>
        /// <param name="page">总页数.</param>
        /// <param name="limit">一页多少行数据.</param>
        /// <returns>项目列表.</returns>
        public ActionResult GetProjectList(int page, int limit)
        {
            // 须返回数据条数
            int count = Db.Queryable<Project>().Count();

            // 分页操作，Skip()跳过前面数据项
            List<Project> list = Db.Queryable<Project>().Skip((page - 1) * limit).Take(limit).ToList();

            // 参数必须一一对应，JsonRequestBehavior.AllowGet一定要加，表单要求code返回0
            return Json(new { code = 0, msg = string.Empty, count, data = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建投票项目.
        /// </summary>
        /// /// <param name="project">传入数据.</param>
        /// <returns>Json.</returns>
        public ActionResult AddProject(Project project)
        {
            if (string.IsNullOrEmpty(project.FilePath))
            {
                // 无项目文件
                return Json(new { code = 401, msg = "请先上传投票项目文件！" }, JsonRequestBehavior.AllowGet);
            }

            // 限制同时只能存在唯一的投票项目的开关，若要开启该功能请解除下列代码的注释！
            // if (Db.Queryable<Project>().Count(it => it.Status != "结束投票") != 0)
            // {
            //    // 有进行中的投票项目
            //    return Json(new { code = 402 }, JsonRequestBehavior.AllowGet);
            // }

            // 自增列用法
            int projectId = Db.Insertable(project).ExecuteReturnIdentity();

            // 新增评委
            AddExpert(project.ExpertCount, project.Name, projectId);

            // 读取项目
            int contentNum = ReadProject(projectId, project.FilePath);
            if (contentNum > 0)
            {
                project.Id = projectId;
                project.ContentNum = contentNum;
                Db.Updateable(project).ExecuteCommand();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 403, msg = "项目文件读取失败，请按照模版格式重新上传！" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 新增指定数量评委.
        /// </summary>
        /// <param name="expertCount">评委数.</param>
        /// <param name="projectName">项目名称.</param>
        /// <param name="projectId">项目编号.</param>
        public void AddExpert(int expertCount, string projectName, int projectId)
        {
            List<Expert> list = new List<Expert>();
            for (int i = 1; i <= expertCount; i++)
            {
                Expert expert = new Expert
                {
                    Account = i.ToString(),
                    Name = projectName + "-" + i + "号评委",
                    ProjectId = projectId,
                    Status = "未投票",
                    Password = GetRandomString.GenerateRandomNumber(),
                };
                expert.CodePath = CreateCodePath(expert.Account, expert.Password, expert.Name);
                list.Add(expert);
            }

            Db.Insertable(list).ExecuteCommand();
        }

        /// <summary>
        /// 生成二维码保存.
        /// </summary>
        /// <param name="account">评委账号.</param>
        /// <param name="password">密码.</param>
        /// <param name="name">名字.</param>
        /// <returns>二维码路径.</returns>
        public string CreateCodePath(string account, string password, string name)
        {
            // 获取当前完整URL
            string url = Request.Url.ToString();
            Uri uri = new Uri(url);

            // 获取当前主机部分
            string host = uri.Host;
            string uploadPath = Server.MapPath("~/" + "/Source");
            string codePath = "http://" + host + "/VotingLogin/CodeLogin?username=" + account + "&password=" + password;
            return "/Source" + QRCodeUtil.QRCode(uploadPath, codePath, name, name);
        }

        /// <summary>
        /// 从文件中读取内容存入数据库，并返回内容数.
        /// </summary>
        /// <param name="projectId">项目编号.</param>
        /// <param name="filePath">项目文件路径.</param>
        /// <returns>内容数.</returns>
        public int ReadProject(int projectId, string filePath)
        {
            try
            {
                filePath = Server.MapPath("~/" + filePath);
                DataTable dt = FileHelper.ExcelToDataTable(filePath);
                List<Content> list = new List<Content>();
                foreach (DataRow row in dt.Rows)
                {
                    Content content = new Content
                    {
                        ProjectId = projectId,
                        Number = row[0].ToString(),
                        Name = row[1].ToString(),
                    };
                    list.Add(content);
                }

                if (list.Count > 0)
                {
                    int num = Db.Insertable(list).ExecuteCommand();
                    if (num == list.Count)
                    {
                        return num;
                    }
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// 上传文件.
        /// </summary>
        /// <returns>Json.</returns>
        public ActionResult UploadProject()
        {
            string filePath = string.Empty;
            string fileName = string.Empty;
            string msg = string.Empty;
            HttpPostedFileBase file = Request.Files["file"];
            fileName = file.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                msg = "无效文件，请重新上传！";
                return Json(new { filePath, msg, code = 400, fileName = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 获得当前时间的string类型
                string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                string path = "/Source/ProjectFile/";
                string uploadPath = Server.MapPath("~/" + path);
                file.SaveAs(uploadPath + name + ".xlsx");
                filePath = path + name + ".xlsx";
                msg = "上传成功！";
                return Json(new { filePath, msg, code = 200, fileName }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改投票项目.
        /// </summary>
        /// <param name="project">项目.</param>
        /// <returns>Json.</returns>
        public ActionResult EditProject(Project project)
        {
            if (!string.IsNullOrEmpty(project.FilePath))
            {
                Db.Deleteable<Content>().Where(it => it.ProjectId == project.Id).ExecuteCommand();
                Db.Deleteable<Expert>().Where(it => it.ProjectId == project.Id).ExecuteCommand();
                int contentNum = ReadProject(project.Id, project.FilePath);
                AddExpert(project.ExpertCount, project.Name, project.Id);
                if (contentNum > 0)
                {
                    project.ContentNum = contentNum;
                    Db.Updateable(project).ExecuteCommand();
                    return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 403, msg = "项目文件读取失败，请按照模版格式重新上传！" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { code = 404, msg = "修改失败！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启动投票项目.
        /// </summary>
        /// <param name="projectId">项目编号.</param>
        /// <returns>Json.</returns>
        public ActionResult StartProject(int projectId)
        {
            Project project = Db.Queryable<Project>().Where(it => it.Id == projectId).Single();
            project.Status = "进行中";
            Db.Updateable(project).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查投票项目是否完成.
        /// </summary>
        /// <param name="projectId">项目编号.</param>
        /// <returns>Json.</returns>
        public ActionResult IsFinished(int projectId)
        {
            Project project = Db.Queryable<Project>().Where(it => it.Id == projectId).Single();
            int rest = 0;
            int total = 0;
            var experts = Db.Queryable<Expert>().Where(it => it.ProjectId == projectId).ToList();
            foreach (Expert expert in experts)
            {
                if (expert.Status == "未投票")
                {
                    rest++;
                }

                total++;
            }

            return Json(new { code = 200, rest, total }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 停止投票项目.
        /// </summary>
        /// <param name="projectId">项目编号.</param>
        /// <returns>Json.</returns>
        public ActionResult StopProject(int projectId)
        {
            Project project = Db.Queryable<Project>().Where(it => it.Id == projectId).Single();
            project.Status = "结束投票";
            Db.Updateable(project).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除投票项目.
        /// </summary>
        /// <param name="projectId">项目编号.</param>
        /// <returns>Json.</returns>
        public ActionResult DelProject(int projectId)
        {
            Db.Deleteable<Project>().Where(it => it.Id == projectId).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}