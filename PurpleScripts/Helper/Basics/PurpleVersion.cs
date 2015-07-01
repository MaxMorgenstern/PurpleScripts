using System.Reflection;
using System;

[assembly:AssemblyVersion ("1.0.*.*")]
public class PurpleVersion
{
	// Version intormation
	public int 	_Major;		// Major - Big Versions
	public int 	_Minor;		// Minor - Functions added
	public int 	_Build;		// Buildnumer - Days of development
	public int 	_Revision;	// Revision numbers

	public PurpleVersion ()
	{
		_Major = 0;
		_Minor = 0;
		_Build = 0;
		_Revision = 1;
	}

	public PurpleVersion (string version)
	{
		string[] versionArray = version.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
		if(versionArray.Length == 4)
		{
			_Major = Int32.Parse (versionArray [0]);
			_Minor = Int32.Parse (versionArray [1]);
			_Build = Int32.Parse (versionArray [2]);
			_Revision = Int32.Parse (versionArray [3]);
		}
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
		int major = 0;
		int minor = 3;
		int firstDevelopmentDay = 5500;
		return GetCurrent (major, minor, firstDevelopmentDay);
	}

	public string GetCurrent(int major, int minor, int offset)
	{
		System.Version sv = Assembly.GetExecutingAssembly ().GetName ().Version;

		// Major, Minor, Days of Development, Build minute of current day
		PurpleVersion pv = new PurpleVersion (major, minor, (sv.Build-offset), (sv.Revision*2/60));
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

	public string GetDatabaseVersion()
	{
		return PurpleConfig.Database.Version.Required;
	}

	public bool AreEqual(PurpleVersion compareValue)
	{
		if (_Major != compareValue._Major)
			return false;

		if (_Minor != compareValue._Minor)
			return false;

		if (_Build != compareValue._Build)
			return false;

		if (_Revision != compareValue._Revision)
			return false;

		return true;
	}
}
