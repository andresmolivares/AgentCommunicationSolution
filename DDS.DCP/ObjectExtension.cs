using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DDS.DCP.Extensions
{
    public static class ObjectExtension
    {
        public static async Task<HttpContent> ToHttpContentAsync(this object request)
        {
            if (request == null)
                throw new ArgumentNullException("Cannot convert null to HtttpContent.");
            var payload = JsonConvert.SerializeObject(request, Formatting.Indented);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            return await Task.FromResult(httpContent);
        }

        public static MemoryStream ToStream(this string owner)
        {
            // Initialize stram from string byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(owner);
            return new MemoryStream(byteArray);
        }
    }
}
