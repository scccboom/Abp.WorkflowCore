using System.ComponentModel;
using System.Reflection;

using Abp.Dependency;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using WorkflowDemo.Workflows;

namespace WorkflowDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowInstaller : IWindsorInstaller
    {
        private IWindsorContainer _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _container = container;
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
             * gets all event handler interfaces and registers type to Event Bus for each handling event.
             */
            if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetTypeInfo().GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }
                _container?.Resolve<IAbpWorkflowRegistry>()?.RegisterWorkflow(handler.ComponentModel.Implementation);
            }
        }
    }
}