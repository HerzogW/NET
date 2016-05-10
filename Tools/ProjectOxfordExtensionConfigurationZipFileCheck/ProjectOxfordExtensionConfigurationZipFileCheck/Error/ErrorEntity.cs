namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorEntity
    {
        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>
        /// The type of the error.
        /// </value>
        public ErrorType errorType { get; set; }

        /// <summary>
        /// Gets or sets the name of the zip file.
        /// </summary>
        /// <value>
        /// The name of the zip file.
        /// </value>
        public string zipFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the json file.
        /// </summary>
        /// <value>
        /// The name of the json file.
        /// </value>
        public string jsonFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string resourceName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string errorMessage { get; set; }

        /// <summary>
        /// Gets the error information.
        /// </summary>
        /// <returns></returns>
        public string GetErrorInfo()
        {
            string errorInfo = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.zipFileName))
            {
                errorInfo = string.Format("【Zip   {0}】:   ", this.zipFileName);
            }

            switch (errorType)
            {
                case ErrorType.Common:
                    errorInfo = string.Format("{0}{1}", errorInfo, errorMessage);
                    break;
                case ErrorType.NotFound:
                    errorInfo = string.Format("{0}【File   {1}】 not found.", errorInfo, this.jsonFileName);
                    break;
                case ErrorType.CanNotConvertToJson:
                    errorInfo = string.Format("{0}【File   {1}】 can not be converted to Json.", errorInfo, this.jsonFileName);
                    break;
                case ErrorType.LostResource:
                    errorInfo = string.Format("{0}【File   {1}】:   【Resource   {2}】 not found", errorInfo, this.jsonFileName, this.resourceName);
                    break;
                case ErrorType.CanNotDeserialize:
                    errorInfo = string.Format("{0}【File   {1}】 can not be deserialized to Object.", errorInfo, this.jsonFileName);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                errorInfo = string.Format("{0}\r\nOriginal Error Message:   {1}", errorInfo, errorMessage);
            }

            return string.Format("{0}\r\n", errorInfo);
        }
    }
}
