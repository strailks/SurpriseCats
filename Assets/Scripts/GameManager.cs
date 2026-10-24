using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI[] statTexts;
    public int trees = 0;
    public int paper = 0;
    public int cards = 0;

    public int treeConversion = 10;
    public int paperConversion = 10;

    public Resource activePanel = Resource.Wood;
    public GameObject[] canvases;

    private int clickPower = 1;
    public int autoClickPower = 1;
    private AutoClicker autoClicker;
    private int totalLumber = 0;

    public bool isPaused = false;
    public GameObject pauseCanvas;

    public GameObject clawMarks;
    public float clawMarksDuration = 0.5f;

    private Animator ClawMarksAnimator;
    private Coroutine clawMarksCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTrees();
        autoClicker = GetComponent<AutoClicker>();

        if (clawMarks != null)
        {
            ClawMarksAnimator = clawMarks.GetComponent<Animator>();
            clawMarks.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ChangePauseState();
        }
    }

    public void ChopTree()
    {

        trees += clickPower;
        UpdateTrees();
        totalLumber += clickPower;
        UpdateStats();

        PlayClawMarks();
    }

private void PlayClawMarks()
    {
        if (clawMarks == null || ClawMarksAnimator == null)
            return;

        if (clawMarksCoroutine != null)
        {
            StopCoroutine(clawMarksCoroutine);
            clawMarksCoroutine = null;
        }

        clawMarks.SetActive(true);
        ClawMarksAnimator.Play("Base Layer.ClawMarks", 0, 0f);
        ClawMarksAnimator.Update(0f);
        clawMarksCoroutine = StartCoroutine(HideClawMarks());
    }

    private IEnumerator HideClawMarks()
    {
        yield return new WaitForSeconds(clawMarksDuration);

        clawMarks.SetActive(false);
        clawMarksCoroutine = null;
    }
    public void AutoChopTree()
    {
        trees += autoClickPower;
        UpdateTrees();

        totalLumber += autoClickPower;
        UpdateStats();
    }

    public void UpgradeAutoClickPower(int amount)
    {
        autoClickPower += amount;
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
                    UpdateTrees();
                    break;
                case Resource.Paper:
                    UpdatePaper();
                    paper -= price;
                    break;
                case Resource.Card:
                    UpdateCards();
                    cards -= price;
                    break;
                default:
                    break;
            }
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
                case ShopOptions.Cheaper:

                    foreach(ShopItem shopItem in canvases[(int)item.resource].GetComponentsInChildren<ShopItem>())
                    {
                        shopItem.cost = Mathf.Max(1, (int)(shopItem.cost * ((10 - item.value) / 10.0)));
                        shopItem.UpdateCost();
                    }
                    break;

                case ShopOptions.AutoclickPower:
                    UpgradeAutoClickPower(item.valueScaling);
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
    }

    private void UpdateTrees()
    {
        treeText.GetComponent<TextMeshProUGUI>().text = ToScience(trees);
    }

    private void UpdatePaper()
    {
        paperText.GetComponent<TextMeshProUGUI>().text = ToScience(paper);
    }

    private void UpdateCards()
    {
        cardText.GetComponent<TextMeshProUGUI>().text = ToScience(cards);
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

    private void UpdateStats()
    {
        statTexts[0].text = (totalLumber / 10).ToString();
        statTexts[1].text = (totalLumber / 10000).ToString();
    }

    private string ToScience(int num)
    {
        if (num > 1000)
        {
            return num.ToString("E2");
        }
        else
        {
            return num.ToString();
        }
    }

    public void ChangePauseState()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}

public enum Resource
{
    Wood, Paper, Card
}