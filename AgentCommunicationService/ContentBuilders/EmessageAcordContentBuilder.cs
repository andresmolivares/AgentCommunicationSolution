using DDS.DCP.Models;
using DDS.DCP.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.ContentBuilders
{
    public class EmessageAcordContentBuilder : IAcordContentBuilder
    {
        public async ValueTask<string> BuildAcordContentAsync(ItemMetadata item, CancellationToken token)
        {
            var acordHeader = string.Empty;
            var acordContent = string.Empty;
            //var agentInfo = await GetAgentInfoAsync(request.AgentSubCode, token);
            //var vendoSystems = await _repository.GetVendorSystemsAsync(token);
            // TODO: Build Acord header with database and statically defined values

            // TODO: Build ACORDCommonSvcRsActivityNoteRsItemIdInfo

            // TODO: Build ACORDCommonSvcRsActivityNoteRsMsgStatus

            // TODO: Build ACORDCommonSvcRsActivityNoteRsInsuredOrPrincipalItemIdInfo
            // NOTE: Create function for GetNameInfo

            // TODO: Build ACORDCommonSvcRsActivityNoteRsPartialPolicy
            //GetTransCodeForTransactionType(
            //    request.TransactionType,
            //    ((int)request.TransmissionType).ToString(),
            //    request.ActivityNoteType);

            // TODO: Build ACORDCommonSvcRsActivityNoteRs

            // NOTE: Implement GetTransactionRemarkText

            // NOTE: Build ACORDCommonSvcRsClaimsOccurrenceRs for valid claims transactions

            // TODO: Build Acord content with database, service call and statically defined values based on transmission type
            return await Task.FromResult(acordContent);
        }

        public async ValueTask<string> BuildAcordFileContentAsync(ItemMetadata item, string content, CancellationToken token)
        {
            var emessageItem = ((IAcordContentBuilder)this).GetItem<EmessageMetadata>(item);
            return await Task.FromResult(content);
        }

        public async ValueTask<string> BuildDestinationFileNameAsync(ItemMetadata item, CancellationToken token)
        {
            var businessPurposeTypeCode = item.TransactionType;
            var isEmessage = item.TransmissionType == TransmissionTypeEnum.eMessage;
            var isClaimsActivity = new[] { "ClaimStatusUpdate", "ClaimFNOL" }.Contains(item.ActivityNoteType);
            var isClaimsEmessage = isEmessage && isClaimsActivity;
            var fileNameSuffix = item.SequenceId.ToString();

            if (isClaimsEmessage)
            {
                var claimNumber = string.Empty;
                // TODO: Get claim number
                fileNameSuffix = claimNumber;
            }

            // This destination oath shoud be the full
            var destinationPath = $"{item.PolicyNumber.Replace(" ", "_")}_{businessPurposeTypeCode}_{fileNameSuffix}";
            return await Task.FromResult(destinationPath);
        }
    }
}
