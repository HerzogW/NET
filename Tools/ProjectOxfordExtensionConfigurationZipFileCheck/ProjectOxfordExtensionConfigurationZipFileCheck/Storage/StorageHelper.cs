using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Web;


namespace ProjectOxfordExtensionConfigurationZipFileCheck.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageHelper
    {
        /// <summary>
        /// Get container data.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="storageAccountKey">The storage account key.</param>
        /// <param name="storageAccount">The storage account.</param>
        /// <param name="container">The container name.</param>
        /// <returns>Returns the container data.</returns>
        public static T GetContainerList<T>(
            string storageAccountKey,
            string storageAccount,
            string container)
        {
            DateTime dt = DateTime.UtcNow;
            string stringToSign = string.Format("GET\n"
                + "\n" // content encoding
                + "\n" // content language
                + "\n" // content length
                + "\n" // content md5
                + "\n" // content type
                + "\n" // date
                + "\n" // if modified since
                + "\n" // if match
                + "\n" // if none match
                + "\n" // if unmodified since
                + "\n" // range
                + "x-ms-date:" + dt.ToString("R") + "\nx-ms-version:2012-02-12\n" // headers
                + "/{0}/{1}\ncomp:list\nrestype:container", storageAccount, container);

            string auth = CreateAuthorizationHeader(
                stringToSign,
                storageAccountKey,
                storageAccount);

            string method = "GET";
            string urlPath = string.Format(
                "https://{0}.blob.core.windows.net/{1}?restype=container&comp=list",
                storageAccount,
                container);

            Uri uri = new Uri(urlPath);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Headers.Add("x-ms-date", dt.ToString("R"));
            request.Headers.Add("x-ms-version", "2012-02-12");
            request.Headers.Add("Authorization", auth);

            T data = default(T);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                    settings.XmlResolver = null;

                    var xmlReader = System.Xml.XmlReader.Create(reader, settings);

                    data = (T)serializer.Deserialize(xmlReader);

                }
            }

            return data;
        }

        /// <summary>
        /// Get blob response.
        /// </summary>
        /// <param name="storageAccountKey">The storage account key.</param>
        /// <param name="storageAccount">The storage account.</param>
        /// <param name="container">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>Returns the blob response.</returns>
        public static HttpWebResponse GetBlobResponse(
            string storageAccountKey,
            string storageAccount,
            string container,
            string fileName)
        {
            fileName = HttpUtility.UrlPathEncode(fileName);
            DateTime dt = DateTime.UtcNow;
            string stringToSign = string.Format("GET\n"
                + "\n" // content encoding
                + "\n" // content language
                + "\n" // content length
                + "\n" // content md5
                + "\n" // content type
                + "\n" // date
                + "\n" // if modified since
                + "\n" // if match
                + "\n" // if none match
                + "\n" // if unmodified since
                + "\n" // range
                + "x-ms-date:" + dt.ToString("R") + "\nx-ms-version:2014-02-14\n" // headers
                + "/{0}/{1}/{2}", storageAccount, container, fileName);

            string auth = CreateAuthorizationHeader(
                stringToSign,
                storageAccountKey,
                storageAccount);

            string method = "GET";
            string urlPath = string.Format(CultureInfo.InvariantCulture,
                "https://{0}.blob.core.windows.net/{1}/{2}",
                storageAccount,
                container,
                fileName);

            Uri uri = new Uri(urlPath);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Headers.Add("x-ms-date", dt.ToString("R"));
            request.Headers.Add("x-ms-version", "2014-02-14");
            request.Headers.Add("Authorization", auth);

            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Create authorization header.
        /// </summary>
        /// <param name="canonicalizedString">The canonicalized string.</param>
        /// <param name="storageAccountKey">The storage account key.</param>
        /// <param name="storageAccount">The storage account.</param>
        /// <returns>Returns the authorization header string.</returns>
        private static string CreateAuthorizationHeader(
            string canonicalizedString,
            string storageAccountKey,
            string storageAccount)
        {
            string signature = string.Empty;
            using (HMACSHA256 hmacSha256 = new HMACSHA256(Convert.FromBase64String(storageAccountKey)))
            {
                byte[] dataToHmac = System.Text.Encoding.UTF8.GetBytes(canonicalizedString);
                signature = Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }

            string authorizationHeader = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}:{2}",
                "SharedKey",
                storageAccount,
                signature);

            return authorizationHeader;
        }
    }
}
