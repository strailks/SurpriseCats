using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.Json;

public class SaveScript : MonoBehaviour
{
    public List<int> resources = new List<int> { -1, -1, -1 };
    public List<List<int>> values = new List<List<int>> { new List<int> { -1, -1, -1, -1 }, new List<int> { -1, -1, -1, -1 }, new List<int> { -1, -1, -1, -1 } };
    public List<List<int>> prices = new List<List<int>> { new List<int>{ -1, -1, -1, -1 }, new List<int> { -1, -1, -1, -1 }, new List<int> { -1, -1, -1, -1 } };
    public List<bool> autoclickers = new List<bool> { false, false, false };

    public static SaveScript Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate
            return;
        }

        // Set the active instance and protect it from destruction
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadFromFile()
    {

    }

    public void SaveToFile()
    {
        //var combinedData = new { resources, values, prices, autoclickers};
        //string jsonString = JsonSerializer.Serialize(combinedData, new JsonSerializerOptions { WriteIndented = true});
    }

    public void SetAllValues()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameManager _instance = GameManager.Instance;
            if(resources[0] != -1)
            {
                _instance.trees = resources[0];
            }
            if(resources[1] != -1)
            {
                _instance.paper = resources[1];
            }
            if(resources[2] != -1)
            {
                _instance.cards = resources[2];
            }
            for (int i = 0; i < 3; i++)
            {
                ShopItem[] children = _instance.canvases[i].GetComponentsInChildren<ShopItem>();
                for (int x = 0; x < 4; x++)
                {
                    if (prices[i][x] != -1)
                    {
                        children[x].StartupCost(prices[i][x]);
                    }
                    if (values[i][x] != -1)
                    {
                        children[x].StartupValue(values[i][x]);
                    }
                }
            }
            _instance.autoClicker.isTreeOn = autoclickers[0];
            _instance.autoClicker.isPaperOn = autoclickers[1];
            _instance.autoClicker.isCardOn = autoclickers[2];
        }
    }

    public void GrabAllValues()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameManager _instance = GameManager.Instance;
            resources[0] = _instance.trees;
            resources[1] = _instance.paper;
            resources[2] = _instance.cards;

            for (int i = 0; i < 3; i++)
            {
                ShopItem[] children = _instance.canvases[i].GetComponentsInChildren<ShopItem>();
                for(int x = 0; x < 4; x++)
                {
                    prices[i][x] = children[x].cost;
                    values[i][x] = children[x].value;
                }
            }
            autoclickers[0] = _instance.autoClicker.isTreeOn;
            autoclickers[1] = _instance.autoClicker.isPaperOn;
            autoclickers[2] = _instance.autoClicker.isCardOn;
        }
    }
}

public class SaveData
{

}