using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class ErrorEntity
    {
        public ErrorType errorType { get; set; }

        public string zipFileName { get; set; }

        public string jsonFileName { get; set; }

        public string resourceName { get; set; }

        public string errorMessage { get; set; }

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
