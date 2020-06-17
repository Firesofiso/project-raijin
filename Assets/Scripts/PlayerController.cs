using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int curHealth, maxHealth;
	public int curMana, maxMana;

	public GameObject weaponObject;
	public int numAttacks, attackCount;
	public float atkSpeed;
	public float speed = 1;
	private Rigidbody2D _rigidBody;

	public bool _canAttack = true;
	private float comboTimeout = 1;

	// Start is called before the first frame update
	void Start()
    {
		_rigidBody = GetComponent<Rigidbody2D>();
		curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log(_canAttack);

		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		//if (moveX != 0 && moveY != 0) {
		//	moveX /= 2;
		//	moveY /= 2;
		//}

		//Gets mouse position, you can define Z to be in the position you want the weapon to be in
		var mouse = Input.mousePosition;
		var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle -90);

		Move(speed, moveX, moveY);

		if (Input.GetButtonDown("Primary") && !weaponObject.activeInHierarchy && _canAttack) {
			MeleeAttack();
		}
	}

	public void Move(float i_fSpeed, float i_fXAxis, float i_fYAxis)
	{ 
		_rigidBody.velocity = new Vector2(Mathf.Lerp(0, i_fXAxis * i_fSpeed, 0.8f),
										  Mathf.Lerp(0, i_fYAxis * i_fSpeed, 0.8f));
	}

	public void TakeDamage(int damage) {
		if (curHealth - damage < 0) {
			// Dead
			curHealth = 0;
		} else {
			curHealth -= damage;
		}
	}

	public void MeleeAttack() {
		if (_canAttack) {
			StartCoroutine(SetWeaponActive());
		}
	}

	public IEnumerator SetWeaponActive() {
		weaponObject.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		weaponObject.SetActive(false);
		StartCoroutine(AttackDowntime(1 / atkSpeed));
	}

	public IEnumerator AttackDowntime(float i_fWaitTime) {
		_canAttack = false;
		yield return new WaitForSeconds(attackCount < numAttacks ? 0.1f : i_fWaitTime);
		attackCount++;
		// Debug.Log("Attack Count: " + attackCount);
		// Debug.Log("Combo timer: " + (attackCount < numAttacks ? 0.1f : i_fWaitTime));
		if (attackCount == numAttacks) {
			attackCount = 0;
		}
		_canAttack = true;
	}
}
