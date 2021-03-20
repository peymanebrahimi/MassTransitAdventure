using MassTransit.Definition;

namespace Twitch.Sample.Components.CourierActivities
{
    public class AllocateInventoryActivityDefinition :
        ActivityDefinition<AllocateInventoryActivity, AllocateInventoryArguments, AllocateInventoryLog>
    {
        public AllocateInventoryActivityDefinition()
        {
            ConcurrentMessageLimit = 10;
        }
    }
}