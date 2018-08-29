using DotNetCore.CAP.Abstractions;

namespace DotNetCore.CAP
{
	/// <summary>
	/// An attribute for subscribe CMQ messages.
	/// </summary>
	public class CapSubscribeAttribute : TopicAttribute
	{
		public CapSubscribeAttribute(string name) : base(name)
		{
		}
	}
}
