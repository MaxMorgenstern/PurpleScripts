using UnityEngine;
namespace PurpleNetwork
{
	// DELEGATES FOR CALLBACK AND EVENTS
	public delegate void PurpleNetCallback(object converted_object, NetworkPlayer network_info); // With message
	public delegate void PurpleNetworkEvent(object passed_object, NetworkPlayer network_info); // network event
}
