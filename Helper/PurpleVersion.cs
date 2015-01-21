using System;

public class PurpleVersion
{
	// Version intormation
	private static int 	_Major;		// Major Builds
	private static int 	_Minor;		// Minor Builds - Functions added
	private static int 	_Status;	// 0 for alpha - 1 for beta - 2 for release candidate - 3 for (final) release
	private static int 	_Revision;	// Bugs fixed - changes made to code

	public PurpleVersion ()
	{
		_Major = 0;
		_Minor = 0;
		_Status = 0;
		_Revision = 1;
	}

	public PurpleVersion (int major, int minor, int status, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Status = status;
		_Revision = revision;
	}


	// VERSION /////////////////////////
	public string Version
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

	public string GetCurrent()
	{
		// Return the current PurpleScripts version
		PurpleVersion pv = new PurpleVersion (0, 3, 0, 1); // 2015-01-19
		return pv.Version;
	}
}
