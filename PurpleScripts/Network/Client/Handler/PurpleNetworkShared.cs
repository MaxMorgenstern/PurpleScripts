using UnityEngine;
using Entities.PurpleNetwork;

namespace PurpleNetwork.Client.Handler
{
	public class Shared
	{
		protected static void trigger_purple_event(PurpleNetworkClientEvent eve)
		{
			trigger_purple_event (eve, null);
		}

		protected static void trigger_purple_event(PurpleNetworkClientEvent eve, object passed_object)
		{
			if(eve != null)
				eve(passed_object);
		}
	}
}

