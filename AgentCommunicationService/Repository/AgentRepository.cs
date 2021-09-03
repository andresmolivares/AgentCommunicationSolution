using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.Controllers
{
    public class AgentRepository : IAgentRepository
    {
        public async ValueTask<IEnumerable<object>> GetBusinessPurposeTypeCodeAsync(string codeId, CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetAcordCodeAsync(string codeId, CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<object> GetAgentInfoAsync(string agentId, CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetSupportedBusinessTypesAsync(string lob, CancellationToken token)
        {
            // NOTE: Refactor this so that this information is retrievd by a proc
            switch (lob)
            {
                case "CL":
                    var typesForCLAS = new string[] { "ADCA", "ADFN", "ADRV", "CACA", "CARS", "IDCA", "NBIS", "PCNM", "RCIS", "REIS", "RETO", "XFER" }; // "ERRC", "PRMT"
                    return await Task.FromResult(typesForCLAS);
                case "PL":
                    var typesForPLUS = new string[] { "CRIS", "NBIS", "PCNM", "REIS" };
                    return await Task.FromResult(typesForPLUS);
            }
            return await Task.FromResult(new[] { string.Empty });
        }

        public async ValueTask<IEnumerable<object>> GetVendorSystemsAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetLineOfBusinessesAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetDefaultTransacionsAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetTransactionStatusesAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetTransactionMappingsAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async Task AddProcessLogItemAsync(string logItem)
        {
            await Task.Yield();
            //return await Task.FromResult(new List<object>());
        }

        public async ValueTask<object> GetProcessLogAsync(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<IEnumerable<object>> GetNotificationEvents(CancellationToken token)
        {
            await Task.Yield();
            return await Task.FromResult(new List<object>());
        }

        public async ValueTask<string> GetPolicyNumberLob(string policyPrefix)
        {
            var lobCode = policyPrefix.Trim() switch
            {
                "S" => "CPKGE",     // Commercial Package - 360
                "A" => "AUTOB",     // Commercial Auto - 341
                "B" => "CRIM",      // Crime - 362
                "F" => "AUTOP",     // Personal Auto - 342
                "H" => "HOME",      // Homeowners - 381
                "PCL" => "UMBRP",   // Umbrella - Personal (excess indemnity) - 422
                "WC" => "WORK",     // Worker's Compensation - 426
                "MY" => "OLIB",     // Other (Management) Liability - 401
                "DP" => "DFIRE",    // Dwelling Fire - 365
                "M" => "BOP",       // Business Owners Policy - 348
                "UB" => "UMBRC",    // Umbrella - Commercial
                _ => string.Empty,
            };
            return await Task.FromResult(lobCode);
        }
    }
}
