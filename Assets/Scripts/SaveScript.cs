using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour
{
    public List<int> resources = new List<int> { -1, -1, -1 };
    public List<int> clickPower = new List<int> { -1, -1, -1 };
    public List<int> autoClick = new List<int> { -1, -1, -1 };
    public List<int> woodPrices = new List<int> { -1, -1, -1 };
    public List<int> paperPrices = new List<int> { -1, -1, -1 };
    public List<int> cardPrices = new List<int> { -1, -1, -1 };


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
        }
    }
}
