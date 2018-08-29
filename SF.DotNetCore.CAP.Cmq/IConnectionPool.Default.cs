using DotNetCore.CAP;
using DotNetCore.CAP.Cmq;
using Microsoft.Extensions.Logging;
using SF.DotNetCore.CmqSDK.Cmq;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace SF.DotNetCore.CAP.Cmq
{
	public class ConnectionPool : IConnectionPool, IDisposable
	{
		private readonly ILogger<ConnectionPool> _logger;
		private readonly Func<CmqAccount> _activator;
		private readonly ConcurrentQueue<CmqAccount> _pool;
		private int _count;
		private int _maxSize;

		public ConnectionPool(ILogger<ConnectionPool> logger, CmqOptions options)
		{
			_logger = logger;
			_pool = new ConcurrentQueue<CmqAccount>();
			_activator = CreateActivator(options);
		}

		public string ServersAddress => throw new NotImplementedException();

		public void Dispose()
		{
			_maxSize = 0;

			while (_pool.TryDequeue(out var context))
			{
				context = null;
			}
		}

		public CmqAccount Rent()
		{
			if (_pool.TryDequeue(out var connection))
			{
				Interlocked.Decrement(ref _count);

				Debug.Assert(_count >= 0);

				return connection;
			}

			connection = _activator();

			return connection;
		}

		public bool Return(CmqAccount context)
		{
			if (Interlocked.Increment(ref _count) <= _maxSize)
			{
				_pool.Enqueue(context);

				return true;
			}

			Interlocked.Decrement(ref _count);

			Debug.Assert(_maxSize == 0 || _pool.Count <= _maxSize);

			return false;
		}

		private static Func<CmqAccount> CreateActivator(CmqOptions options)
		{
			return () => new CmqAccount(options.Endpoint, options.SecredId, options.SecredKey);
		}
	}
}
