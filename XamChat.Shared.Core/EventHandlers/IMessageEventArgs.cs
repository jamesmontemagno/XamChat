using System;
using System.Collections.Generic;
using System.Text;

namespace XamChat.Core.EventHandlers
{
	public interface IMessageEventArgs
	{
		string Message { get; }
	}
}
