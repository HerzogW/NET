

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class FindResourceResult
    {
        public FindResourceResultStatus status { get; set; }

        public string resourceInfo { get; set; }
    }

    public enum FindResourceResultStatus
    {
        Ok,
        NotFound
    }
}
