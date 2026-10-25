using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CardProgressBar : MonoBehaviour
{
    [SerializeField] private Image Fill;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Button endButton;
    [SerializeField] private int goal = 10000;
    [SerializeField] private float smoothSpeed = 5f;

    void Start()
    {
        endButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        int total = GameManager.Instance.totalCardsCreated;
        float target = Mathf.Clamp01((float)total / goal);
        
        Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, target, Time.deltaTime * smoothSpeed);

        if (total >= goal)
        {
            percentText.text = "FINISH!";
            endButton.interactable = true;
        }
        else
        {
            //percentText.text = (target * 100f).ToString("F1") + "%";
            percentText.text = total.ToString() + " out of 10,000 cards"; 
        }
    }
}
