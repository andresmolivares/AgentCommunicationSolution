using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DDS.DCP.Extensions
{
    public static class HttpClientExtension
    {
        public static async ValueTask<TResult> GetDataFromHttpClient<TResult>(this HttpClient hc, string uri, CancellationToken token)
            where TResult : class, new()
        {
            // Process request and return content result
            if (hc == null)
                throw new Exception();
            var response = await hc.GetAsync(uri, token);
            return await Task.FromResult(JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStringAsync(token)));
        }
        public static async ValueTask<string> GetDataFromHttpClient(this HttpClient hc, string uri, CancellationToken token)
        {
            // Process request and return content result
            if (hc == null)
                throw new Exception();
            var response = await hc.GetAsync(uri, token);
            return await response.Content.ReadAsStringAsync(token);
        }

        public static async ValueTask<TResult> PostDataFromHttpClient<TResult>(this HttpClient hc, string uri, HttpContent payload, CancellationToken token)
            where TResult : class, new()
        {
            // Process request and return content result
            if (hc == null)
                throw new Exception();
            if (payload == null)
                Console.WriteLine("HttpContent for POST payload was null.");
            var response = await hc.PostAsync(uri, payload, token);
            return await Task.FromResult(JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStringAsync(token)));
        }

        public static async ValueTask<string> PostDataFromHttpClient(this HttpClient hc, string uri, HttpContent payload, CancellationToken token)
        {
            // Process request and return content result
            if (hc == null)
                throw new Exception();
            if (payload == null)
                Console.WriteLine("HttpContent for POST payload was null.");
            var response = await hc.PostAsync(uri, payload, token);
            return await Task.FromResult(await response.Content.ReadAsStringAsync(token));
        }

        public static async ValueTask<TResult> PutDataFromHttpClient<TResult>(this HttpClient hc, string uri, HttpContent payload, CancellationToken token)
            where TResult : class, new()
        {
            if (hc == null)
                throw new Exception();
            // Process request and return content result
            if (payload == null)
                Console.WriteLine("HttpContent for PUT payload was null.");
            var response = await hc.PutAsync(uri, payload, token);
            return await Task.FromResult(JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStringAsync(token)));
        }

        public static async ValueTask<string> PutDataFromHttpClientAsync(this HttpClient hc, string uri, HttpContent payload, CancellationToken token)
        {
            if (hc == null)
                throw new Exception();
            // Process request and return content result
            if (payload == null)
                Console.WriteLine("HttpContent for PUT payload was null.");
            var response = await hc.PutAsync(uri, payload, token);
            return await Task.FromResult(await response.Content.ReadAsStringAsync(token));
        }
    }
}
