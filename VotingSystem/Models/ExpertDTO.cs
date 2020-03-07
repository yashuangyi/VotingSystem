
namespace VotingSystem.Models
{
    /// <summary>
    /// 加上项目名称属性.
    /// </summary>
    public class ExpertDTO : Expert
    {
        /// <summary>
        /// Gets or sets 项目名称.
        /// </summary>
        public string ProjectName { get; set; }
    }
}