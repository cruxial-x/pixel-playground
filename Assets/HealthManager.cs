using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerController player;
    private Image[] hearts;

    void Start()
    {
        hearts = new Image[player.hitPoints];
        for (int i = 0; i < player.hitPoints; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts[i] = heart.GetComponent<Image>();
        }
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.hitPoints)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}