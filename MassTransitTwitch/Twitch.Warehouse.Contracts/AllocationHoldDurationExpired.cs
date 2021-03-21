using System;

namespace Twitch.Warehouse.Contracts
{
    public interface AllocationHoldDurationExpired
    {
        Guid AllocationId { get; }
    }
}