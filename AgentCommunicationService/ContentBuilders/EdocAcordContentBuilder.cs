using DDS.DCP.Models;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationService.ContentBuilders
{
    public class EdocAcordContentBuilder : IAcordContentBuilder
    {
        public object TransactionCodeMappings { get; set; }

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


            // TODO: Build ACORDCommonSvcRsActivityNoteRsFileAttachmentInfoItemIdInfo

            // TODO: Build ACORDCommonSvcRsActivityNoteRs

            // NOTE: Implement GetTransactionRemarkText

            // TODO: Build Acord content with database, service call and statically defined values based on transmission type
            return await Task.FromResult(acordContent);
        }

        public async ValueTask<string> BuildAcordFileContentAsync(ItemMetadata item, string content, CancellationToken token)
        {
            // NOTE: I thought the default interface implemetation would be
            // cleaner to call, which it is externally, but internally this
            // was the only way to access it.
            var edocItem = ((IAcordContentBuilder)this).GetItem<EdocMetadata>(item);

            StringBuilder builder = new StringBuilder();
            //builder.AppendLine(ServiceConstants.HEADER_MIME_VERSION);
            //builder.AppendLine(ServiceConstants.HEADER_CONTENT_TYPE_MULTIPART);
            builder.AppendLine("");
            //builder.AppendLine(ServiceConstants.FILE_BOUNDARY_HEADER);
            //builder.AppendLine(ServiceConstants.HEADER_CONTENT_TYPE);
            //builder.AppendLine(ServiceConstants.HEADER_CONTENT_ID_ACORD);
            builder.AppendLine(Environment.NewLine);
            builder.AppendLine(content);
            builder.AppendLine(Environment.NewLine);

            var contentDescription = Path.GetFileNameWithoutExtension(edocItem.AttachmentFileName);

            builder.AppendLine(Environment.NewLine);
            //builder.AppendLine(ServiceConstants.FILE_BOUNDARY_HEADER);
            //builder.AppendLine(ServiceConstants.CONTENT_TYPE);
            builder.AppendLine($"Content-id: {edocItem.AttachmentFileName}");
            builder.AppendLine($"Content-description: {contentDescription}");
            builder.AppendLine(Environment.NewLine);
            // Read content data
            byte[] bytes = File.ReadAllBytes(edocItem.AttachmentFilePath);
            // Convert to string with fixed length rule
            string fileAsBase64Str = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            builder.AppendLine(fileAsBase64Str);
            builder.AppendLine(Environment.NewLine);
            builder.Append(Environment.NewLine);
            //builder.AppendLine(ServiceConstants.FILE_BOUNDARY_FOOTER);
            // Return back final stream
            return await Task.FromResult(builder.ToString());
        }

        public async ValueTask<string> BuildDestinationFileNameAsync(ItemMetadata item, CancellationToken token)
        {
            var businessPurposeTypeCode = item.TransactionType;
            var fileNameSuffix = item.SequenceId.ToString();

            // This destination oath shoud be the full
            var destinationPath = $"{item.PolicyNumber.Replace(" ", "_")}_{businessPurposeTypeCode}_{fileNameSuffix}";
            return await Task.FromResult(destinationPath);
        }

        private async ValueTask<string> BuildContentFileName(ItemMetadata item, CancellationToken token)
        {
            // NOTE: Requirement [BR 006]
            //•	Policy Number
            //•	Insured Name
            //•	Type of Document(New Business, Endorsement, etc.)
            //•	Transaction Effective Date

            var ptItem = item as EdocMetadata;
            if (ptItem is not null)
            {
                var insuredName = ptItem.InsuredName.Trim();
                var documentType = ptItem.BusinessPurposeTypeCode; // itcBusinessPurposeTypeCd?.Code;
                var contentFileExtension = ptItem.DcnExtension; // Path.GetExtension(dcn);

                var fileName = $@"{ptItem.PolicyNumber}_{insuredName}_{documentType}_{ptItem.FormattedPolicyEffectiveDate}{contentFileExtension}";
                return await Task.FromResult(fileName);
            }
            throw new Exception("Metadata content is not for policy transactions.");
        }
    }

}
