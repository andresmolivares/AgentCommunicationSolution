using AgentCommunicationService.ContentBuilders;
using AgentCommunicationService.Validators;
using DDS.DCP.Requests;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgentCommunicationService.Controllers
{
    public partial class AgentComponent : IAgentComponent
    {
        private readonly IAgentRepository _repository;
        private readonly TransmissionValidatorFactory _validatorFactory;
        private readonly AcordContentBuilderFactory _contentBuilderFactory;

        public AgentComponent(
            IAgentRepository repository,
            TransmissionValidatorFactory validatorFactory,
            AcordContentBuilderFactory contentBuilderFactory)
        {
            _repository = repository;
            _validatorFactory = validatorFactory;
            _contentBuilderFactory = contentBuilderFactory;
        }

        public async Task ProcessAgencyDownloadAsync(CancellationToken token)
        {
            Console.WriteLine("Retrieve agency data source.");
            // TODO: Call the IVANS API for the agency database and persist on app server.
            // NOTE: We can simplify this by working with IVANS to create an API that returns a mailbox id based on a specified producer code

            // Initialize Ivans Service
            //var account = ServiceConstants.Helper.GetProperty(ServiceConstants.IVANS_SERVICE, ServiceConstants.ACCOUNT);
            //var password = ServiceConstants.Helper.GetProperty(ServiceConstants.IVANS_SERVICE, ServiceConstants.PASSWORD);
            //var endpoint = ServiceConstants.Helper.GetWebServicePathSettings(ServiceConstants.IVANS_ICE_SERVICE_KEY);
            //var iceService = new IceServicesReference.IceServicesClient(ServiceConstants.IVANS_ICE_CONFIG_KEY, endpoint);
            //var agencyDatabase = await iceService.ListProducersXMLAsync(account, password);


            //var agencyDatabasePath = GetDatabaseAgencyFilePath();
            //PersistDataToDisk(agencyDatabasePath, () => builder.ToString().ToStream());

            // NOTE: We can store a timestamp on the agencyDatabase file and reload if a configured timespan has elapsed.

            Console.WriteLine("Persist data source for session.");
            await Task.FromResult("GetAgencyDownloadAsync needs implementation.");
        }

        public async Task NotifyAdminAsync(string message, CancellationToken token)
        {
            // TODO: Use the service to notify team of activity
            Console.WriteLine("Team has been notififed.");
            await Task.Yield();
        }

        public async ValueTask<TransmissionFileResponse> ProcessMetadataAsync(MetadataProcessRequest request, CancellationToken token)
        {
            var response = new TransmissionFileResponse();
            var item = request.Item;
            var validator = _validatorFactory.GetTransmissionValidator(item.TransmissionType);
            if (validator.ValidateTransaction(item))
            {
                // Build content file
                var contentBuilder = _contentBuilderFactory.GetContentBuilder(item.TransmissionType);
                var content = await contentBuilder.BuildAcordContentAsync(item, token);
                var acordPayload = await contentBuilder.BuildAcordFileContentAsync(item, content, token);
                var destinationFileName = await contentBuilder.BuildDestinationFileNameAsync(item, token);
                var agencySubCode = item.AgencySubCode;

                // Build transfer file
                var accountMailbox = await GetAccountMailboxAsync(agencySubCode, token);
                var xfrPayload = await BuildTransferFileContentAsync(accountMailbox, destinationFileName, token);

                // Build response
                response.AcordPayload = acordPayload;
                response.XfrTransferPayload = xfrPayload;
                var destinationLocation = ""; // NOTE: Get from SIGI configuration
                response.DestinationFileLocation = Path.Combine(destinationLocation, destinationFileName);
            }
            return await Task.FromResult(response);
        }

        private async ValueTask<string> GetAccountMailboxAsync(string producerCode, CancellationToken token)
        {
            var accountMailbox = string.Empty;
            // TODO: Lookup and return mailbox for producer code
            await ProcessAgencyDownloadAsync(token);
            var xmlAgencyData = "";
            // NOTE: Need a method of getting IVANS agency database only once (possibly check xml file date attributes)
            var ivansAgencyDatabase = XDocument.Parse(xmlAgencyData.ToString());
            var producer = ivansAgencyDatabase.Descendants("Producer")
                .FirstOrDefault(e => e.Element("ProducerCode").Value == producerCode);
            return await Task.FromResult(accountMailbox);
        }

        private async ValueTask<bool> IsBusinessTypeSupportedAsync(string transactionType, string lob, CancellationToken token)
        {
            var supportedBusinessTypes = await _repository.GetSupportedBusinessTypesAsync(lob, token);
            var isSupported = supportedBusinessTypes.Contains(transactionType);
            return await Task.FromResult(isSupported);
        }

        private async ValueTask<object> GetAgentInfoAsync(string agentId, CancellationToken token)
        {
            return await _repository.GetAgentInfoAsync(agentId, token);
        }

        private async ValueTask<object> GetAcordCodeAsync(string codeId, CancellationToken token)
        {
            return await _repository.GetAcordCodeAsync(codeId, token);
        }

        private async ValueTask<object> GetNaicCodeAsync(string policyNumber, string termNumber, CancellationToken token)
        {
            // Note: Implement and use getTermNumber
            // Note: Implement and use getNAIC_CodeFromPolicyNumber
            // TODO: Implement calls to CLAS and PLUS services for policy info and NAIC code data
            return await Task.FromResult(new object());
        }

        private async ValueTask<object> GetBusinessPurposeTypeCodeAsync(string codeId, CancellationToken token)
        {
            var acordCodes = await _repository.GetAcordCodeAsync(codeId, token);
            return await Task.FromResult(acordCodes.FirstOrDefault());
        }

        private async ValueTask<string> GetPolicyLobCode(string policyPrefix)
        {
            return await _repository.GetPolicyNumberLob(policyPrefix);
        }

        private object GetTransCodeForTransactionType(string transactionType, string transmissionType, string activityNoteType)
        {
            //var transactiontype = dataXml.GetElementValueFromXPath(ServiceConstants.ACORD_TRANSACTION_TYPE_PATH);
            //// eDocs will always be PolicyTransaction. New request types for eDocs must implement an activitynotetype data point
            //var activitynotetype = dataXml.GetElementValueFromXPath(ServiceConstants.ACORD_ACTIVITY_NOTE_TYPE_PATH).IfEmptyNullify() ?? "PolicyTransaction";

            // NOTE: Get TransactionCodeMappings from database
            var TransactionCodeMappings = new dynamic[] { };

            var tcm = TransactionCodeMappings
                .ToList()
                .SingleOrDefault(i =>
                i.TransactionCode == transactionType &&
                i.TransmissionId == transmissionType &&
                i.ActivityNoteTypeCode == activityNoteType);
            if (tcm == null)
            {
                var message = @$"Could not find the business purpose type mapping for the 
transaction type {transactionType}, 
transmission type {transmissionType} and 
activity note type {activityNoteType} in the ECM data.";
                throw new NullReferenceException(message);
            }
            return tcm;
        }

        private async ValueTask<string> BuildTransferFileContentAsync(string targetMailbox, string contentFileName, CancellationToken token)
        {
            var builder = new StringBuilder();
            builder.AppendLine(targetMailbox);
            builder.AppendLine(contentFileName);

            // Return back final stream
            return await Task.FromResult(builder.ToString());
        }
    }
}
