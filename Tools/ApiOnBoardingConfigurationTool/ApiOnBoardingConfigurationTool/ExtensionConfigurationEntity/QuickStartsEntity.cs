
namespace ApiOnBoardingConfigurationTool
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class QuickStartsEntity
    {
        /// <summary>
        /// Gets or sets the quick starts.
        /// </summary>
        /// <value>
        /// The quick starts.
        /// </value>
        public List<QuickStartUnit> quickStarts { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QuickStartUnit
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string description { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string icon { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public List<QuickStartLink> links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QuickStartLink
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string text { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public string uri { get; set; }
    }
}
