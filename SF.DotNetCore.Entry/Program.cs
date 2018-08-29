using SF.DotNetCore.CmqSDK.Cmq;
using SF.DotNetCore.CmqSDK.Exception;
using System;
using System.Collections.Generic;

namespace SF.DotNetCore.Entry
{
	class Program
	{
		static void Main(string[] args)
		{
			string secretId = "";
			string secretKey = "";
			string endpoint = "http://cmq-topic-gz.api.qcloud.com";
			try
			{
				var account = new CmqAccount(endpoint, secretId, secretKey);
				var topicName = "dotNet-test";
				var subscriptionName = "subsc-test";
				account.createTopic(topicName, 65536);

				var topicList = new List<string>();
				int totalCount = account.listTopic(topicName, 0, 0, topicList);

				//get topic Attributes
				var topic = account.getTopic(topicName);
				var meta = new TopicMeta();
				meta = topic.getTopicAttributes();				
				topic.setTopicAttributes(32768);

				string subscEndpoint = "http://test.hahaha.com";
				string protocol = "http";

				account.createSubscribe(topicName, subscriptionName, subscEndpoint, protocol);
				account.createSubscribe(topicName, subscriptionName, endpoint, protocol, new List<string> { "test1" }, null, "BACKOFF_RETRY", "JSON");
				var subsc = account.getSubscribe(topicName, subscriptionName);

				var subscriptionList = new List<string>();
				topic.ListSubscription(0, 0, "", subscriptionList);

				string msg = "this is a test message";
				topic.publishMessage(msg, new List<string> { "test1", "test2" }, " ");
				
				var vMsg = new List<string>();
				int msgCount = 5;
				for (int i = 0; i < msgCount; ++i)
					vMsg.Add(msg);
				topic.batchPublishMessage(vMsg);
				
				account.deleteSubscribe(topicName, subscriptionName);
				account.deleteTopic(topicName);
				Console.Read();
			}
			catch (ClientException e)
			{
				Console.Write(e.Message);
			}
			catch (ServerException e)
			{
				Console.Write(e.ToString());
			}
			catch (Exception e)
			{
				Console.Write(e.Message);
			}

			return;
		}
	}
}
