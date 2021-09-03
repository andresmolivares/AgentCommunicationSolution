using DDS.DCP.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.ContentBuilders
{
    public interface IAcordContentBuilder : IItemMetadataGetter
    {
        ValueTask<string> BuildAcordContentAsync(ItemMetadata item, CancellationToken token);

        ValueTask<string> BuildAcordFileContentAsync(ItemMetadata item, string content, CancellationToken token);

        ValueTask<string> BuildDestinationFileNameAsync(ItemMetadata item, CancellationToken token);
    }
}