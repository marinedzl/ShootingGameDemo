using UnityEngine;

public class Player : MonoBehaviour
{
	Role m_role;
	Animator m_animtor;
	AnimatorStateInfo m_currentState;

	bool m_shift = false;

	void Start()
	{
		m_role = GetComponent<Role>();
		m_animtor = GetComponent<Animator>();
	}

	void Update()
	{
		m_currentState = m_animtor.GetCurrentAnimatorStateInfo(0);

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			m_shift = true;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			m_shift = false;
		}

		m_animtor.SetBool("Running", m_shift);

		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if (x != 0 || y != 0)
		{
			ResetTriggers();
			m_animtor.SetTrigger("Move");
		}

		bool running = false;

		if (m_currentState.IsTag("Walk"))
		{
			UpdateMove(m_role.WalkSpeed);
		}
		else if (m_currentState.IsTag("Run"))
		{
			running = true;
			UpdateMove(m_role.RunSpeed);
		}

		if (!running)
		{
			if (Input.GetMouseButtonUp(0))
			{
				ResetUpperTriggers();
				m_animtor.SetTrigger("Fire");
			}

			if (Input.GetKeyUp(KeyCode.R))
			{
				ResetUpperTriggers();
				m_animtor.SetTrigger("Reload");
			}
		}
	}

	void UpdateMove(float moveSpeed)
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if (x != 0 || y != 0)
		{
			float rotateSpeed = moveSpeed * 3;

			Vector3 dir = Camera.main.transform.TransformDirection(new Vector3(x, 0, y));
			dir = dir.normalized;

			Vector3 motion = dir * moveSpeed * Time.deltaTime;
			motion.y = -5;

			m_role.Move(motion);
			m_role.Lookat(dir, rotateSpeed * Time.deltaTime);
		}
		else
		{
			ResetTriggers();
			m_animtor.SetTrigger("Stand");
		}
	}

	void ResetTriggers()
	{
		m_animtor.ResetTrigger("Move");
		m_animtor.ResetTrigger("Stand");
	}

	void ResetUpperTriggers()
	{
		m_animtor.ResetTrigger("Reload");
		m_animtor.ResetTrigger("Fire");
	}
}
