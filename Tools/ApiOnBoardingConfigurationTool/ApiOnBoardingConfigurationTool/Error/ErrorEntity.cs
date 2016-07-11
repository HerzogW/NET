namespace ApiOnBoardingConfigurationTool
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEntity"/> class.
        /// </summary>
        public ErrorEntity()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEntity"/> class.
        /// </summary>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="zipFileName">Name of the zip file.</param>
        public ErrorEntity(ErrorType errorType, string zipFileName)
        {
            this.errorType = errorType;
            this.apiFolderName = zipFileName;
        }
        public ErrorEntity(ErrorType errorType, string zipFileName,string jsonFileName)
        {
            this.errorType = errorType;
            this.apiFolderName = zipFileName;
            this.jsonFileName = jsonFileName;
        }
        public ErrorEntity(ErrorType errorType, string zipFileName, string jsonFileName,string itemName)
        {
            this.errorType = errorType;
            this.apiFolderName = zipFileName;
            this.jsonFileName = jsonFileName;
            this.itemName = itemName;
        }
        public ErrorEntity(ErrorType errorType, string zipFileName, string jsonFileName, string itemName,ErrorStatus errorStatus)
        {
            this.errorType = errorType;
            this.apiFolderName = zipFileName;
            this.jsonFileName = jsonFileName;
            this.itemName = itemName;
            this.errorStatus = errorStatus;
        }

        private ErrorStatus status = ErrorStatus.NotFixed;

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
        public string apiFolderName { get; set; }

        /// <summary>
        /// Gets or sets the name of the json file.
        /// </summary>
        /// <value>
        /// The name of the json file.
        /// </value>
        public string jsonFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        public string itemName { get; set; }

        /// <summary>
        /// Gets or sets the error detail.
        /// </summary>
        /// <value>
        /// The error detail.
        /// </value>
        public string errorDetail { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string exceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the error status.
        /// </summary>
        /// <value>
        /// The error status.
        /// </value>
        public ErrorStatus errorStatus
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        /// <summary>
        /// Gets the error information.
        /// </summary>
        /// <returns></returns>
        public string GetErrorInfo()
        {
            string errorInfo = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.apiFolderName))
            {
                errorInfo = string.Format("【Folder   {0}】:   ", this.apiFolderName);
            }

            switch (errorType)
            {
                case ErrorType.None:
                    return string.Empty;

                case ErrorType.NullValue:
                    errorInfo = string.Format("{0}【File   {1}】:   【Item   {2}】 not found the item.", errorInfo, this.jsonFileName, this.itemName);
                    break;
                case ErrorType.Common:
                    break;
                case ErrorType.NotFound:
                    errorInfo = string.Format("{0}【File   {1}】 not found.", errorInfo, this.jsonFileName);
                    break;
                case ErrorType.CanNotConvertToJson:
                    errorInfo = string.Format("{0}【File   {1}】 can not be converted to Json.", errorInfo, this.jsonFileName);
                    break;
                case ErrorType.LostResource:
                    errorInfo = string.Format("{0}【File   {1}】:   【Resource   {2}】 not found.", errorInfo, this.jsonFileName, this.itemName);
                    break;
                case ErrorType.NotSetAsResourceItem:
                    errorInfo = string.Format("{0}【File   {1}】:   【Item   {2}】 not set as resource item.", errorInfo, this.jsonFileName, this.itemName);
                    break;
                case ErrorType.NotSetAsSvgFile:
                    errorInfo = string.Format("{0}【File   {1}】:   【Item   {2}】 not set as SVG file.", errorInfo, this.jsonFileName, this.itemName);
                    break;
                case ErrorType.CanNotDeserialize:
                    errorInfo = string.Format("{0}【File   {1}】 can not be deserialized to Object.", errorInfo, this.jsonFileName);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(errorDetail))
            {
                errorInfo = string.Format("{0}\r\nError Detail:   {1}", errorInfo, errorDetail);
            }
            if (!string.IsNullOrWhiteSpace(exceptionMessage))
            {
                errorInfo = string.Format("{0}\r\nOriginal Exception Message:   {1}", errorInfo, exceptionMessage);
            }

            return string.Format("{0}\r\n【***STATUS ： {1}***】\r\n", errorInfo, errorStatus);
        }
    }
}
