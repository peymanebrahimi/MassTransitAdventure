using System;

namespace Twitch.Warehouse.Contracts
{
    public interface InventoryAllocated
    {
        Guid AllocationId { get; }

        string ItemNumber { get; }
        decimal Quantity { get; }
    }
}