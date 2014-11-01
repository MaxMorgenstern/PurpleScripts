using System;

public class PurpleVersion
{
	// Version intormation
	private static int 	_Major;
	private static int 	_Minor;
	private static int 	_Status;
	private static int 	_Revision;

	public PurpleVersion ()
	{
		_Major = 0;		// Major Builds
		_Minor = 0;		// Minor Builds - Functions added
		_Status = 0;	// 0 for alpha - 1 for beta - 2 for release candidate - 3 for (final) release
		_Revision = 1;	// Bugs fixed - changes made to code
	}

	// VERSION /////////////////////////
	public static string Version 
	{
		get
		{
			return _Major.ToString() +"."+ _Minor.ToString() +"."+ _Status.ToString() +"."+ _Revision.ToString();
		}
	}

	public void SetVersion(int major, int minor, int status, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Status = status;
		_Revision = revision;
	}
}
