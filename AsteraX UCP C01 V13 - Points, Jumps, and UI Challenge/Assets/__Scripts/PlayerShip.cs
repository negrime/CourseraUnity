#define DEBUG_PlayerShip_RespawnNotifications

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    // This is a somewhat protected private singleton for PlayerShip
    //? Singleton Instance

    private static PlayerShip _instance = null;

    public static PlayerShip Instance
    {
    
        get {return GameObject.FindObjectOfType<PlayerShip>();}
    }

    [Header("Set in Inspector")]
    public float        shipSpeed = 10f;
    public GameObject   bulletPrefab;

    Rigidbody           rigid;

    [Header("Player stats")]
    [SerializeField]
    private PlayerStats _playerStats;

    [SerializeField] 
    private GameObject _shipModel; // disable when we jump

    [Header("User Interface")]
    [SerializeField]
    private StatsView _statsView;
    
    [Header("Game Over View")]
    [SerializeField]
    private GameOverView _gameOverView;


    void Awake()
    {
        // NOTE: We don't need to check whether or not rigid is null because of [RequireComponent()] above
        rigid = GetComponent<Rigidbody>();
        _statsView.ShowJumps(_playerStats.GetJumps());
        _statsView.ShowScores(_playerStats.GetScores());
    }


    void Update()
    {
        // Using Horizontal and Vertical axes to set velocity
        float aX = CrossPlatformInputManager.GetAxis("Horizontal");
        float aY = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 vel = new Vector3(aX, aY);
        if (vel.magnitude > 1)
        {
            // Avoid speed multiplying by 1.414 when moving at a diagonal
            vel.Normalize();
        }

        rigid.velocity = vel * shipSpeed;

        // Mouse input for firing
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    public void Jump()
    {
        if (_playerStats.GetJumps() > 0)
        {
            _playerStats.UpdateJumps(-1);
            _shipModel.SetActive(false);
            StartCoroutine(Teleport(1));
        }
        else
        {
            GameOver();
        }
        _statsView.ShowJumps(_playerStats.GetJumps());
    }

    private void GameOver()
    {
        _gameOverView.gameObject.SetActive(true);
        Destroy(gameObject);
        _gameOverView.EnableGameOver(_playerStats.GetScores());
    }


    public void AddScores(int value)
    {
        _playerStats.UpdateScores(value);
        _statsView.ShowScores(_playerStats.GetScores());
    }


    private IEnumerator Teleport(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position =  GetSavePosition();
        _shipModel.SetActive(true);
    }


    private Vector3 GetSavePosition()
    {
        Vector3 newPosition = Vector3.zero;
        do
        {
            newPosition = new Vector3(Random.Range(-13, 13), Random.Range(-8, 7), transform.position.z);
            
        } while (!CheckSavePosition(newPosition));
        return newPosition;
    }


    private bool CheckSavePosition(Vector3 position)
    {
        var asteroids = AsteraX.GetAsteroids();
        foreach (var item in asteroids)
        {
            if (Vector2.Distance(position, item.transform.position) < 5)
            {
                return false;
            }              
        }
        return true;
    }

    
    void Fire()
    {
        // Get direction to the mouse
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);

        // Instantiate the Bullet and set its direction
        GameObject go = Instantiate<GameObject>(bulletPrefab);
        go.transform.position = transform.position;
        go.transform.LookAt(mPos3D);
    }

    static public float MAX_SPEED
    {
        get
        {
            return Instance.shipSpeed;
        }
    }
    
	static public Vector3 POSITION
    {
        get
        {
            return Instance.transform.position;
        }
    }
}
