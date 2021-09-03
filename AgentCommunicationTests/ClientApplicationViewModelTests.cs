using AgentCommunicationUtility;
using DDS.DCP.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationTests
{
    public class ClientApplicationViewModelTests
    {
        private IClientApplicationViewModel vm;
        private readonly CancellationToken token = CancellationToken.None;

        [SetUp]
        public void Setup()
        {
            vm = new ClientApplicationViewModel();
        }

        [Test]
        public async Task NotifyAdminAsyncTest()
        {
            await vm.NotifyAdminAsync("Testing.", token);
        }

        [Test]
        public async Task GetAgencyDatabaseAsyncTest()
        {
            await vm.LoadAgencyDatabaseAsync(token);
        }

        [Test]
        public async Task GetDataSourceAsyncTest()
        {
            await vm.GetDataSourceAsync(token);
        }
        private async ValueTask<ItemMetadata> GetMockItemMetadata()
        {
            var item = new ItemMetadata("path", new Random().Next(1, 100), TransmissionTypeEnum.eDoc);
            return await Task.FromResult(item);
        }

        [Test]
        public async Task ProcessMetadataAsyncTest()
        {
            var item = await GetMockItemMetadata();
            var response = await vm.ProcessMetadataAsync(item, token);
            Assert.IsNotEmpty(response.AcordPayload);
            Assert.IsNotEmpty(response.XfrTransferPayload);
            Assert.IsNotEmpty(response.DestinationFileLocation);

            await Task.Yield();
        }

        [Test]
        public async Task PostTransmissionFilesAsyncTest()
        {
            var item = await GetMockItemMetadata();
            var response = await vm.ProcessMetadataAsync(item, token);
            await vm.TransmitFilesAsync(response, token);
            FileAssert.Exists(Path.Combine(response.DestinationFileLocation, response.AcordPayload));
            // TODO: Verify files were transmitted
        }

    }
}