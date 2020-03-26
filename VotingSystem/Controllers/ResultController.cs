using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
                    FirstPrizeNum = content.FirstPrizeNum,
                    SecondPrizeNum = content.SecondPrizeNum,
                    ThirdPrizeNum = content.ThirdPrizeNum,
                    GiveupNum = content.GiveupNum,
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
                    FirstPrizeNum = content.FirstPrizeNum,
                    SecondPrizeNum = content.SecondPrizeNum,
                    ThirdPrizeNum = content.ThirdPrizeNum,
                    GiveupNum = content.GiveupNum,
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

        /// <summary>
        /// 统计结果代码，价值5毛.
        /// </summary>
        /// <param name="projectName">项目名称.</param>
        /// <returns>Json.</returns>
        public ActionResult GetResult(string projectName)
        {
            Project project = Db.Queryable<Project>().Where(it => it.Name == projectName).Single();
            if (project.Status == "未启动" || project.Status == "进行中")
            {
                return Json(new { code = 401, msg = "该项目未结束评审，无法统计结果！" }, JsonRequestBehavior.AllowGet);
            }

            int projectId = project.Id;
            if (project.Method == "投票")
            {
                // 此处为一等奖票、二等奖票、三等奖票的权值
                int firstPrizeVal = 5;
                int secondPrizeVal = 3;
                int thirdPrizeVal = 1;
                int giveupVal = (firstPrizeVal + secondPrizeVal + thirdPrizeVal) / 3;

                List<Content> contentsBefore = Db.Queryable<Content>().Where(it => it.ProjectId == projectId).ToList();
                foreach (Content content in contentsBefore)
                {
                    content.Score = (firstPrizeVal * content.FirstPrizeNum) + (secondPrizeVal * content.SecondPrizeNum) + (thirdPrizeVal * content.ThirdPrizeNum) + (giveupVal * content.GiveupNum);
                }

                Db.Updateable(contentsBefore).ExecuteCommand();
            }

            List<Content> contents = Db.Queryable<Content>().Where(it => it.ProjectId == projectId).OrderBy(it => it.Score, OrderByType.Desc).ToList();
            int num = 1;
            for (int i = 0; i < contents.Count(); i++)
            {
                contents[i].Result = "第" + num + "名";
                if (i != contents.Count() - 1 && contents[i] != contents[i + 1])
                {
                    num++;
                }
            }

            Db.Updateable(contents).ExecuteCommand();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出Excel.
        /// </summary>
        /// <param name="projectName">项目名称.</param>
        /// <returns>Excel.</returns>
        public ActionResult OutputResult(string projectName)
        {
            Project project = Db.Queryable<Project>().Where(it => it.Name == projectName).Single();
            int projectId = project.Id;
            List<ContentDTO> list = Db.SqlQueryable<ContentDTO>("select a.*,b.name,b.method from content a,project b where a.projectId=b.id and a.projectId=" + projectId).ToList();
            if (project.Status == "完成统计")
            {
                list.OrderBy(it => it.Result);
            }
            else
            {
                list.OrderBy(it => it.Id);
            }

            // 创建Excel对象 工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();

            // 创建Excel工作表 Sheet
            ISheet sheet = excelBook.CreateSheet("内容信息");
            if (project.Method == "评分")
            {
                // 标题
                IRow rowTitle = sheet.CreateRow(0);
                rowTitle.CreateCell(0).SetCellValue("项目名称");
                rowTitle.CreateCell(1).SetCellValue("评审编号");
                rowTitle.CreateCell(2).SetCellValue("评审内容");
                rowTitle.CreateCell(3).SetCellValue("评审方式");
                rowTitle.CreateCell(4).SetCellValue("分数");
                rowTitle.CreateCell(5).SetCellValue("评审结果");

                for (int i = 0; i < list.Count(); i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    rowTitle.CreateCell(0).SetCellValue(list[i].ProjectName);
                    rowTitle.CreateCell(1).SetCellValue(list[i].Number);
                    rowTitle.CreateCell(2).SetCellValue(list[i].Name);
                    rowTitle.CreateCell(3).SetCellValue(list[i].Method);
                    rowTitle.CreateCell(4).SetCellValue(list[i].Score);
                    rowTitle.CreateCell(5).SetCellValue(list[i].Result);
                }
            }
            else
            {
                // 标题
                IRow rowTitle = sheet.CreateRow(0);
                rowTitle.CreateCell(0).SetCellValue("项目名称");
                rowTitle.CreateCell(1).SetCellValue("评审编号");
                rowTitle.CreateCell(2).SetCellValue("评审内容");
                rowTitle.CreateCell(3).SetCellValue("评审方式");
                rowTitle.CreateCell(4).SetCellValue("一等奖票数");
                rowTitle.CreateCell(5).SetCellValue("二等奖票数");
                rowTitle.CreateCell(6).SetCellValue("三等奖票数");
                rowTitle.CreateCell(7).SetCellValue("弃权票数");
                rowTitle.CreateCell(8).SetCellValue("分数");
                rowTitle.CreateCell(9).SetCellValue("评审结果");

                for (int i = 0; i < list.Count(); i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    rowTitle.CreateCell(0).SetCellValue(list[i].ProjectName);
                    rowTitle.CreateCell(1).SetCellValue(list[i].Number);
                    rowTitle.CreateCell(2).SetCellValue(list[i].Name);
                    rowTitle.CreateCell(3).SetCellValue(list[i].Method);
                    rowTitle.CreateCell(4).SetCellValue(list[i].FirstPrizeNum);
                    rowTitle.CreateCell(5).SetCellValue(list[i].SecondPrizeNum);
                    rowTitle.CreateCell(6).SetCellValue(list[i].ThirdPrizeNum);
                    rowTitle.CreateCell(7).SetCellValue(list[i].GiveupNum);
                    rowTitle.CreateCell(8).SetCellValue(list[i].Score);
                    rowTitle.CreateCell(9).SetCellValue(list[i].Result);
                }
            }

            string fileName = project.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";
            MemoryStream stream = new MemoryStream();
            excelBook.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}