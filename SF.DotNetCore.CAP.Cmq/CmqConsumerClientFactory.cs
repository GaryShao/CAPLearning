namespace DotNetCore.CAP.Cmq
{
	internal sealed class CmqConsumerClientFactory : IConsumerClientFactory
	{
		private readonly IConnectionPool _connectionPool;
		private readonly CmqOptions _cmqOptions;


		public CmqConsumerClientFactory(CmqOptions cmqOptions, IConnectionPool connectionPool)
		{
			_cmqOptions = cmqOptions;
			_connectionPool = connectionPool;
		}

		public IConsumerClient Create(string groupId)
		{
			return new CmqConsumerClient(groupId, _connectionPool, _cmqOptions);
		}
	}
}
