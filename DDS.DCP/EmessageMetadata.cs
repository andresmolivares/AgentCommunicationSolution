#nullable enable


namespace DDS.DCP.Models
{
    public record EmessageMetadata : ItemMetadata
    {
        public EmessageMetadata(
                string metatdataFilePath,
                int sequenceId) :
                base(metatdataFilePath, sequenceId, TransmissionTypeEnum.eMessage)
        { }
    }
}
