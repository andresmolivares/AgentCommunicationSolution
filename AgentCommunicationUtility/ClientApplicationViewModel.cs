using DDS.DCP.Extensions;
using DDS.DCP.Models;
using DDS.DCP.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationUtility
{
    public class ClientApplicationViewModel : IClientApplicationViewModel
    {
        // TODO: Add to config
        const string baseUri = @"http://localhost:6900";

        public async ValueTask<string> LoadAgencyDatabaseAsync(CancellationToken token)
        {
            var agencyPath = ""; // TODO: Get config path for xml agency database
            var uri = $"{baseUri}/agencydownload";
            var hc = GetServiceConnection();
            var results = await hc.PostDataFromHttpClient(uri, null, token);
            // Persist agency database
            PersistDataToDisk(agencyPath, results.ToStream());

            return await Task.FromResult(results);
        }

        public async ValueTask<bool> NotifyAdminAsync(string message, CancellationToken token)
        {
            var uri = $"{baseUri}/notifyadmin";
            var hc = GetServiceConnection();
            var results = await hc.PostDataFromHttpClient(uri, await message.ToHttpContentAsync(), token);
            var wasAdminNotified = !string.IsNullOrWhiteSpace(results);
            return await Task.FromResult(wasAdminNotified);
        }
        
        public async ValueTask<IEnumerable<string>> GetDataSourceAsync(CancellationToken token)
        {
            // TODO: Get Source data - each data source should be associated to group of catch output policies
            return await Task.FromResult<IEnumerable<string>>(null);
        }

        public async ValueTask<TransmissionFileResponse> ProcessMetadataAsync(ItemMetadata data, CancellationToken token)
        {
            var uri = $"{baseUri}/processmetadata";
            var hc = GetServiceConnection();
            var request = new MetadataProcessRequest
            {
                Item = data
            };
            var response = await hc.PutDataFromHttpClient<TransmissionFileResponse>(uri, await request.ToHttpContentAsync(), token);
            Console.WriteLine($"Process Metadata response is: {response}");

            return response;
        }

        public async ValueTask<bool> TransmitFilesAsync(TransmissionFileResponse response, CancellationToken token)
        {
            var wereFilesTransmitted = true;

            try
            {
                // Persist content file
                PersistDataToDisk($"{response.DestinationFileLocation}.xml", response.AcordPayload.ToStream());

                // Persist transfer file
                PersistDataToDisk($"{response.DestinationFileLocation}.xfr", response.XfrTransferPayload.ToStream());

            }
            catch (Exception)
            {
                wereFilesTransmitted = false;
            }

            return await Task.FromResult(wereFilesTransmitted);
        }

        private void PersistDataToDisk(string targetPath, Stream streamData)
        {
            // Truncate data for existing file
            var fileMode = File.Exists(targetPath) ? FileMode.Truncate : FileMode.OpenOrCreate;
            // Writing out final stream
            using FileStream fs = new(targetPath, fileMode);
            streamData.Position = 0;
            streamData.CopyTo(fs);
            fs.Flush();
        }

        private HttpClient GetServiceConnection()
        {
            // Initialize client connection
            return new HttpClient();
        }
    }
}
