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

	public PurpleVersion (int major, int minor, int build, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Build = build;
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

	public void SetVersion(int major, int minor, int build, int revision)
	{
		_Major = major;
		_Minor = minor;
		_Build = build;
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

	public string GetServerVersion()
	{
		PurpleVersion pv = new PurpleVersion (
			PurpleConfig.Version.Server.Major, 
			PurpleConfig.Version.Server.Minor, 
			PurpleConfig.Version.Server.Build,
			PurpleConfig.Version.Server.Revision);
		return pv.Version;
	}

	public string GetClientVersion()
	{
		PurpleVersion pv = new PurpleVersion (
			PurpleConfig.Version.Client.Major, 
			PurpleConfig.Version.Client.Minor, 
			PurpleConfig.Version.Client.Build,
			PurpleConfig.Version.Client.Revision);
		return pv.Version;
	}
}
