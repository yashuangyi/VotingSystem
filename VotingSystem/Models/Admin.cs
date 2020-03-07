// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.Models
{
    /// <summary>
    /// 登录实体类.
    /// </summary>
    [SugarTable("admin")]
    public partial class Admin
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 用户名.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        public string Password { get; set; }
    }
}