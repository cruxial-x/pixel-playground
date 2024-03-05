using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject rightKey;
    public GameObject ctrlKey;
    public GameObject spaceKey;
    // Start is called before the first frame update
    void Start()
    {
        rightKey.SetActive(true);
        ctrlKey.SetActive(false);
        spaceKey.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (rightKey.activeSelf && Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowCtrlKey();
        }
        else if (ctrlKey.activeSelf && Input.GetKeyDown(KeyCode.LeftControl))
        {
            ShowSpaceKey();
        }
        else if (spaceKey.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            HideTutorial();
        }
        
    }
    public void ShowCtrlKey()
    {
        rightKey.SetActive(false);
        ctrlKey.SetActive(true);
    }
    public void ShowSpaceKey()
    {
        ctrlKey.SetActive(false);
        spaceKey.SetActive(true);
    }
    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
