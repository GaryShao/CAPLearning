using System;

namespace SF.DotNetCore.CmqSDK.Exception
{
	public class ClientException : ApplicationException
    {
        public ClientException(string message) : base(message) { }

        public override string Message
        {
            get { return base.Message; }
           
        }
    }
}
