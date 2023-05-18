using FluentAssertions;
using Konso.Clients.Metrics.Clients.Extensions;
using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Requests;
using Konso.Clients.Metrics.Models.Requests.Enums;
using Konso.Metrics.Client.Tests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Konso.Clients.Metrics.Tests
{
    public class MetricsTests 
    {
        private const string apiUrl = "https://apis.konso.io";
        private const string bucketId = "<your bucket id>";
        private const string apiKey = "<your app key>";
        private const string app = "TestApp";


        [Fact]
        public async Task Create_SimpleMetric()
        {
            // arrange
            var service = new MetricsServiceClient(Options.Create(new MetricServiceConfig() {
                ApiKey= apiKey,
                AppName = app,
                BucketId= bucketId,
                Endpoint= apiUrl
            }), new DefaultHttpClientFactory());

            // act
            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/test", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200 };
            var res = await service.CreateAsync(o);
                
            // assert
            res.Should().BeTrue();
                       
        }

        [Fact]
        public async Task CreateAndGet_SimpleMetric()
        {
            // arrange
            var service = new MetricsServiceClient(Options.Create(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }), new DefaultHttpClientFactory());

            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/test", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200 };
            var res = await service.CreateAsync(o);

            // act
            var pagedList = await service.GetByAsync(new MetricsGetRequest() { BucketId = bucketId, From = 0, To = 10 });

            // assert
            pagedList.Should().NotBeNull();
            pagedList.Total.Should().BeGreaterThan(0);

        }

        [Fact]
        public async Task Create_SimpleWithTag()
        {
            var service = new MetricsServiceClient(Options.Create(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }), new DefaultHttpClientFactory());


            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/test", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200, Tags = new List<string>() { "test" } };

            var response = await service.CreateAsync(o);

            // 
            response.Should().BeTrue();
        }


        [Fact]
        public async Task CreateAndGet_SimpleWithTag()
        {
            // arrange
            var service = new MetricsServiceClient(Options.Create(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }), new DefaultHttpClientFactory());

            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/tagtest", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200, Tags = new List<string>() { "test" } };

            var response = await service.CreateAsync(o);

            // act
            var res = await service.GetByAsync(new MetricsGetRequest { Tags = new List<string>() { "test" }, From = 0, To = 10, Sort = SortingTypes.TimeStampDesc });


            // assert
            foreach (var item in res.List)
            {
                item.Tags.Should().Contain("test");
            }
        }
    }
}
