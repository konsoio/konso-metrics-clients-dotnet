using System.Net.Http;

namespace Konso.Metrics.Client.Tests
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new HttpClient();
    }
}
