using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WorkflowCore.Interface;

namespace WorkflowDemo
{
    public class WorkflowHostedService : IHostedService
    {
        private readonly IWorkflowHost _host;

        public WorkflowHostedService(IWorkflowHost host)
        {
            _host = host;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _host.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _host.Stop();
            return Task.CompletedTask;
        }
    }
}
