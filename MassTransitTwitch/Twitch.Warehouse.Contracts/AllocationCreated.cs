using System;

namespace Twitch.Warehouse.Contracts
{
    public interface AllocationCreated
    {
        Guid AllocationId { get; }
        TimeSpan HoldDuration { get; }
    }
}