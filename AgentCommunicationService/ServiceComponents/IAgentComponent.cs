using DDS.DCP;
using DDS.DCP.Models;
using DDS.DCP.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.Controllers
{
    public interface IAgentComponent
    {
        Task ProcessAgencyDownloadAsync(CancellationToken token);

        Task NotifyAdminAsync(string message, CancellationToken token);

        ValueTask<TransmissionFileResponse> ProcessMetadataAsync(MetadataProcessRequest request, CancellationToken token);
    }
}
