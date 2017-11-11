using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Просто перемещает текущий объект игры
/// </summary>
public class MoveScript : MonoBehaviour
{
	// 1 - переменные

	/// <summary>
	/// Скорость объекта
	/// </summary>
	public Vector2 speed = new Vector2(2, 2);

	/// <summary>
	/// Направление движения
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);

	private Vector2 movement;

	void Update()
	{
		// 2 - Перемещение
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
	}

	void FixedUpdate()
	{
		// Применить движение к Rigidbody
		gameObject.GetComponent<Rigidbody2D>().velocity = movement;
	}
}
