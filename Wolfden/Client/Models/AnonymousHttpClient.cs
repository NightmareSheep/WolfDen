using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wolfden.Client.Models
{
    public class AnonymousHttpClient
    {
        public readonly HttpClient httpClient;

        public AnonymousHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public string GetBaseUrl()
        {
            return httpClient.BaseAddress.ToString();
        }
    }
}
