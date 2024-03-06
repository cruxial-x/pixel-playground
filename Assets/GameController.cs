using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool easyMode = false;
    public float selfDestructDistance = 10f;
    public GameObject gameOverScreen;
    private TutorialManager tutorialManager;

    void Start()
    {
        tutorialManager = GetComponentInChildren<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverScreen.activeSelf && Input.GetButtonDown("Jump"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        tutorialManager.ShowSpaceKey();
    }
}
