using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int cost = 10;
    public int costScaling = 2;
    public string itemName = "Click Power";
    public int value = 1;
    public int valueScaling = 1;
    public ShopOptions option;

    private Button button;
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI valueText;
    private GameManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(BuyItem);
        priceText = button.GetComponentInChildren<TextMeshProUGUI>();
        priceText.text = cost.ToString();
        valueText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        valueText.text = itemName + " " + value.ToString();
        manager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {
        if(manager.GetMoney() >= cost)
        {
            if (manager.BuyItem(cost, this))
            {
                cost *= costScaling;
                UpdateCost();
                UpdateValue();
            }
        }
    }

    private void UpdateCost()
    {
        priceText.text = cost.ToString();
    }

    private void UpdateValue()
    {
        valueText.text = itemName + " " + value.ToString();
    }
}
