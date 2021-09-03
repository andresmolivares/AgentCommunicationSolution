using DDS.DCP.Models;

namespace AgentCommunicationService.Validators
{
    public class TransmissionValidatorFactory
    {
        public ITransmissionValidator GetTransmissionValidator(TransmissionTypeEnum transmissionType)
        {
            return transmissionType switch
            {
                TransmissionTypeEnum.eDoc => new EdocTransmissionValidator(),
                TransmissionTypeEnum.eMessage => new EmessageTransmissionValidator(),
                _ => null,
            };
        }
    }


}
