using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
       // if no enemies are present, spawn a new enemy
         if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
         {
              Instantiate(enemyPrefab, transform.position, Quaternion.identity, null);
         } 
    }
}
