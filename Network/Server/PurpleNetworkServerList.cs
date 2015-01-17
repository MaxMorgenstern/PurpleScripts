using System;
using System.Collections.Generic;
using PurpleStorage;

// TODO

namespace PurpleNetwork
{
	namespace Server
	{
		public class ReferenceList
		{
			private List <ServerReference> serverList;

			public bool Add(ServerReference reference)
			{
				serverList.Add (reference);
				return true;
			}

			public bool Test(ServerReference reference)
			{
				// check server availability
				return true;
			}

			public bool Test()
			{
				// check all server availabilities
				return true;
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
				// TODO: Test - Load from disk
				serverList = PurpleStorage.PurpleStorage.Load<List <ServerReference>> ("dummy_filename");
				return true;
			}

			public bool Save()
			{
				// TODO: Test - Save from disk...
				return PurpleStorage.PurpleStorage.SaveBinaryFile ("dummy_filename", serverList);
			}

			// CONSTRUCTOR
			public ReferenceList()
			{
				serverList = new List<ServerReference> ();
			}

		}
	}
}
