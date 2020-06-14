using System;
using System.Text;
using UnityEngine;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Transports;

namespace SocketIO
{
	public class SocketIOComponent : MonoBehaviour
	{
		private readonly SocketOptions _opts = new SocketOptions {ConnectWith = TransportTypes.WebSocket};
		public string Url
		{
			get => url;
			set
			{
				url = value;
				print($"change url to {url}");
				_manager = new SocketManager(new Uri(url) , _opts);
			}
		}

		[SerializeField] private string url = "ws://localhost:4567/socket.io/?EIO=3&transport=websocket";
		private SocketManager _manager;

		#region socketIO_utils
		public void On(string ev , Action<SocketIOEvent> callback)
		{
			switch (ev)
			{
				case "open":
					_manager.Socket.On(SocketIOEventTypes.Connect , (socket, packet, args) =>
					{
						try
						{
							callback(new SocketIOEvent("open" , new JSONObject(packet.RemoveEventName(true))));
						}
						catch (Exception e)
						{
							print($"socket open error:{e}");
							throw;
						}
					});
					break;
				case "error":
					_manager.Socket.On(SocketIOEventTypes.Error , (socket, packet, args) => callback(new SocketIOEvent("error" , JSONObject.nullJO)));
					break;
				default:
					_manager.Socket.On(ev , (socket, packet, args) =>
					{
						var obj = (packet == null) ? JSONObject.nullJO : new JSONObject(packet.RemoveEventName(true));
						callback(new SocketIOEvent(ev , obj));
					});
					break;
			}
		}

		public void Connect()
		{
			print("trying to connect on server");
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
			var uri = new Uri(Url);
			_manager = new SocketManager(uri , _opts);
		}
	}
}
