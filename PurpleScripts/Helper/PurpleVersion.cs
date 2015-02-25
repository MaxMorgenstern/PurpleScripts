using System;
using System.Reflection;

[assembly:AssemblyVersion ("1.0.*.*")]
public class PurpleVersion
{
	// Version intormation
	private static int 	_Major;		// Major - Big Versions
	private static int 	_Minor;		// Minor - Functions added
	private static int 	_Build;		// Buildnumer - Days of development
	private static int 	_Revision;	// Revision numbers

	public PurpleVersion ()
	{
		_Major = 0;
		_Minor = 0;
		_Build = 0;
		_Revision = 1;
	}

	public PurpleVersion (int major, int minor, int status, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Build = status;
		_Revision = revision;
	}


	// VERSION /////////////////////////
	public string Version
	{
		get
		{
			return _Major.ToString() +"."+ _Minor.ToString() +"."+ _Build.ToString() +"."+ _Revision.ToString();
		}
	}

	public void SetVersion(int major, int minor, int status, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Build = status;
		_Revision = revision;
	}

	public string GetCurrent()
	{
		System.Version sv = Assembly.GetExecutingAssembly ().GetName ().Version;
		int firstDevelopmentDay = 5500;

		// Major, Minor, Days of Development, Build minute of current day
		PurpleVersion pv = new PurpleVersion (0, 3, (sv.Build-firstDevelopmentDay), (sv.Revision*2/60));
		return pv.Version;
	}
}
