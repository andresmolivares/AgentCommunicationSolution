using DDS.DCP.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentComponent _component;

        public AgentController(IAgentComponent component)
        {
            _component = component;
        }

        [HttpPost(Constants.POST_URI_NOTIFY_ADMIN)]
        public async Task PostNotifyAdminAsync(string message, CancellationToken token)
        {
            await _component.NotifyAdminAsync(message, token);
        }

        [HttpPost(Constants.POST_URI_AGENCY_DOWNLOAD)]
        public async Task PostAgencyDownloadAsync(CancellationToken token)
        {
            await _component.ProcessAgencyDownloadAsync(token);
        }

        [HttpPut(Constants.PUT_URI_PROCESS_METADATA)]
        public async Task PutProcessMetadataAsync(MetadataProcessRequest request, CancellationToken token)
        {
            await _component.ProcessMetadataAsync(request, token);
        }
    }
}
