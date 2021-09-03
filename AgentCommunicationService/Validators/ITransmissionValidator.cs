using DDS.DCP.Models;

namespace AgentCommunicationService.Validators
{
    public interface ITransmissionValidator : IItemMetadataGetter
    {
        bool ValidateTransaction(ItemMetadata item);
    }
}
