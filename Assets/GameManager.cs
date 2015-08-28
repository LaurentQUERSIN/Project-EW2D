using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Stormancer;
using System.Threading.Tasks;
using Stormancer.Core;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections.Concurrent;
using Stormancer.Diagnostics;

public class GameManager : MonoBehaviour
{
	public bool							connecting = false;
	public bool							connected = false;
	public bool 						gamePaused = true;
	public ConnexionPanel 				connexionPanel;
	public Player 						localPlayer;
	public GameObject 					playerShip;
	public GameObject					opponentShip;

	private Scene 						_scene;
	private Client 						_client;
	private Dictionary<uint, Player>	_players;
	

	public void Connect()
	{
		if (connecting == true)
			return;
		if (connexionPanel.username.text == "")
			connexionPanel.errorText.text = "please enter a user name.";
		else
		{
			connecting = true;
			Debug.Log("starting connection");
			UniRx.MainThreadDispatcher.Initialize();
			var Config = ClientConfiguration.ForAccount("7794da14-4d7d-b5b5-a717-47df09ca8492", "projectew2d");
			var client = new Client(Config);
			_client = client;

			localPlayer = new Player(0, connexionPanel.username.text, 0);
			localPlayer.ship = playerShip;
			localPlayer.color_red = connexionPanel.redSlider.value;
			localPlayer.color_blue = connexionPanel.blueSlider.value;
			localPlayer.color_green = connexionPanel.greenSlider.value;
			playerShip.GetComponent<Renderer>().material.color = new Color(localPlayer.color_red, localPlayer.color_green, localPlayer.color_blue);

			Debug.Log("config complete");
			client.GetPublicScene("test", (myGameObject)localPlayer).ContinueWith ( task =>
			                                                              {
				var scene = task.Result;
				_scene = scene;
				Debug.Log ("configuring routes");
				_scene.AddRoute("chat", onChat);
				_scene.AddRoute("update_position", onUpdatePosition);
				_scene.AddRoute("get_id", onGetId);
				_scene.AddRoute("update_status", onUpdateStatus);
				Debug.Log ("connecting to remote scene");
				_scene.Connect();
				UniRx.MainThreadDispatcher.Post(() =>
				                                {
					checkIfConnected();
				});
			});
			Debug.Log ("tentative de connexion terminée, en attente de la réponse");
		}
	}

	public void checkIfConnected()
	{
		connecting = false;

		if (_scene.Connected == false)
		{
			connexionPanel.errorText.text = "Connexion failed";
			Debug.Log ("connexion failed");
		}
		else
		{
			Debug.Log ("connexion succeded");
			connexionPanel.gameObject.SetActive(false);
			playerShip.GetComponent<Renderer>().enabled = true;
			gamePaused = false;
			connected = true;
		}
	}

	public void onUpdatePosition(Packet<IScenePeer> obj)
	{
		using (var reader = new BinaryReader(obj.Stream))
		{
			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				var id = reader.ReadUInt32();
				var x = reader.ReadSingle();
				var y = reader.ReadSingle();
				var rot = reader.ReadSingle();
				var vx = reader.ReadSingle();
				var vy = reader.ReadSingle();
				var updateTime = reader.ReadInt64();

				if (_players.ContainsKey(id))
					_players[id].updatePosition(x, y, rot, vx, vy, updateTime);
			}
			reader.Close ();
		}
	}

	public void onChat(Packet<IScenePeer> obj)
	{
		Debug.Log (obj.Stream);
	}

	public void onUpdateStatus(Packet<IScenePeer> obj)
	{
		using (var reader = new BinaryReader(obj.Stream))
		{
			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				var id = reader.ReadUInt32();
				var status = reader.ReadInt16();

				switch (status)
				{
					case (0):
						if (_players.ContainsKey(id))
							_players[id].ship.GetComponent<Renderer>().enabled = true;
						break;
				case (1):
					if (_players.ContainsKey(id))
						_players[id].ship.GetComponent<Renderer>().enabled = false;
					break;
				case (2):
					if (!_players.ContainsKey(id))
					{
						GameObject newShip = Instantiate(opponentShip);
						Player newPlayer = new Player();

						var red = reader.ReadSingle();
						var blue = reader.ReadSingle();
						var green = reader.ReadSingle();

						newPlayer.id = id;
						newPlayer.color_red = red;
						newPlayer.color_blue = blue;
						newPlayer.color_green = green;
						newPlayer.ship = newShip;

						newPlayer.ship.GetComponent<Renderer>().material.color = new Color(red, green, blue);
						_players.Add(id, newPlayer);
					}
					break;
				case (3):
					if (_players.ContainsKey(id))
					{
						Destroy(_players[id].ship);
						_players.Remove (id);
					}
					break;
				}

			}
			reader.Close ();
		}
	}

	public void onGetId(Packet<IScenePeer> obj)
	{
		using (var reader = new BinaryReader(obj.Stream))
		{
			var id = reader.ReadUInt32();
			reader.Close();
			localPlayer.id = id;
		}
	}

	void Start()
	{
		_players = new Dictionary<uint, Player>();
	}

	void Update()
	{
		if (connected == true && _scene.Connected)
		{
			localPlayer.updatePositionFromShip();
			_scene.SendPacket("update_position", s =>
			            {
				using (var writer = new BinaryWriter(s, Encoding.UTF8))
				{
					writer.Write (localPlayer.id);
					writer.Write (localPlayer.pos_x);
					writer.Write (localPlayer.pos_y);
					writer.Write (localPlayer.rotation);
				}
			}, PacketPriority.MEDIUM_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED);
		}
	}
}
