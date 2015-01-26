using System;
using System.Collections.Generic;
using PurpleStorage;

// TODO

namespace PurpleNetwork.Server
{
	public class ReferenceList
	{
		private List <ServerReference> serverList;

		public void Reset()
		{
			serverList = new List<ServerReference> ();
		}

		public bool Add(ServerReference reference)
		{
			serverList.Add (reference);
			return true;
		}

		public bool Test(ServerReference reference)
		{
			// TODO: check server availability
			return true;
		}

		public bool Test()
		{
			// check all server availabilities
			bool returnValue = true;
			foreach(ServerReference sr in serverList)
			{
				bool tmpReturn = Test(sr);
				if(returnValue)
				{
					returnValue = tmpReturn;
				}
			}
			return returnValue;
		}

		public bool Remove(ServerReference reference)
		{
			return serverList.Remove (reference);
		}

		public List <ServerReference> Get()
		{
			return serverList;
		}

		public bool Load()
		{
			serverList = PurpleStorage.PurpleStorage.Load<List <ServerReference>> ("dummy_filename");
			return true;
		}

		public bool Save()
		{
			return PurpleStorage.PurpleStorage.SaveBinaryFile ("dummy_filename", serverList);
		}

		// CONSTRUCTOR
		public ReferenceList()
		{
			serverList = new List<ServerReference> ();
		}

	}
}
