using DDS.DCP.Models;

namespace AgentCommunicationService.ContentBuilders
{
    public class AcordContentBuilderFactory
    {
        public IAcordContentBuilder GetContentBuilder(TransmissionTypeEnum transmissionType)
        {
            return transmissionType switch
            {
                TransmissionTypeEnum.eDoc => new EdocAcordContentBuilder(),
                TransmissionTypeEnum.eMessage => new EmessageAcordContentBuilder(),
                _ => null,
            };
        }
    }
}
