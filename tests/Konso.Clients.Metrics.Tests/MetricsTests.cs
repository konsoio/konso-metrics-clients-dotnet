using FluentAssertions;
using Konso.Clients.Metrics.Clients.Extensions;
using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Requests;
using Konso.Clients.Metrics.Models.Requests.Enums;
using Konso.Metrics.Client.Tests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Konso.Clients.Metrics.Tests
{
    public class MetricsTests 
    {
        private const string apiUrl = "https://devapis.konso.io/v1/metrics";
        private const string bucketId = "cab6ff8d";
        private const string apiKey = "IL84eTnxvn5mtlZNH3sSnMqE8V6E5hNm3Synx9E+XeU=";
        private const string app = "TestApp";


        [Fact]
        public async Task Create_SimpleMetric()
        {
            // arrange
            var service = new MetricsService(new MetricServiceConfig() {
                ApiKey= apiKey,
                AppName = app,
                BucketId= bucketId,
                Endpoint= apiUrl
            }, new DefaultHttpClientFactory());

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
            var service = new MetricsService(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }, new DefaultHttpClientFactory());

            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/test", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200 };
            var res = await service.CreateAsync(o);

            // act
            var pagedList = await service.GetByAsync(new MetricsGetRequest() { BucketId = bucketId, From = 0, To = 10 });

            // assert
            pagedList.Should().NotBeNull();
            pagedList.Total.Should().BeGreaterThan(10);

        }

        [Fact]
        public async Task Create_SimpleWithTag()
        {
            var service = new MetricsService(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }, new DefaultHttpClientFactory());


            var o = new CreateMetricsRequest() { AppName = "test", Duration = 15, Name = "GET v1/test", TimeStamp = DateTime.UtcNow.ToEpoch(), ResponseCode = 200, Tags = new List<string>() { "test" } };

            var response = await service.CreateAsync(o);

            // 
            response.Should().BeTrue();
        }


        [Fact]
        public async Task CreateAndGet_SimpleWithTag()
        {
            // arrange
            var service = new MetricsService(new MetricServiceConfig()
            {
                ApiKey = apiKey,
                AppName = app,
                BucketId = bucketId,
                Endpoint = apiUrl
            }, new DefaultHttpClientFactory());

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
