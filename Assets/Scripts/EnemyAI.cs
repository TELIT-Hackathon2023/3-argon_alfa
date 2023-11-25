using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour {
	[SerializeField] private NavMeshAgent2D agent;
	private Vector2 _destination;

	[SerializeField] private float setNewDestinationDelay = 5f;
	private float _setNewDestinationTimer;

	private void Start()
	{
		_setNewDestinationTimer = 0f;
		SetRandomDestination();
	}

	private void Update()
	{
		if ((Vector2)transform.position == _destination)
			SetRandomDestination();

		if (Time.time > _setNewDestinationTimer + setNewDestinationDelay)
		{
			SetRandomDestination();
			_setNewDestinationTimer = Time.time;
		}
	}

	private void SetRandomDestination()
	{
		_destination = new Vector2(transform.position.x + Random.Range(-3, 3), transform.position.y + Random.Range(-3, 3));
		agent.SetDestination(_destination);
	}
}
