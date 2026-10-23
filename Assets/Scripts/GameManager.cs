using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps it alive across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroys duplicates that enter the scene
        }
    }
    #endregion

    public GameObject treeText; 
    public int money = 0;

    private int clickPower = 1;
    private AutoClicker autoClicker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMoney();
        autoClicker = GetComponent<AutoClicker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void AddTreesChopped(int _trees)
    //{
    //    money += _trees;
    //    UpdateMoney();
    //}

    public void ChopTree()
    {
        money += clickPower;
        UpdateMoney();
    }

    public int GetMoney()
    {
        return money;
    }

    public bool BuyItem(int price, ShopItem item)
    {
        if(price <= money)
        {
            money -= price;
            UpdateMoney();
            item.value += item.valueScaling;
            switch (item.option)
            {
                case ShopOptions.ClickPower:
                    clickPower = item.value;
                    break;
                case ShopOptions.Autoclick:
                    if(!autoClicker.isOn)
                    {
                        autoClicker.StartAutoClicker();
                    }
                    autoClicker.timeBetweenClicks = item.value;
                    break;
                default:
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
        return false;
    }

    private void UpdateMoney()
    {
        treeText.GetComponent<TextMeshProUGUI>().text = money.ToString();
    }

    public int GetClickPower()
    {
        return clickPower;
    }
}
