namespace DotNetCore.CAP
{
	public class CmqOptions
	{
		/// <summary>
		/// The identifier of your app's secret key registered in tencent cloud
		/// </summary>
		public string SecredId;
		/// <summary>
		/// Your app's secret key registered in tencent cloud
		/// </summary>
		public string SecredKey;
		/// <summary>
		/// The full domain url of the tencent cmq api
		/// </summary>
		public string Endpoint = "http://cmq-queue-gz.api.qcloud.com";

		public string HostName { get; set; } = "localhost";
	}
}
