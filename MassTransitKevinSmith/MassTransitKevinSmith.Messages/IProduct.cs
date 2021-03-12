namespace MassTransitKevinSmith.Messages
{
    public interface IProduct
    {
        string Name { get; }

        decimal Price { get; }
        int Number { get; }
    }
}