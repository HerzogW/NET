
namespace ApiOnBoardingConfigurationTool
{
    /// <summary>
    /// 
    /// </summary>
    public class FindResourceResult
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public FindResourceResultStatus status { get; set; }

        /// <summary>
        /// Gets or sets the resource information.
        /// </summary>
        /// <value>
        /// The resource information.
        /// </value>
        public string resourceInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FindResourceResultStatus
    {
        /// <summary>
        /// The ok
        /// </summary>
        Ok,

        /// <summary>
        /// The not found
        /// </summary>
        NotFound
    }
}
