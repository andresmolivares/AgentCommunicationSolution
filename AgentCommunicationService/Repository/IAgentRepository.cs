using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.Controllers
{
    public interface IAgentRepository
    {
        ValueTask<object> GetAgentInfoAsync(string agentSubCode, CancellationToken token);

        ValueTask<IEnumerable<object>> GetAcordCodeAsync(string codeId, CancellationToken token);

        ValueTask<IEnumerable<object>> GetBusinessPurposeTypeCodeAsync(string codeId, CancellationToken token);

        ValueTask<IEnumerable<object>> GetVendorSystemsAsync(CancellationToken token);

        ValueTask<IEnumerable<object>> GetLineOfBusinessesAsync(CancellationToken token);

        ValueTask<IEnumerable<object>> GetDefaultTransacionsAsync(CancellationToken token);

        ValueTask<IEnumerable<object>> GetTransactionStatusesAsync(CancellationToken token);

        ValueTask<IEnumerable<object>> GetTransactionMappingsAsync(CancellationToken token);

        ValueTask<IEnumerable<object>> GetNotificationEvents(CancellationToken token);

        ValueTask<IEnumerable<object>> GetSupportedBusinessTypesAsync(string lob, CancellationToken token);

        Task AddProcessLogItemAsync(string logItem);

        ValueTask<object> GetProcessLogAsync(CancellationToken token);

        ValueTask<string> GetPolicyNumberLob(string policyPrefix);
    }
}
