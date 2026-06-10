using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    private GameBehavior _gameManager;
    private int _enemyLives = 3;
    public Transform PatrolRoute;
    private List<Transform> _locations = new List<Transform>();
    private int _locationIndex = 0;

    private Transform _player;

    public int EnemyLives
    {
        get { return _enemyLives; }
        private set
        {
            _enemyLives = value;
            if (_enemyLives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down!");
            }
        }
    }

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").transform;
        _gameManager = GameObject.Find("Game manager").GetComponent<GameBehavior>();

        if (PatrolRoute != null)
        {
            foreach (Transform child in PatrolRoute)
            {
                _locations.Add(child);
            }
        }

        if (_locations.Count > 0)
        {
            MoveToNextPatrolLocation();
        }

    }

    void Update()
    {
        if (_locations.Count > 0 && _navAgent.remainingDistance < 0.5f && !_navAgent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (_locations.Count == 0) return;

        _navAgent.destination = _locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % _locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player detected - attack!");
            if (_gameManager != null)
            {
                _gameManager.HP -= 2; // Damage 2 HP
                Debug.Log("Player HP: " + _gameManager.HP);
            }

            if (_navAgent != null && _player != null)
            {
                _navAgent.destination = _player.position;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
        if (_locations.Count > 0)
        {
            MoveToNextPatrolLocation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit! Enemy lives: " + _enemyLives);
        }
    }
}
