#nullable enable

using DDS.DCP.Models;

namespace DDS.DCP.Requests
{
    public record MetadataProcessRequest
    {
        public ItemMetadata? Item { get; set; }
    }

}
