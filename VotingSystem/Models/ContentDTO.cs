// Copyright (c) PlaceholderCompany. All rights reserved.

namespace VotingSystem.Models
{
    /// <summary>
    /// 投票项目扩展实体类.
    /// </summary>
    public partial class ContentDTO : Content
    {
        /// <summary>
        /// Gets or sets 项目名称.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets 评审进度.
        /// </summary>
        public string Progress { get; set; }

        /// <summary>
        /// Gets or sets 评审方式.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets 票数.
        /// </summary>
        public string TicketsNum { get; set; }
    }
}