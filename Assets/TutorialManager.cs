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

    private float defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        bool hasCompletedTutorial = PlayerPrefs.GetInt("hasCompletedTutorial", 0) == 1;
        rightKey.SetActive(!hasCompletedTutorial);
        ctrlKey.SetActive(false);
        spaceKey.SetActive(false);

        rightKeyAnimator = rightKey.GetComponent<Animator>();
        ctrlKeyAnimator = ctrlKey.GetComponent<Animator>();
        spaceKeyAnimator = spaceKey.GetComponent<Animator>();
        defaultPosition = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        if (rightKey.activeSelf)
        {
            if (Input.GetAxis("Horizontal") != defaultPosition)
            {
                rightKeyAnimator.SetBool("Pressed", true);
            }
            else if (Input.GetAxis("Horizontal") == defaultPosition && rightKeyAnimator.GetBool("Pressed"))
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
                PlayerPrefs.SetInt("hasCompletedTutorial", 1);
                PlayerPrefs.Save();
                HideTutorial();
            }
        }
    }
    public void PlayerIsIdle()
    {
        Animator player = GameObject.Find("Knight").GetComponent<Animator>();
        AnimatorStateInfo stateInfo = player.GetCurrentAnimatorStateInfo(0);
        Debug.Log(stateInfo);
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