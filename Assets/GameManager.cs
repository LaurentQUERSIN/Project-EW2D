using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Stormancer;
using System.Threading.Tasks;
using Stormancer.Core;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Text;
using Stormancer.Diagnostics;

public class GameManager : MonoBehaviour
{
	public bool 						gamePaused = true;
	public ConnexionPanel 				connexionPanel;
	public Player 						localPlayer;
	public GameObject 					playerShip;
	public GameObject					opponentShip;

	private Scene 						_scene;
	private Client 						_client;
	private ConcurrentDictionary<long, Player>	_players = new ConcurrentDictionary<long, Player>();
    private bool _connecting = false;
    private bool _connected = false;


    public void Connect()
	{
		if (_connecting == true)
			return;
        if (connexionPanel.username.text == "")
            connexionPanel.errorText.text = "please enter a user name.";
        else
        {
            _connecting = true;
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
            client.GetPublicScene("test", (myGameObject)localPlayer).ContinueWith(task =>
           {
               if (task.IsFaulted)
               {
                   Debug.Log("connection failed, cannot get scene : " + task.Exception.Message);
                   StormancerActionHandler.Post(() =>
                   {
                       connexionPanel.errorText.text = "Connexion failed";
                   });
                   _connecting = false;
               }
               else
               {
                   var scene = task.Result;
                   _scene = scene;
                   Debug.Log("configuring routes");
                   _scene.AddRoute("chat", onChat);
                   _scene.AddRoute("update_position", onUpdatePosition);
                   _scene.AddRoute("player_connected", onPlayerConnected);
                   _scene.AddRoute("player_disconnected", onPlayerDisconnected);
                   _scene.AddRoute("get_id", onGetId);
                   _scene.AddRoute("update_status", onUpdateStatus);
                   Debug.Log("connecting to remote scene");
                   _scene.Connect().ContinueWith(t =>
                   {
                       if (_scene.Connected)
                       {
                           _connected = true;
                           _connecting = false;
                           Debug.Log("connection successful");
                           StormancerActionHandler.Post(() => {
                               connexionPanel.gameObject.SetActive(false);
                               playerShip.GetComponent<Renderer>().enabled = true;
                               gamePaused = false;
                               _connected = true;
                           });
                       }
                       else
                       {
                           Debug.Log("connection failed: " + t.Exception.InnerException.Message);
                           _connecting = false;
                           StormancerActionHandler.Post(() => {
                               connexionPanel.errorText.text = "Connexion failed";
                           });
                       }
                   });
               }
           });
		}
	}

	public void onUpdatePosition(Packet<IScenePeer> obj)
	{
        var reader = new BinaryReader(obj.Stream, Encoding.UTF8);
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var id = reader.ReadInt64();
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();

            if (_players.ContainsKey(id) == true && localPlayer.id != id)
            {
                StormancerActionHandler.Post(() =>
                   {
                       _players[id].updatePosition(x, y, _client.Clock);
                   });
            }
        }
	}

	public void onChat(Packet<IScenePeer> obj)
	{
		Debug.Log (obj.ReadObject<string>());
	}

	public void onUpdateStatus(Packet<IScenePeer> obj)
	{
		using (var reader = new BinaryReader(obj.Stream))
		{
			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				var id = reader.ReadInt32();
				var status = reader.ReadInt16();

                Debug.Log("updating ship status");

				switch (status)
				{
					case (0):
						if (_players.ContainsKey(id))
                        {
                            StormancerActionHandler.Post(() =>
                            {
                                _players[id].ship.GetComponent<Renderer>().enabled = true;
                            });
                        }
                        break;
			    	case (1):
                        if (_players.ContainsKey(id))
                        {
                            StormancerActionHandler.Post(() =>
                            {
                                _players[id].ship.GetComponent<Renderer>().enabled = false;
                            });
                        }
				    	break;
				}
			}
		}
	}

    public void onPlayerConnected(Packet<IScenePeer> packet)
    {
        var reader = new BinaryReader(packet.Stream);
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {

            var id = reader.ReadInt32();
            var status = reader.ReadInt32();
            var red = reader.ReadSingle();
            var blue = reader.ReadSingle();
            var green = reader.ReadSingle();

            if (_players.ContainsKey(id) == false && localPlayer.id != id)
            {
                StormancerActionHandler.Post(() =>
                {
                    GameObject newShip = Instantiate(opponentShip);
                    Player newPlayer = new Player();

                    newPlayer.id = id;
                    newPlayer.color_red = red;
                    newPlayer.color_blue = blue;
                    newPlayer.color_green = green;
                    newPlayer.ship = newShip;

                    newPlayer.ship.GetComponent<Renderer>().material.color = new Color(red, green, blue);
                    if (status == 1)
                    {
                        newPlayer.ship.GetComponent<Renderer>().enabled = false;
                    }
                    _players.TryAdd(id, newPlayer);
                });
            }
        }
    }

    public void onPlayerDisconnected(Packet<IScenePeer> packet)
    {
        Player temp;
        var reader = new BinaryReader(packet.Stream);
        var id = reader.ReadInt32();

        Debug.Log("a player quit the game");

        if (_players.ContainsKey(id) && localPlayer.id != id)
        {
            StormancerActionHandler.Post(() =>
            {
                Destroy(_players[id].ship);
                _players.TryRemove(id, out temp);
            });
        }
    }

	public void onGetId(Packet<IScenePeer> obj)
	{
        var reader = new BinaryReader(obj.Stream);
        var id = reader.ReadInt64();
        localPlayer.id = id;
	}

    private long _lastUpdate;
	void Update()
	{
        if (_connected == true)
        {
            if (_lastUpdate + 100 < _client.Clock)
            {
                _lastUpdate = _client.Clock;
                localPlayer.updatePositionFromShip();
                _scene.SendPacket("update_position", s =>
               {
                   var writer = new BinaryWriter(s, Encoding.UTF8);
                   writer.Write(localPlayer.pos_x);
                   writer.Write(localPlayer.pos_y);
                   writer.Write(localPlayer.rotation);
               }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED);
            }
        }
	}

    void OnApplicationQuit()
    {
        _client.Disconnect();
    }
}
