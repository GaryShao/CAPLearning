using System;
using DotNetCore.CAP.Cmq;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.CAP
{
	internal sealed class CmqOptionsExtension : ICapOptionsExtension
	{
		private readonly Action<CmqOptions> _configure;

		public CmqOptionsExtension(Action<CmqOptions> configure)
		{
			_configure = configure;
		}

		public void AddServices(IServiceCollection services)
		{
			services.AddSingleton<CapMessageQueueMakerService>();

			var options = new CmqOptions();
			_configure?.Invoke(options);
			services.AddSingleton(options);

			services.AddSingleton<IConsumerClientFactory, CmqConsumerClientFactory>();
			services.AddSingleton<IConnectionChannelPool, ConnectionChannelPool>();
			services.AddSingleton<IPublishExecutor, RabbitMQPublishMessageSender>();
			services.AddSingleton<IPublishMessageSender, RabbitMQPublishMessageSender>();
		}
	}
}
