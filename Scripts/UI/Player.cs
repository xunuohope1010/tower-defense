using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Stat health, energy, shield;

	// Use this for initialization
	void Awake()
    {
        health.Initialize();
        energy.Initialize();
        shield.Initialize();
	}
	
	// Update is called once per frame
	void Update()
    {
        // functionality for allowing the user to change bar values by pressing keys
		if (Input.GetKeyDown(KeyCode.Q))
        {
            health.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            health.CurrentVal += 10;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            energy.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            energy.CurrentVal += 10;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            shield.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            shield.CurrentVal += 10;
        }
    }
}
