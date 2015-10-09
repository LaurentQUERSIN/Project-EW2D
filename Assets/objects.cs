using UnityEngine;
using System.Collections;
using Stormancer;
using Stormancer.Core;

public class myGameObject
{
	public long id = 0;
	public string name = "";
	public float pos_x = 0;
	public float pos_y = 0;
	public float rotation = 0;
	public long lastUpdate = 0;
	
	public float color_red = 1;
	public float color_blue = 1;
	public float color_green = 1;
}

public class MovingObject : myGameObject
{
	public float vect_x = 0;
	public float vect_y = 0;
	public float last_x = 0;
	public float last_y = 0;
	
	public virtual void updatePosition(float x, float y, long updateTime)
	{
        vect_x = pos_x - last_x;
        vect_y = pos_y - last_y;
		last_x = pos_x;
		last_y = pos_y;
		pos_x = x;
		pos_y = y;
		lastUpdate = updateTime;
	}
}

public class Player : MovingObject
{
	public int life = 100;
	public int lastFired = 0;
	public int lastHit = 0;
	public StatusTypes status = StatusTypes.ALIVE;
	public Weapon weapon = null;
	public GameObject ship;

	public void updatePositionFromShip()
	{
		pos_x = ship.transform.position.x;
		pos_y = ship.transform.position.y;
		rotation = ship.transform.rotation.eulerAngles.z;

	}

	public override void updatePosition(float x, float y, long updateTime)
	{
        //ship.transform.position = new Vector3(x, y, 0f);
        //base.updatePosition(x, y, updateTime);
        //if (vect_x < 0.001f)
        //          vect_x = 0;
        //if (vect_y < 0.001f)
        //          vect_y = 0;
        //ship.GetComponent<Rigidbody>().AddForce(new Vector3(vect_x, vect_y, 0f));
        //ship.GetComponent<Rigidbody>().MovePosition(new Vector3(x, y, 0f));
        //ship.transform.position = Vector3.Lerp(ship.transform.position, new Vector3(x, y, 0f), 0.5f);
        ship.GetComponent<ShipInterpolator>().setNextPosition(new Vector3(x, y, 0f));
    }

    public Player(long pId, string pName, long updateTime)
	{
		id = pId;
		name = pName;
		weapon = Weapons.instance.getWeapon(WeaponTypes.STANDARD);
		lastUpdate = updateTime;
	}
	public Player(myGameObject obj, long updateTime)
	{
		id = obj.id;
		name = obj.name;
		pos_x = obj.pos_x;
		pos_y = obj.pos_y;
		rotation = obj.rotation;
		color_red = obj.color_red;
		color_blue = obj.color_blue;
		color_green = obj.color_green;
		lastUpdate = updateTime;
	}

	public Player(){}
	public Player(int test) {} 
}

public class Bullet : MovingObject
{
	public Weapon weapon = null;
	public GameObject bullet = null;
	
	public Bullet(long object_id, Player player, long updateTime, GameObject go)
	{
		id = object_id;
		weapon = player.weapon;
		lastUpdate = updateTime;
        bullet = go;
	}

    public void updatePosition(float x, float y, float vx, float vy, long updateTime)
    {
        bullet.transform.position = new Vector3(x, y, 0f);
        bullet.GetComponent<Rigidbody>().AddForce(new Vector3(vx, vy, 0f));
    }
}

