// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.Models
{
    /// <summary>
    /// 投票项目实体类.
    /// </summary>
    [SugarTable("expert")]
    public partial class Expert
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 编号.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 对应的投票项目编号.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets 状态.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets 账号.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets 名字.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 二维码路径.
        /// </summary>
        public string CodePath { get; set; }
    }
}