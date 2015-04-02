using System;
using System.Collections.Generic;

namespace PurpleNetwork.Spam
{
	public class Prevention
	{
		private static List<RequestQueue> requestQueue  = new List<RequestQueue>(); 

		public static bool AddClient(int clientID)
		{
			if(clientID == Constants.SERVER_ID_INT) return true;
			if(find_client(clientID) != null)
			{
				return false;
			}
			requestQueue.Add (new RequestQueue (clientID));
			return true;
		}

		public static bool AddClient(int clientID, int maxRequests, TimeSpan timeSpan)
		{
			if(clientID == Constants.SERVER_ID_INT) return true;
			if(find_client(clientID) != null)
			{
				return false;
			}
			requestQueue.Add (new RequestQueue (clientID, maxRequests, timeSpan));
			return true;
		}


		public static void UpdateLimit(int clientID, int maxRequests, TimeSpan timeSpan)
		{
			RequestQueue clientObject = find_client(clientID);
			if(clientObject != null)
			{
				clientObject.UpdateLimit (maxRequests, timeSpan);
			}
		}


		public static bool CanClientRequestNow(int clientID)
		{
			return CanClientRequestNow (clientID, true);
		}

		public static bool CanClientRequestNow(int clientID, bool activeRequest)
		{
			if(clientID == Constants.SERVER_ID_INT) return true;
			RequestQueue clientObject = find_client(clientID);
			if(clientObject == null)
			{
				return AddClient(clientID);
			}
			return clientObject.CanClientRequestNow (activeRequest);
		}


		public static void GetRequestsInTimespan(int clientID, out int request, out TimeSpan time)
		{
			request = 20;
			time = new TimeSpan (0, 1, 0);

			RequestQueue clientObject = find_client(clientID);
			if(clientObject == null)
			{
				clientObject.GetRequestsInTimespan(out request, out time);
			}
		}

		public static bool SendSpamResponse(int clientID)
		{
			RequestQueue clientObject = find_client(clientID);
			if(clientObject != null)
			{
				bool returnValue = clientObject.serverSpamResponseSend;
				clientObject.serverSpamResponseSend = false;
				return returnValue;
			}
			return false;
		}

		
		// PRIVATE /////////////////////////
		private static RequestQueue find_client(int clientID)
		{
			return requestQueue.Find(item => item.requestClientID == clientID);
		}

		
		// INTERNAL CLASS /////////////////////////
		private class RequestQueue
		{
			private int 			requestMaxInTimeSpan;
			private TimeSpan 		requestTimeSpan;
			private Queue<DateTime>	requestQueue;
			
			public int 				requestClientID;
			public bool				serverSpamResponseSend;

			public void UpdateLimit(int maxRequests, TimeSpan timeSpan)
			{
				requestMaxInTimeSpan = maxRequests;
				requestTimeSpan = timeSpan;
			}

			public void GetRequestsInTimespan(out int request, out TimeSpan time)
			{
				request = requestMaxInTimeSpan;
				time = requestTimeSpan;
			}

			public bool CanClientRequestNow()
			{
				return CanClientRequestNow (true);
			}

			public bool CanClientRequestNow(bool activeRequest)
			{
				UpdateQueue();
				if(activeRequest)
				{
					requestQueue.Enqueue(DateTime.Now);
				}
				return requestQueue.Count <= requestMaxInTimeSpan;
			}

			
			// PRIVATE /////////////////////////
			private void UpdateQueue()
			{
				while ((requestQueue.Count > 0) && (requestQueue.Peek().Add(requestTimeSpan) < DateTime.Now))
					requestQueue.Dequeue();
			}


			// CONSTRUCTOR /////////////////////////
			public RequestQueue(int clientID)
			{
				requestClientID = clientID;
				requestMaxInTimeSpan = PurpleConfig.Network.Server.Spam.MaxRequests;	// 20 Requests per Minute
				requestTimeSpan = new TimeSpan (0, 1, 0);
				requestQueue = new Queue<DateTime>(requestMaxInTimeSpan);
				serverSpamResponseSend = false;
			}

			public RequestQueue(int clientID, int maxRequests, TimeSpan timeSpan)
			{
				requestClientID = clientID;
				requestMaxInTimeSpan = maxRequests;
				requestTimeSpan = timeSpan;
				requestQueue = new Queue<DateTime>(requestMaxInTimeSpan);
				serverSpamResponseSend = false;
			}
		}
	}
}