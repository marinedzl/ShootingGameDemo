using UnityEngine;

public class Role : MonoBehaviour
{
	public float WalkSpeed = 2;
	public float RunSpeed = 6;
	public Transform m_weaponHandle;
	public Transform m_projectileHandle;

	CharacterController m_cc;
	int m_bulletCount = 0;

	public int BulletCount
	{
		get { return m_bulletCount; }
	}

	void Start()
	{
		m_cc = GetComponent<CharacterController>();
		ChangeWeapon();
		Reload();
	}

	void Update()
	{

	}

	public void Move(Vector3 motion)
	{
		m_cc.Move(motion);
	}

	public void Lookat(Vector3 dir, float lerp)
	{
		dir.y = 0;
		Quaternion lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lerp);
	}

	void ChangeWeapon()
	{
		GameObject go = Instantiate(Resources.Load("Weapon/plasma_gun")) as GameObject;
		go.transform.parent = m_weaponHandle;
		go.transform.localScale = Vector3.one;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localPosition = Vector3.zero;
	}

	public void Reload()
	{
		m_bulletCount = 10;
	}

	void OnShot()
	{
		GameObject go = Instantiate(Resources.Load("Weapon/plasma_projectile")) as GameObject;
		go.transform.rotation = m_projectileHandle.rotation;
		go.transform.position = m_projectileHandle.position;
		go.GetComponent<Rigidbody>().velocity = go.transform.forward * 100f;
		Destroy(go, 0.5f);
		--m_bulletCount;
	}
}
