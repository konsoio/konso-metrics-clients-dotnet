using Konso.Clients.Metrics.Clients.Extensions;
using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Dtos;
using Konso.Clients.Metrics.Models.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Konso.Clients.Metrics
{
    public class MetricsServiceClient : IMetricsServiceClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly MetricServiceConfig _metricsConfig;

        public MetricsServiceClient(IOptions<MetricServiceConfig> config, IHttpClientFactory clientFactory)
        {
            _metricsConfig = config.Value;
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateAsync(CreateMetricsRequest request)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                if (string.IsNullOrEmpty(_metricsConfig.Endpoint)) throw new Exception("Endpoint is not defined");
                if (string.IsNullOrEmpty(_metricsConfig.BucketId)) throw new Exception("Bucket is not defined");
                if (string.IsNullOrEmpty(_metricsConfig.ApiKey)) throw new Exception("API key is not defined");


                if (_metricsConfig.IgnorePath != null)
                {
                    foreach (var ignoredPath in _metricsConfig.IgnorePath)
                    {
                        // ignore path 
                        if(CultureInfo.InvariantCulture.CompareInfo.IndexOf(request.Name, ignoredPath, CompareOptions.IgnoreCase) >= 0)
                            return true;
                    }
                }

                request.AppName = _metricsConfig.AppName;
                
                request.TimeStamp = DateTime.UtcNow.ToEpoch();

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                
                request.AppVersion = !string.IsNullOrEmpty(fvi.FileVersion) ? fvi.FileVersion : "0.0.0";
                request.Runtime = 1; // dotnet
                request.RuntimeVersion = Environment.Version.ToString();
                // serialize request as json
                var jsonStr = JsonSerializer.Serialize(request);

                var httpItem = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                if (!client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", _metricsConfig.ApiKey)) throw new Exception("Missing API key");

                var response = await client.PostAsync($"{_metricsConfig.Endpoint}/v1/metrics/{_metricsConfig.BucketId}", httpItem);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }


        public async Task<PagedResponse<MetricsDto>> GetByAsync(MetricsGetRequest request)
        {
            try
            {
                var client = new HttpClient();

                if (string.IsNullOrEmpty(_metricsConfig.Endpoint)) throw new Exception("Endpoint is not defined");
                if (string.IsNullOrEmpty(_metricsConfig.BucketId)) throw new Exception("Bucket is not defined");
                if (string.IsNullOrEmpty(_metricsConfig.ApiKey)) throw new Exception("API key is not defined");
                if (!client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", _metricsConfig.ApiKey)) throw new Exception("Missing API key");

                int sortNum = (int)request.Sort;
                var builder = new UriBuilder($"{_metricsConfig.Endpoint}/v1/metrics/{_metricsConfig.BucketId}")
                {
                    Port = -1
                };
                var query = HttpUtility.ParseQueryString(builder.Query);
                if (request.ResponseCode.HasValue)
                    query["responseCode"] = request.ResponseCode.Value.ToString();


                if (!string.IsNullOrEmpty(request.Name))
                    query["name"] = request.Name;

                if (request.DateFrom.HasValue)
                    query["fromDate"] = request.DateFrom.ToString();

                if (request.DateTo.HasValue)
                    query["toDate"] = request.DateTo.ToString();
                if (sortNum > 0)
                    query["sort"] = sortNum.ToString();
                query["from"] = request.From.ToString();
                query["to"] = request.To.ToString();
                
                query["correlationId"] = request.CorrelationId;
                if (request.Tags != null && request.Tags.Count > 0)
                {
                    query["tags"] = String.Join(",", request.Tags);
                }

                builder.Query = query.ToString();
                string url = builder.ToString();

                string responseBody = await client.GetStringAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var responseObj = JsonSerializer.Deserialize<PagedResponse<MetricsDto>>(responseBody, options);
                return responseObj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
