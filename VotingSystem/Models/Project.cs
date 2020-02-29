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
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        /// <summary>
        /// Gets or sets 编号.
        /// </summary>
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
        /// Gets or sets 状态.
        /// </summary>
        public string Status { get; set; }
    }
}