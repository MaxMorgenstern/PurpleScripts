// ------------------------------------------------------------------------------
//  TODO : ALL
// ------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using _PurpleSerializer = PurpleSerializer;
using _JSON = Newtonsoft.Json.JsonConvert;

namespace PurpleNetwork
{
	// Purple Network with Queue
	public class PurpleNetworkQueue
	{
		public PurpleNetworkQueue ()
		{
		}
	}
}


// DEV ////////////////////////////
/*

	// TODO: PurpleNetworkQueue - klasse klonen die eine queue nutzt
		
		public enum queueType {broadcast, server, player};
		private Queue <message_queue_item> message_queue = new Queue<message_queue_item> (); //new Queue();


		public static void Broadcast (string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.broadcast, new NetworkPlayer ());
			} else {
				Broadcast(event_name, message);
			}
		}
		
		public static void ToServer (string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.server, new NetworkPlayer());
			} else {
				ToServer(event_name, message);
			}
		}
		
		public static void ToPlayer(NetworkPlayer player, string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.player, player);
			} else {
				ToPlayer(player, event_name, message);
			}
		}

		public static void ToPlayer(NetworkMessageInfo info, string event_name, object message, bool useQueue)
		{
			if (useQueue) {
				Instance.to_queue (event_name, message, queueType.player, info.sender);
			} else {
				ToPlayer(info, event_name, message);
			}
		}



		private void to_queue(string event_name, object message, queueType type, NetworkPlayer player)
		{
			message_queue_item mqi = new message_queue_item (event_name, message, type, player);
			message_queue.Enqueue (mqi);

		}

		public static void ProcessQueue(){ Instance.process_queue(); }

		private void process_queue()
		{
			List<message_queue_item> broadcast_list = new List<message_queue_item> ();
			List<message_queue_item> server_list = new List<message_queue_item> ();

			while (message_queue.Count > 0)
			{
				message_queue_item current_item = message_queue.Dequeue ();

				switch (current_item.type) 
				{
					case queueType.broadcast:
						broadcast_list.Add(current_item);
						break;

					case queueType.server:
						server_list.Add(current_item);
						break;

					case queueType.player:
						// Each seperate as this does only affect specific player
						to_sender(current_item.player, current_item.event_name, current_item.message);
						break;

					default:
						PurpleDebug.LogError("invalid queue type detected: " + current_item.type.ToString());
						break;
				}
			} // END WHILE

			// TODO - send broadcast and server list
			// queue_broadcast
			// queue_to_server
		}

		private class message_queue_item
		{
			public string event_name;
			public object message;
			public queueType type;
			public NetworkPlayer player;

			public message_queue_item(string en, object me, queueType ty, NetworkPlayer pl)
			{
				event_name = en;
				message = me;
				type = ty;
				player = pl;
			}
		}

		// SEND QUEUE TO ALL
		private void queue_broadcast (string event_name, object message)
		{
			string string_message = object_to_string_converter(message);
			purpleNetworkView.RPC("receive_purple_network_message_queue", RPCMode.All, event_name, string_message);
		}
		
		// SEND QUEUE TO SERVER
		private void queue_to_server (string event_name, object message)
		{
			string string_message = object_to_string_converter(message);
			if (Network.isServer)
			{
				receive_purple_network_message_queue(event_name, string_message, new NetworkMessageInfo());
			}
			else
			{
				purpleNetworkView.RPC("receive_purple_network_message_queue", RPCMode.Server, event_name, string_message);
			}
		}



		// RECEIVE ALL QUEUE DATA ////////////////////
		[RPC]
		void receive_purple_network_message_queue(string event_name, string xml_message, NetworkMessageInfo info)
		{
			// TODO
			PurpleDebug.Log ("receive_purple_network_message_queue(string event_name, string xml_message, NetworkMessageInfo info)");
			DebugPurpleDebug.Log (xml_message);
			eventListeners[event_name](xml_message);
		}

*/
// * ////////////////////////////