using UnityEngine;
using System.Collections;
using Stormancer;
using Stormancer.Core;

public class myGameObject
{
	public uint id = 0;
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
	
	public virtual void updatePosition(float x, float y, float rot, float vx, float vy, long updateTime)
	{
		last_x = pos_x;
		last_y = pos_y;
		pos_x = x;
		pos_y = y;
		rotation = rot;
		vect_x = vx;
		vect_y = vy;
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

	public override void updatePosition(float x, float y, float rot, float vx, float vy, long updateTime)
	{
		base.updatePosition(x, y, rot, vx, vy, updateTime);
		ship.transform.position.Set(x, y, 0f);
		ship.transform.rotation.eulerAngles.Set(0f, 0f, rot);
		ship.GetComponent<Rigidbody>().AddForce(new Vector3(vx, vy, 0f));
	}
	
	public Player(uint pId, string pName, long updateTime)
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

	
	public bool isColliding(Player p, long time)
	{
		float x = pos_x + (vect_x * (weapon.speed * ((time - lastUpdate) / 1000f)));
		float y = pos_y + (vect_y * (weapon.speed * ((time - lastUpdate) / 1000f)));
		if (p.pos_x - 100 <= x && x <= p.pos_x + 100 && p.pos_y - 100 <= y && y <= p.pos_y + 100)
			return true;
		return false;
	}
	
	public Bullet(uint object_id, Player player, long updateTime)
	{
		id = object_id;
		name = player.name + " bullet " + id.ToString();
		rotation = player.rotation;
		color_red = player.color_red;
		color_blue = player.color_blue;
		color_green = player.color_green;
		weapon = player.weapon;
		lastUpdate = updateTime;
	}
}

