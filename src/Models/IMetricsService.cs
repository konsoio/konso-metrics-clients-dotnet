using Konso.Clients.Metrics.Models.Dtos;
using Konso.Clients.Metrics.Models.Requests;
using System.Threading.Tasks;

namespace Konso.Clients.Metrics.Models
{
    public interface IMetricsService
    {
        Task<bool> CreateAsync(CreateMetricsRequest request);

        Task<PagedResponse<MetricsDto>> GetByAsync(MetricsGetRequest request);
    }
}
