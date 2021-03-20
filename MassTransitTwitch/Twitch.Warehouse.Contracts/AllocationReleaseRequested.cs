using System;

namespace Twitch.Warehouse.Contracts
{
    public interface AllocationReleaseRequested
    {
        Guid AllocationId { get; }

        string Reason { get; }
    }
}