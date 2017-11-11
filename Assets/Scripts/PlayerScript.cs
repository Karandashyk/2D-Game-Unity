using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
	public Vector2 speed = new Vector2(10, 10);
	private Rigidbody2D rb;
	// 2 - направление движения
	private Vector2 movement;
	private bool faceRight = true;
	private int count;

	void Awake() 
	{
		DontDestroyOnLoad(transform.gameObject);
		rb = GetComponent<Rigidbody2D> ();
		if (!PlayerPrefs.HasKey("levelNew"))
		{
			PlayerPrefs.SetInt ("levelNew", 1);
			PlayerPrefs.Save ();
		}
	}

	void Update()
	{
		// 3 -  извлечь информацию оси
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// 4 - движение в каждом направлении
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);

		// 5 - Стрельба
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Замечание: Для пользователей Mac, Ctrl + стрелка - это плохая идея

		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript> ();
			if (weapon != null)
			{
				// ложь, так как игрок не враг
				weapon.Attack(false);
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			rb.AddForce (Vector2.up * 10000);
			rb.AddForce (Vector2.right * 5000);
		}

		// 6 – Убедиться, что игрок не выходит за рамки кадра
		var dist = (transform.position - Camera.main.transform.position).z;

		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).x;

		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
		).x;

		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).y;

		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
		).y;

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z
		);

		if (inputX > 0 && !faceRight) {
			flip ();
		} else if (inputX < 0 && faceRight) {
			flip ();
		}
		// Вот и весь метод Update

	}

	void flip ()
	{
		faceRight = !faceRight;
		transform.localScale = new Vector3 (
			transform.localScale.x * -1,
			transform.localScale.y,
			transform.localScale.z);
	}

	void FixedUpdate()
	{
		// 5 - перемещение игрового объекта
		gameObject.GetComponent<Rigidbody2D> ().velocity = movement;
	}

	void OnTriggerEnter2D (Collider2D coll) 
	{
		if (coll.CompareTag ("Coin")) 
		{
			count++;
			Destroy (coll.gameObject);
			if (count > 4) 
			{
				if (PlayerPrefs.GetInt ("levelNew") == 1) {
					PlayerPrefs.SetInt ("levelNew", 2);
					PlayerPrefs.Save ();
					Application.LoadLevel (2);
				} 
				else if (PlayerPrefs.GetInt ("levelNew") == 2) {
					Application.LoadLevel (3);
					PlayerPrefs.DeleteKey ("levelNew");
				}
				

			}

		}	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;

		// Столкновение с врагом
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Смерть врага
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp, false);

			damagePlayer = true;
		}

		// Повреждения у игрока
		if (damagePlayer)
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage(1, true);
		}
	}




}
