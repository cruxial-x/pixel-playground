using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject rightKey;
    public GameObject ctrlKey;
    public GameObject spaceKey;

    private Animator rightKeyAnimator;
    private Animator ctrlKeyAnimator;
    private Animator spaceKeyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rightKey.SetActive(true);
        ctrlKey.SetActive(false);
        spaceKey.SetActive(false);

        rightKeyAnimator = rightKey.GetComponent<Animator>();
        ctrlKeyAnimator = ctrlKey.GetComponent<Animator>();
        spaceKeyAnimator = spaceKey.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightKey.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rightKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                ShowCtrlKey();
            }
        }
        else if (ctrlKey.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ctrlKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                ShowSpaceKey();
            }
        }
        else if (spaceKey.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spaceKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                HideTutorial();
            }
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