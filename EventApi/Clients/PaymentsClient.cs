using EventApi.Dtos.Dashboard;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EventApi.Clients
{
    public class PaymentsClient : IPaymentsClient
    {
        private readonly HttpClient _http;
        public PaymentsClient(HttpClient http) => _http = http;

        public async Task<decimal> GetTotalAsync(DateTime? from = null, DateTime? to = null)
        {
            var url = "api/transactions/total";
            if (from.HasValue || to.HasValue)
            {
                var qs = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (from.HasValue) qs["from"] = from.Value.ToString("O");
                if (to.HasValue) qs["to"] = to.Value.ToString("O");
                url += "?" + qs.ToString();
            }

            var result = await _http.GetFromJsonAsync<decimal?>(url);
            return result ?? 0m;
        }
        public async Task<List<RevenuePointDto>> GetRangeAsync(DateTime? from = null, DateTime? to = null)
        {
            var url = "api/transactions/range";
            var qs = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (from.HasValue) qs["from"] = from.Value.ToString("O");
            if (to.HasValue) qs["to"] = to.Value.ToString("O");
            if (qs.Count > 0) url += "?" + qs.ToString();
            return await _http.GetFromJsonAsync<List<RevenuePointDto>>(url) ?? new();
        }
    }
}
