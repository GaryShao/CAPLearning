using SF.DotNetCore.CmqSDK.Cmq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotNetCore.CAP.Cmq
{
	internal sealed class CmqConsumerClient : IConsumerClient
	{
		private readonly IConnectionPool _connectionPool;
		private readonly CmqOptions _cmqOption;
		private readonly CmqAccount _cmqAccount;

		public CmqConsumerClient(IConnectionPool connectionPool,
			CmqOptions options)
		{
			_connectionPool = connectionPool;
			_cmqOption = options;
			InitClient();
		}

		public event EventHandler<MessageContext> OnMessageReceived;

		public event EventHandler<LogMessageEventArgs> OnLog;

		public string ServersAddress => _cmqOption.HostName;

		public void Subscribe(IEnumerable<string> topics)
		{
			if (topics == null)
			{
				throw new ArgumentNullException(nameof(topics));
			}

			foreach (var topic in topics)
			{
				_connectionPool.Rent().createSubscribe(topic, "", "", "http");
			}
		}

		public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
		{
			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();
				cancellationToken.WaitHandle.WaitOne(timeout);
			}

			// ReSharper disable once FunctionNeverReturns
		}

		public void Commit()
		{
			_cmqA
			_channel.BasicAck(_deliveryTag, false);
		}

		public void Reject()
		{
			_channel.BasicReject(_deliveryTag, true);
		}

		public void Dispose()
		{
			_channel.Dispose();
			_connection.Dispose();
		}

		private void InitCmqAccount()
		{
			_cmqAccount.createQueue("queueName", )

			_connection = _connectionChannelPool.GetConnection();

			_channel = _connection.CreateModel();

			_channel.ExchangeDeclare(
				_exchageName,
				RabbitMQOptions.ExchangeType,
				true);

			var arguments = new Dictionary<string, object>
			{
				{"x-message-ttl", _rabbitMQOptions.QueueMessageExpires}
			};
			_channel.QueueDeclare(_queueName, true, false, false, arguments);
		}

		#region events

		private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
		{
			var args = new LogMessageEventArgs
			{
				LogType = MqLogType.ConsumerCancelled,
				Reason = e.ConsumerTag
			};
			OnLog?.Invoke(sender, args);
		}

		private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
		{
			var args = new LogMessageEventArgs
			{
				LogType = MqLogType.ConsumerUnregistered,
				Reason = e.ConsumerTag
			};
			OnLog?.Invoke(sender, args);
		}

		private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
		{
			var args = new LogMessageEventArgs
			{
				LogType = MqLogType.ConsumerRegistered,
				Reason = e.ConsumerTag
			};
			OnLog?.Invoke(sender, args);
		}

		private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
		{
			_deliveryTag = e.DeliveryTag;
			var message = new MessageContext
			{
				Group = _queueName,
				Name = e.RoutingKey,
				Content = Encoding.UTF8.GetString(e.Body)
			};
			OnMessageReceived?.Invoke(sender, message);
		}

		private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
		{
			var args = new LogMessageEventArgs
			{
				LogType = MqLogType.ConsumerShutdown,
				Reason = e.ReplyText
			};
			OnLog?.Invoke(sender, args);
		}

		#endregion
	}
}
