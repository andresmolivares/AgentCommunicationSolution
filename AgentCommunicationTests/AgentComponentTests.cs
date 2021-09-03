
using AgentCommunicationService.ContentBuilders;
using AgentCommunicationService.Controllers;
using AgentCommunicationService.Validators;
using DDS.DCP.Models;
using DDS.DCP.Requests;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgentCommunicationTests
{
    /// <summary>
    /// Integration Tests
    /// </summary>
    class AgentComponentTests
    {
        private IAgentComponent component;

        [SetUp]
        public void Setup()
        {
            component = new AgentComponent(
                new AgentRepository(), 
                new TransmissionValidatorFactory(),
                new AcordContentBuilderFactory()
                );
        }

        [Test]
        public async Task NotifyAdminAsyncTest()
        {
            await component.NotifyAdminAsync("Testing", CancellationToken.None);
        }

        [Test]
        public async Task ProcessMetadataAsyncTest()
        {
            var request = new MetadataProcessRequest
            {
               Item = new ItemMetadata("path", new Random().Next(1, 100), TransmissionTypeEnum.eDoc)
            };
            await component.ProcessMetadataAsync(request, CancellationToken.None);
        }
    }
}