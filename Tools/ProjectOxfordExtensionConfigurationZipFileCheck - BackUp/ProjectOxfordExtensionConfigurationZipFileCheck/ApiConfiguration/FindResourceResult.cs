
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    /// <summary>
    /// 
    /// </summary>
    public class FindResourceResult
    {
        public FindResourceResultStatus status { get; set; }

        public string resourceInfo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FindResourceResultStatus
    {
        Ok,
        NotFound
    }
}
