using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDistant : MonoBehaviour
{
    private float selfDestructDistance = 10f;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").transform; // Assuming the player GameObject has the tag "Player"
        GameController gameController = GameObject.Find("UI").GetComponent<GameController>();
        selfDestructDistance = gameController.selfDestructDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // If the player is more than 20 units away, destroy this enemy
        if (distanceToPlayer > selfDestructDistance)
        {
            Destroy(gameObject);
            return; // Exit the method early as there's no need to do the rest if the enemy is destroyed
        }
        
    }
}
