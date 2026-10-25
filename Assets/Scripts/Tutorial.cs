using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public int tutorialStep = 1;
    public GameObject[] tutorialPanels;

    private int stepOneCounter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StepOne()
    {
        stepOneCounter++;
        tutorialPanels[0].GetComponentsInChildren<TextMeshProUGUI>()[1].text = stepOneCounter.ToString() + "/10";
        if(stepOneCounter >= 10)
        {
            NextStep();
        }
    }

    public void StepTwo()
    {
        NextStep();
    }

    public void StepThree()
    {
        if (tutorialStep == 3)
        {
            if (GameManager.Instance.trees >= 10)
            {
                NextStep();
            }
            else
            {
                tutorialPanels[2].GetComponentsInChildren<TextMeshProUGUI>()[1].text = GameManager.Instance.trees.ToString() + "/10";
            }
        }
    }

    public void StepFour()
    {
        NextStep();
    }

    public void StepFive()
    {
        NextStep();
    }

    public void StepSix()
    {
        NextStep();
    }

    private void NextStep()
    {
        tutorialPanels[tutorialStep - 1].SetActive(false);
        if (tutorialStep < tutorialPanels.Length)
        {
            tutorialPanels[tutorialStep].SetActive(true);
        }
        tutorialStep++;
    }
}
