// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.Models
{
    /// <summary>
    /// 每个评委对每个内容的评审记录实体类.
    /// </summary>
    [SugarTable("record")]
    public partial class Record
    {
        // 指定主键和自增列

        /// <summary>
        /// Gets or sets 编号.
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 对应的投票内容编号.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets 对应的评委编号.
        /// </summary>
        public int ExpertId { get; set; }

        /// <summary>
        /// Gets or sets 记录值.
        /// </summary>
        public int? Value { get; set; }
    }
}