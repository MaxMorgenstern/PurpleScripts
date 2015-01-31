using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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


		// TESTER FUNCTIONS /////////////////////////
		
		public bool Test()
		{
			// check all server availabilities
			bool returnValue = true;
			foreach(ServerReference sr in serverList)
			{
				ServerReference newSR = new ServerReference();
				bool pingReturn = Test(sr, out newSR);
				sr.ReferencePingNote = newSR.ReferencePingNote;
				sr.ServerState = newSR.ServerState;
				sr.ReferenceLastSeen = newSR.ReferenceLastSeen;
				
				if(returnValue)
					returnValue = pingReturn;
			}
			return returnValue;
		}
		
		public bool Test(ServerReference reference)
		{
			return Ping (reference.ServerHost);
		}
		
		public bool Test(ServerReference reference, out ServerReference newRefernece)
		{
			string pingMessage = String.Empty;
			newRefernece = reference;
			bool pingReturn = Ping(reference.ServerHost, out pingMessage);
			
			newRefernece.ReferencePingNote = pingMessage;
			if(pingReturn)
			{
				newRefernece.ServerState = ServerStates.Online;
				newRefernece.ReferenceLastSeen = DateTime.Now;
			} 
			else 
			{
				newRefernece.ServerState = ServerStates.Offline;
			}
			return pingReturn;
		}
		
		public bool Test(ServerReference reference, out string pingMessage)
		{
			return Ping (reference.ServerHost, out pingMessage);
		}

		public bool Ping(string host)
		{
			string pingMessage = String.Empty;
			return Ping (host, out pingMessage);
		}

		public bool Ping(string host, out string pingMessage)
		{
			bool returnValue = true;
			pingMessage = String.Empty;

			IPAddress address = Dns.GetHostEntry(host).AddressList.First();

			Ping ping = new Ping ();

			PingOptions pingOptions = new PingOptions ();
			pingOptions.DontFragment = true;

			byte[] buffer = new byte[32];

			for (int i = 0; i < 4; i++)
			{
				try
				{
					PingReply pingReply = ping.Send(address, 1000, buffer, pingOptions);
					if (!(pingReply == null))
					{
						switch (pingReply.Status)
						{
						case IPStatus.Success:
							pingMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
							break;
						case IPStatus.TimedOut:
							pingMessage = "Connection has timed out...";
							returnValue = false;
							break;
						default:
							pingMessage = string.Format("Ping failed: {0}", pingReply.Status.ToString());
							returnValue = false;
							break;
						}
					}
					else
					{
						pingMessage = "Connection failed for an unknown reason...";
						returnValue = false;
					}
				}
				catch (PingException ex)
				{
					pingMessage = string.Format("Connection Error: {0}", ex.Message);
					returnValue = false;
				}
				catch (Exception ex)
				{
					pingMessage = string.Format("Connection Error: {0}", ex.Message);
					returnValue = false;
				}
			}
			return returnValue;
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
