using Konso.Clients.Metrics.Clients.Extensions;
using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Dtos;
using Konso.Clients.Metrics.Models.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
                    if (HasStringToIgnore(request.Name, _metricsConfig.IgnorePath))
                    {
                        return true;
                    }
                }

                request.AppName = _metricsConfig.AppName;
                
                request.TimeStamp = DateTime.UtcNow.ToEpoch();
                
                request.AppVersion = VersionInfoHelper.AppVersion(); ;
                request.Runtime = 1; // dotnet
                request.RuntimeVersion = Environment.Version.ToString();
                // serialize request as json
                var jsonStr = JsonSerializer.Serialize(request);

                var httpItem = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                if (!client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", _metricsConfig.ApiKey)) throw new Exception("Missing API key");

                var response = await client.PostAsync($"{_metricsConfig.Endpoint}/metrics/{_metricsConfig.BucketId}", httpItem);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public bool HasStringToIgnore(string input, List<string> toIgnore)
        {
            var result = false;
            foreach (var ignoredPath in toIgnore)
            {
                // ignore path 
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(input, ignoredPath, CompareOptions.IgnoreCase) >= 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
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
                var builder = new UriBuilder($"{_metricsConfig.Endpoint}/metrics/{_metricsConfig.BucketId}")
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
