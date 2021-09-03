using DDS.DCP.Models;
using DDS.DCP.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationUtility
{
    public interface IClientApplicationViewModel
    {
        ValueTask<string> LoadAgencyDatabaseAsync(CancellationToken token);

        ValueTask<bool> NotifyAdminAsync(string message, CancellationToken token);

        ValueTask<IEnumerable<string>> GetDataSourceAsync(CancellationToken token);

        ValueTask<TransmissionFileResponse> ProcessMetadataAsync(ItemMetadata data, CancellationToken token);

        ValueTask<bool> TransmitFilesAsync(TransmissionFileResponse response, CancellationToken token);
    }
}
