using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Processor.States;
using Microsoft.Extensions.Logging;
using SF.DotNetCore.CmqSDK.Cmq;
using System;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cmq
{
	internal sealed class CmqPublishMessageSender : BasePublishMessageSender
	{
		private readonly ILogger _logger;

		public CmqPublishMessageSender(ILogger<CmqPublishMessageSender> logger, CapOptions options,
			IStorageConnection connection, IStateChanger stateChanger)
			: base(logger, options, connection, stateChanger)
		{
			_logger = logger;
		}

		public override Task<OperateResult> PublishAsync(string keyName, string content)
		{
			var account = new CmqAccount("", "", "");
			try
			{
				account.createTopic(keyName, 1);
				_logger.LogDebug($"CMQ topic message [{keyName}] has been published.");

				return Task.FromResult(OperateResult.Success);
			}
			catch (Exception ex)
			{
				var wapperEx = new PublisherSentFailedException(ex.Message, ex);
				var errors = new OperateError
				{
					Code = ex.HResult.ToString(),
					Description = ex.Message
				};

				return Task.FromResult(OperateResult.Failed(wapperEx, errors));
			}
		}
	}
}
