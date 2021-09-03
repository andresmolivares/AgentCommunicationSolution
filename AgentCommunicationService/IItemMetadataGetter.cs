using DDS.DCP.Models;

namespace AgentCommunicationService
{
    public interface IItemMetadataGetter
    {
        T GetItem<T>(ItemMetadata item) where T : ItemMetadata => item as T;
    }
}