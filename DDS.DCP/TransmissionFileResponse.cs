using System.IO;

namespace DDS.DCP.Requests
{
    public record TransmissionFileResponse
    {
        public string AcordPayload { get; set; }

        public string XfrTransferPayload { get; set; }

        public string DestinationFileLocation { get; set; }
    }
}
