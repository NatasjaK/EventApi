using EventApi.Dtos.Dashboard;

namespace EventApi.Clients

{
    public interface IPaymentsClient
    {
        Task<decimal> GetTotalAsync(DateTime? from = null, DateTime? to = null);
        Task<List<RevenuePointDto>> GetRangeAsync(DateTime? from = null, DateTime? to = null);
    }
}
