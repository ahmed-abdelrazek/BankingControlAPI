namespace BankingControlAPI.Domain.Requests.Interfaces
{
    public interface IOrderParams<T> where T : Enum
    {
        T OrderProperty { get; init; }
        bool OrderIsDesc { get; init; }
    }
}
