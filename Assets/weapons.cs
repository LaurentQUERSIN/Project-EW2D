using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponTypes
{
	STANDARD,
	RAPID_FIRE,
	MACHINE_GUN,
	MEGA_BOMB
}

public class Weapons
{
	private Dictionary<WeaponTypes, Weapon> _weapons;
	
	public Weapon getWeapon(WeaponTypes wp)
	{
		return _weapons[wp];
	}
	
	private static Weapons _instance;
	public static Weapons instance
	{
		get
		{
			if (_instance == null)
				_instance = new Weapons();
			return _instance;
		}
	}
	
	private Weapons()
	{
		_weapons = new Dictionary<WeaponTypes, Weapon>();
		_weapons.Add(WeaponTypes.STANDARD, new Weapon("standard gun", 1000, 50, 1, 5, 10));
		_weapons.Add(WeaponTypes.RAPID_FIRE, new Weapon("rapid fire gun", 1000, 20, 3, 15, 6));
		_weapons.Add(WeaponTypes.MACHINE_GUN, new Weapon("machine gun", 100, 10, 10, 10, 3));
		_weapons.Add(WeaponTypes.MEGA_BOMB, new Weapon("mega bomb", 1000, 9999, 0, 5, 0));
	}
}

public class Weapon
{
	private int _cooldown;
	private int _damage;
	private int _spread;
	private int _speed;
	private int _size;
	private string _name;
	
	public int cooldown { get { return _cooldown; } }
	public int damage { get { return _damage; } }
	public string name { get { return _name; } }
	public int spread { get { return _spread; } }
	public int size { get { return _size; } }
	public int speed { get { return _speed; } }
	
	public Weapon(string weapon_name, int weapon_cooldown, int weapon_damage, int weapon_spread, int weapon_speed, int weapon_size)
	{
		_name = weapon_name;
		_cooldown = weapon_cooldown;
		_damage = weapon_damage;
		_speed = weapon_speed;
		_spread = weapon_speed;
		_size = weapon_size;
	}
}
