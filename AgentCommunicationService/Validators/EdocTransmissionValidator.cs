using DDS.DCP.Models;

namespace AgentCommunicationService.Validators
{
    public class EdocTransmissionValidator : ITransmissionValidator
    {
        public virtual bool ValidateTransaction(ItemMetadata item)
        {
            // TODO: Determine if data business purpose type is supported
            if (item.TransmissionType == TransmissionTypeEnum.eDoc)
            {
                // Determine if database configuration does not support eDoc for this agency
                // Determine if the associated LOB transaction type is active
                // Determine if database configuration supports eDoc for this agency

                // TODO: Determine whether content file type supported
                // TODO: Determine if eDoc request is valid according to DCP database configuration
                // TODO: Determine if catch output sources contain policy number
            }
            else
            {
                //Determine transction supported by GetNotificationEvents database coinfiguration
            }

            return true;
        }
    }


}
