using System;

public class PurpleException : Exception
{
	public PurpleException ()
	{
	}

	public PurpleException(string message) : base(message)
	{
	}
	
	public PurpleException(string message, Exception inner) : base(message, inner)
	{
	}
}
