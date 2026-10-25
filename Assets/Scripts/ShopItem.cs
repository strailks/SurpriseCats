using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int cost = 10;
    public float costScaling = 2;
    public string itemName = "Click Power";
    public string itemDesc = "Click Power";
    public int value = 1;
    public int valueScaling = 1;
    public ShopOptions option;
    public Resource resource;

    private Button button;
    //private TextMeshProUGUI priceText;
    private TextMeshProUGUI valueText;
    private GameManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(BuyItem);
        //priceText = button.GetComponentInChildren<TextMeshProUGUI>();
        //priceText.text = cost.ToString();
        GetComponentsInChildren<TextMeshProUGUI>()[0].text = itemName;
        GetComponentsInChildren<TextMeshProUGUI>()[1].text = itemDesc;
        valueText = GetComponentsInChildren<TextMeshProUGUI>()[2];
        //valueText.text = itemName + " " + value.ToString();
        manager = GameManager.Instance;
        UpdateValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Buy the item if the player has enough money
    public void BuyItem()
    {
        if(manager.GetMoney(resource) >= cost)
        {
            if (manager.BuyItem(cost, this))
            {
                cost = (int)(cost * costScaling);
                UpdateCost();
                UpdateValue();
            }
        }
    }

    public void StartupCost(int _cost)
    {
        cost = _cost;
        UpdateCost();
    }

    public void StartupValue(int _value)
    {
        value = _value;
        UpdateValue();
    }

    // Update the cost text
    public void UpdateCost()
    {
        //priceText.text = ToScience(cost);
        //priceText.text = "";
    }

    // Update the value text
    private void UpdateValue()
    {
        valueText.text = "Value: " + ToScience(value) + " Costs: " + ToScience(cost);
    }

    // Convert a number to scientific notation if it is greater than 1000
    private string ToScience(int num)
    {
        /*
        if (num > 1000)
        {
            return num.ToString("E2");
        }
        else
        {
            return num.ToString();
        }*/

        return num.ToString();
    }
}
