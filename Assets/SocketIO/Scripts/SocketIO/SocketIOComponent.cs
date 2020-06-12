using System;
using System.Text;
using UnityEngine;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Transports;

namespace SocketIO
{
	public class SocketIOComponent : MonoBehaviour
	{
		public string url = "ws://127.0.0.1:4567/socket.io/?EIO=4&transport=websocket";
		private SocketManager _manager;

		#region socketIO_utils
		public void On(string ev , Action<SocketIOEvent> callback)
		{
			switch (ev)
			{
				case "open":
					_manager.Socket.On(SocketIOEventTypes.Connect , (socket, packet, args) =>
					{
						callback(new SocketIOEvent("open" , new JSONObject(packet.RemoveEventName(true))));
					});
					break;
				case "error":
					_manager.Socket.On(SocketIOEventTypes.Error , (socket, packet, args) =>
					{
						callback(new SocketIOEvent("error" , new JSONObject(packet.RemoveEventName(true))));
					});
					break;
				default:
					_manager.Socket.On(ev , (socket, packet, args) =>
					{
						callback(new SocketIOEvent(ev , new JSONObject(packet.RemoveEventName(true))));
					});
					break;
			}
		}

		public void Connect()
		{
			_manager.Open();
		}

		public void Emit(string ev)
		{
			print($"emit event {ev} none~~");
			_manager.Socket.Emit(ev, "{}");
		}
		public void Emit(string ev, JSONObject jsonObject)
		{
			var obj = (jsonObject == null) ? "{}":jsonObject.ToString();
			_manager.Socket.Emit(ev, obj);
		}
		public void Off(string ev, Action<SocketIOEvent> callback)
		{
			_manager.Socket.Off(ev , (socket, packet, args) =>
			{
				callback(new SocketIOEvent(name , new JSONObject(packet.ToString())));
			});
		}
		#endregion
		
		private void OnDestroy()
		{
			_manager.Close();
		}

		private void Awake()
		{
			var opts = new SocketOptions {ConnectWith = TransportTypes.WebSocket};
			var uri = new Uri("ws://s2.noj.tw:4567/socket.io/?EIO=3&transport=websocket");
			_manager = new SocketManager(uri , opts);
		}
	}
}
