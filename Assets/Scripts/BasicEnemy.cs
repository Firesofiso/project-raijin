using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
	public int curHealth, maxHealth;

	public float attackRange, attackSpeed;
	public int attackDamage;
	public float speed = 1;
	
	private Rigidbody2D _rigidBody;
	private PlayerController _target;
	private Coroutine _atkCoroutine;
	private bool _attacking;

	// Start is called before the first frame update
	void Start()
    {
		_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		// Find the closest player
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length > 0) {
			float closestDistance = 99999f;
			GameObject closestPlayer = players[0];
			Vector3 direction = new Vector3(0,0,0);

			foreach (GameObject player in players)
			{
				Vector3 heading = player.transform.position - transform.position;
				float distance = heading.magnitude;
				direction = heading / distance;
				if (distance < closestDistance) {
					closestDistance = distance;
					closestPlayer = player;
					_target = closestPlayer.GetComponent<PlayerController>();
				}
			}

			//Gets mouse position, you can define Z to be in the position you want the weapon to be in
			var offset = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.y - transform.position.y);
			var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle - 90);

			if (closestDistance <= attackRange) {
				// Start Attack Coroutine
				if (!_attacking && _target.curHealth > 0) {
					_rigidBody.velocity = new Vector2(0, 0);
					_atkCoroutine = StartCoroutine(Attack(attackSpeed));
				}
				
			} else {
				// Stop attacking
				if (_atkCoroutine != null) {
					StopCoroutine(_atkCoroutine);
					_attacking = false;
				}
				// Move towards closest player
				Move(speed, direction.x, direction.y);
			}
		}
	}

	public void Move(float i_fSpeed, float i_fXAxis, float i_fYAxis)
	{
		_rigidBody.velocity = new Vector2(Mathf.Lerp(0, i_fXAxis * i_fSpeed, 0.8f),
										  Mathf.Lerp(0, i_fYAxis * i_fSpeed, 0.8f));
	}

	public IEnumerator Attack(float i_fAtkSpeed)
	{
		_attacking = true;
		while (_attacking)
		{
			//Debug.Log("Attacking");
			// Make the attack
			_target.TakeDamage(attackDamage);
			if (_target.curHealth <= 0)
			{
				_attacking = false;
				StopCoroutine(_atkCoroutine);
			}
			yield return new WaitForSeconds(i_fAtkSpeed);
		}
	}
}
