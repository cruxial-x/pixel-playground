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
            if (Input.GetAxis("Horizontal") == 0)
            {
                rightKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetAxis("Horizontal") != 0)
            {
                ShowCtrlKey();
            }
        }
        else if (ctrlKey.activeSelf)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ctrlKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                ShowSpaceKey();
            }
        }
        else if (spaceKey.activeSelf)
        {
            if (Input.GetButtonDown("Jump"))
            {
                spaceKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetButtonUp("Jump"))
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
        spaceKey.SetActive(false);
    }
}