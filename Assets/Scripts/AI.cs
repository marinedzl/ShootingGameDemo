using UnityEngine;

public class AI : MonoBehaviour
{
	public float SearchRange = 20;

	Role m_role;
	Animator m_animtor;
	AnimatorStateInfo m_currentState;

	Role m_target;
	Vector3 m_moveDirection;
	float m_elapsedTime = 0;

	public enum State
	{
		Idle,
		Move,
		Attack,
		Reload,
	}

	State m_state = State.Idle;

	void Start()
	{
		m_role = GetComponent<Role>();
		m_animtor = GetComponent<Animator>();
	}

	void FindTarget()
	{
		m_target = null;
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		if (go != null)
		{
			Vector3 delta = go.transform.position - transform.position;
			delta.y = 0;
			float distance = delta.magnitude;
			if (distance <= SearchRange)
			{
				m_target = go.GetComponent<Role>();
			}
		}
	}

	void RandomMove()
	{
		float angle = Random.Range(0, 360);
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		Vector3 dir = rotation * Vector3.forward;
		m_moveDirection = dir;
		m_elapsedTime = Random.Range(1, 3);
	}

	void RandomAttack()
	{
		m_elapsedTime = Random.Range(1.0f, 1.5f);
	}

	void RandomIdle()
	{
		m_elapsedTime = Random.Range(1.0f, 1.5f);
	}
	
	void Update()
	{
		m_currentState = m_animtor.GetCurrentAnimatorStateInfo(0);

		switch (m_state)
		{
			case State.Idle:
				{
					FindTarget();

					if (m_target != null)
					{
						m_state = State.Move;
					}
					else
					{
						if (m_elapsedTime <= 0)
						{
							RandomMove();
							m_state = State.Move;
						}
						else
						{
							m_elapsedTime -= Time.deltaTime;
							ResetTriggers();
							m_animtor.SetTrigger("Stand");
						}
					}
				}
				break;
			case State.Move:
				{
					FindTarget();

					if (m_elapsedTime > 0)
					{
						m_elapsedTime -= Time.deltaTime;
						ResetTriggers();
						m_animtor.SetTrigger("Move");
						Move(m_moveDirection, m_role.WalkSpeed);
					}
					else
					{
						if (m_target != null)
						{
							RandomAttack();
							ResetTriggers();
							m_animtor.SetTrigger("Stand");
							m_state = State.Attack;
						}
						else
						{
							RandomIdle();
							ResetTriggers();
							m_animtor.SetTrigger("Stand");
							m_state = State.Idle;
						}
					}
				}
				break;
			case State.Attack:
				{
					if (m_role.BulletCount <= 0)
					{
						m_elapsedTime = 2;
						ResetUpperTriggers();
						m_animtor.SetTrigger("Reload");
						m_state = State.Reload;
					}

					if (m_elapsedTime > 0)
					{
						float rotateSpeed = 20;
						Vector3 dir = m_target.transform.position - transform.position;
						dir.y = 0;
						m_role.Lookat(dir, rotateSpeed * Time.deltaTime);

						m_elapsedTime -= Time.deltaTime;
						m_animtor.SetTrigger("Fire");
					}
					else
					{
						ResetUpperTriggers();
						RandomMove();
						m_state = State.Move;
					}
				}
				break;
			case State.Reload:
				{
					if (m_elapsedTime > 0)
					{
						m_elapsedTime -= Time.deltaTime;
					}
					else
					{
						m_role.Reload();
						RandomAttack();
						m_state = State.Attack;
					}
				}
				break;
			default:
				break;
		}
	}

	void Move(Vector3 dir, float moveSpeed)
	{
		float rotateSpeed = moveSpeed * 3;

		Vector3 motion = dir * moveSpeed * Time.deltaTime;
		motion.y = -5;

		m_role.Move(motion);
		m_role.Lookat(dir, rotateSpeed * Time.deltaTime);
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
