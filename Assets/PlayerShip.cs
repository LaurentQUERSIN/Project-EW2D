using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Stormancer;
using Stormancer.Core;

public class PlayerShip : MonoBehaviour 
{
    public Camera myCamera;
    public GameManager GM;

    private float _lastUpdate;

	void Update ()
	{
		if (this.GetComponent<GameManager>().gamePaused == false)
		{
            //this.transform.position = Vector3.Lerp(this.transform.position, nextPosition, 0.3f);





            if (GM.scene != null && GM.scene.Connected == true && GM.client != null && _lastUpdate + 50 < GM.client.Clock)
            {
                if (Input.GetKeyDown(KeyCode.Z) == true)
                    GM.scene.SendPacket("enable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(0); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyDown(KeyCode.S) == true)
                    GM.scene.SendPacket("enable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(1); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyDown(KeyCode.Q) == true)
                    GM.scene.SendPacket("enable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(2); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyDown(KeyCode.D) == true)
                    GM.scene.SendPacket("enable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(3); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);

                if (Input.GetKeyUp(KeyCode.Z) == true)
                    GM.scene.SendPacket("disable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(0); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyUp(KeyCode.S) == true)
                    GM.scene.SendPacket("disable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(1); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyUp(KeyCode.Q) == true)
                    GM.scene.SendPacket("disable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(2); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);
                if (Input.GetKeyUp(KeyCode.D) == true)
                    GM.scene.SendPacket("disable_action", s => { var writer = new BinaryWriter(s, System.Text.Encoding.UTF8); writer.Write(3); }, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED);

                if (Input.GetMouseButton(0))
                {
                    GM.scene.SendPacket("firing_weapon", s =>
                    {
                        var writer = new BinaryWriter(s, System.Text.Encoding.UTF8);
                        Ray ray;
                        ray = myCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        Physics.Raycast(ray, out hit);

                        writer.Write(GM.localPlayer.id);
                        writer.Write(hit.point.x);
                        writer.Write(hit.point.y);
                        writer.Write(GM.client.Clock);
                    });
                }
            }
		}
	}
}
