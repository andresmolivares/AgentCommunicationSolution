#nullable enable

using System.IO;

namespace DDS.DCP.Models
{
    public record EdocMetadata : ItemMetadata
    {
        public EdocMetadata(
            string metatdataFilePath,
            int sequenceId) :
            base(metatdataFilePath, sequenceId, TransmissionTypeEnum.eDoc)
        { }

        public string? Dcn { get; set; }

        public string? DcnExtension { get; set; }

        public string? AttachmentFilePath { get; set; }

        public string? AttachmentFileName => AttachmentFilePath != null
            ? Path.GetFileName(AttachmentFilePath)
            : string.Empty;
    }
}
