#nullable enable

using DDS.DCP.Extensions;
using System;
using System.Xml.Linq;

namespace DDS.DCP.Models
{
    public record ItemMetadata
    {

        public ItemMetadata(
            string metatdataFilePath,
            int sequenceId,
            TransmissionTypeEnum transmissionType)
        {
            MetadataFilePath = metatdataFilePath;
            SequenceId = sequenceId;
            TransmissionType = transmissionType;
        }

        public virtual void ParseMetadata()
        {
            if (!string.IsNullOrWhiteSpace(MetadataFilePath))
            {
                var document = XDocument.Load(MetadataFilePath);
                AgencySubCode = document.GetElementValueFromXPath(AcordPaths.ACORD_AGENCY_CODE_PATH);
                PolicyNumber = document.GetElementValueFromXPath(AcordPaths.ACORD_POLICY_NUMBER_PATH);
                //Ecn = document.GetElementValueFromXPath(AcordPaths.ACORD_ECN_PATH);
                InsuredName = document.GetElementValueFromXPath(AcordPaths.ACORD_INSURED_NAME_PATH);
                var effectiveDate = document.GetElementValueFromXPath(AcordPaths.ACORD_POLICY_EFFDATE_PATH);
                PolicyEffectiveDate = DateTime.TryParse(effectiveDate, out var effdate) ? effdate : throw new Exception();
                var expirationDate = document.GetElementValueFromXPath(AcordPaths.ACORD_POLICY_EXPDATE_PATH);
                PolicyExpirationDate = DateTime.TryParse(expirationDate, out var expdate) ? expdate : throw new Exception();
                TransactionType = document.GetElementValueFromXPath(AcordPaths.ACORD_TRANSACTION_TYPE_PATH);
                ActivityNoteType = document.GetElementValueFromXPath(AcordPaths.ACORD_ACTIVITY_NOTE_TYPE_PATH);
            }
        }

        public string? MetadataFilePath { get; set; }

        public int SequenceId { get; set; }

        public ProcessStatusTypeEnum ProcessStatusType { get; set; } = ProcessStatusTypeEnum.Unread;

        public TransmissionTypeEnum TransmissionType { get; set; }

        public short VendorSystemId { get; set; }

        public string? AgencySubCode { get; set; }

        public string? InsuredName { get; set; }

        public string? BusinessPurposeTypeCode { get; set; }

        public string? TransactionType { get; set; }

        public string? ActivityNoteType { get; set; }

        public string? PolicyNumber { get; set; }

        public DateTime PolicyEffectiveDate { get; set; }

        public DateTime PolicyExpirationDate { get; set; }

        public string? FormattedPolicyEffectiveDate { get; set; }

        public string? DestinationPath { get; set; }

        public string? DestinationFileName { get; set; }
    }
}
