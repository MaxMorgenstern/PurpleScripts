using System.Collections.Generic;
using PurpleStorage;

// TODO	 - Test server from list

namespace PurpleNetwork.Server
{
	public class ReferenceList
	{
		private List <ServerReference> serverList;
		public List <ServerReference> ServerReferenceList
		{
			get { return serverList; }
			set { serverList = value; }
		}

		public void Reset()
		{
			serverList = new List<ServerReference> ();
		}

		public bool Add(ServerReference reference)
		{
			serverList.Add (reference);
			return true;
		}

		public bool Remove(ServerReference reference)
		{
			return serverList.Remove (reference);
		}

		public bool Load()
		{
			return Load ("PNServerReferenceList");
		}

		public bool Load(string filename)
		{
			serverList = PurpleStorage.PurpleStorage.LoadBinaryFile<List <ServerReference>> (filename);
			return true;
		}

		public bool Save()
		{
			return Save ("PNServerReferenceList");
		}

		public bool Save(string filename)
		{
			return PurpleStorage.PurpleStorage.SaveBinaryFile (filename, serverList);
		}


		// CONSTRUCTOR /////////////////////////
		public ReferenceList()
		{
			serverList = new List<ServerReference> ();
		}

		public ReferenceList(bool load)
		{
			if(load)
			{
				Load ();
			}
			else 
			{
				serverList = new List<ServerReference> ();
			}
		}
	}
}
