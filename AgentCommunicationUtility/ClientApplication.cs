#nullable enable

using DDS.DCP.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationUtility
{
    public class ClientApplication
    {
        private readonly IClientApplicationViewModel _vm;

        public ClientApplication(IClientApplicationViewModel vm)
        {
            _vm = vm;
        }

        public async ValueTask<int> Run(string[] args)
        {
            var exitCode = 1;
            var noneToken = CancellationToken.None;
            try
            {
                // TODO: Add argument validation 
                if (args == null || args.Length < 3)
                    throw new ArgumentException($"Application arguments were empty or invalid: args => {args}");
                var activityNoteType = args[0];
                var lob = args[1];
                var commandKey = args[2];

                switch (commandKey)
                {
                    case "agencyDownload":
                        await _vm.NotifyAdminAsync("Starting workflow...", noneToken);
                        await _vm.LoadAgencyDatabaseAsync(noneToken);
                        break;
                    case "generateFiles":
                        // TODO: Get database settings
                        // TODO: Get source path for specified lob
                        var source = await _vm.GetDataSourceAsync(noneToken);
                        // NOTE: Previous code processes for multiple sources
                        await ProcessSourceAsync(source, noneToken);
                        await _vm.NotifyAdminAsync("Completing workflow...", noneToken);
                        break;
                }
            }
            catch (Exception e)
            {
                exitCode = 0;
                Console.WriteLine(@$"Error occurred: {e.Message}
Stack Trace: {e.StackTrace}");
            }

            return await Task.FromResult(exitCode);
        }

        private async Task ProcessSourceAsync(IEnumerable<string> source, CancellationToken token)
        {
            // TODO: Get the correct date time range of files to process
            // EXAMPLE: Current Day, Friday on Monday or multiple days
            // TODO: Build reference list of files to process for each date in range and persist listings
            
            var referenceList = new List<ItemMetadata>();
            int sequenceId = 0;
            foreach (var item in source)
            {
                var metadata = new EdocMetadata(item, sequenceId++) 
                { 
                    ProcessStatusType = ProcessStatusTypeEnum.Captured 
                };
                metadata.ParseMetadata();
                referenceList.Add(metadata);
            }
            // TODO: Persist reference list
            // NOTE: The reference list should act as a checklist of items that
            // have been processed. This list should be serializable to continue processing in
            // cases where processing was interupted.

            if (source == null)
                throw new Exception("No source files found.");
            // Iterate through reference list and process each item
            foreach (var data in referenceList)
            {
                try
                {
                    var response = await _vm.ProcessMetadataAsync(data, token);
                    // NOTE: We can leave the responsibility of transmitting the destination files on the server. 
                    // That implementation could change in the future if we have more integration with IVANS API.
                    await _vm.TransmitFilesAsync(response, token);
                    data.ProcessStatusType = ProcessStatusTypeEnum.Processsed;
                    //NOTE: Update data item process state
                }
                catch (Exception)
                {
                    data.ProcessStatusType = ProcessStatusTypeEnum.Errored;
                }
            }
        }
    }
}
