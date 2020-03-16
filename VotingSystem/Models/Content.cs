// Copyright (c) PlaceholderCompany. All rights reserved.

using SqlSugar;

namespace VotingSystem.Models
{
    /// <summary>
    /// 投票项目实体类.
    /// </summary>
    [SugarTable("content")]
    public partial class Content
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
        /// Gets or sets 结果(投票过程中充当记录值).
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets 一等奖票数.
        /// </summary>
        public int FirstPrizeNum { get; set; }

        /// <summary>
        /// Gets or sets 二等奖票数.
        /// </summary>
        public int SecondPrizeNum { get; set; }

        /// <summary>
        /// Gets or sets 三等奖票数.
        /// </summary>
        public int ThirdPrizeNum { get; set; }

        /// <summary>
        /// Gets or sets 弃权票数.
        /// </summary>
        public int GiveupNum { get; set; }

        /// <summary>
        /// Gets or sets 评分分数.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets 编号.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets 名字.
        /// </summary>
        public string Name { get; set; }
    }
}