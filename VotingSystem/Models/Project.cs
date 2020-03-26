// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.Models
{
    /// <summary>
    /// 投票项目实体类.
    /// </summary>
    [SugarTable("project")]
    public partial class Project
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 编号.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 备注.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets 评审方法.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets 开始时间.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets 结束时间.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets 状态（未启动、进行中、结束投票、完成统计）.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets 专家数.
        /// </summary>
        public int ExpertCount { get; set; }

        /// <summary>
        /// Gets or sets 待读取文件的路径.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets 内容数.
        /// </summary>
        public int ContentNum { get; set; }

        /// <summary>
        /// Gets or sets 已投票的评委数.
        /// </summary>
        public int HasVote { get; set; }
    }
}