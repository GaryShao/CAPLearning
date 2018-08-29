using SF.DotNetCore.CmqSDK.Cmq;

namespace DotNetCore.CAP.Cmq
{
	public interface IConnectionPool
	{
		string ServersAddress { get; }

		CmqAccount Rent();

		bool Return(CmqAccount context);
	}
}
