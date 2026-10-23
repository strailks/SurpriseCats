using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Instance
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

    public GameObject treeText, paperText, cardText;
    public int trees = 0;
    public int paper = 0;
    public int cards = 0;

    public int treeConversion = 10;
    public int paperConversion = 10;

    public Resource activePanel = Resource.Wood;
    public GameObject[] canvases;

    private int clickPower = 1;
    private AutoClicker autoClicker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTrees();
        autoClicker = GetComponent<AutoClicker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChopTree()
    {
        trees += clickPower;
        UpdateTrees();
    }

    public void MakePaper()
    {
        if(trees >= treeConversion)
        {
            paper++;
            trees -= treeConversion;
            UpdateTrees();
            UpdatePaper();
        }
    }

    public void MakeCard()
    {
        if (paper >= paperConversion)
        {
            cards++;
            paper -= paperConversion;
            UpdatePaper();
            UpdateCards();
        }
    }

    public int GetMoney()
    {
        return trees;
    }

    public bool BuyItem(int price, ShopItem item)
    {
        int _compValue = (item.resource == Resource.Wood ? trees : (item.resource == Resource.Paper ? paper : cards));
        if(price <= _compValue)
        {
            switch (item.resource)  
            {
                case Resource.Wood:
                    trees -= price;
                    break;
                case Resource.Paper:
                    paper -= price;
                    break;
                case Resource.Card:
                    cards -= price;
                    break;
                default:
                    break;
            }
            UpdateTrees();
            item.value += item.valueScaling;
            switch (item.option)
            {
                case ShopOptions.ClickPower:
                    clickPower = item.value;
                    break;
                case ShopOptions.Autoclick:
                    if(!autoClicker.IsResourceOn(item.resource))
                    {
                        autoClicker.StartAutoClicker(item.resource);
                    }
                    autoClicker.SetTimeBetween(item.resource, item.value);
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

    private void UpdateTrees()
    {
        treeText.GetComponent<TextMeshProUGUI>().text = trees.ToString();
    }

    private void UpdatePaper()
    {
        paperText.GetComponent<TextMeshProUGUI>().text = paper.ToString();
    }

    private void UpdateCards()
    {
        cardText.GetComponent<TextMeshProUGUI>().text = cards.ToString();
    }

    public int GetClickPower()
    {
        return clickPower;
    }

    public void ChangeActiveCanvas(int resource)
    {
        canvases[(int)activePanel].SetActive(false);
        canvases[resource].SetActive(true);
        activePanel = (Resource)resource;
    }
}

public enum Resource
{
    Wood, Paper, Card
}