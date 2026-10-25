using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine.UI;

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
    public int totalCardsCreated;

    public int treeConversion = 10;
    public int paperConversion = 10;

    public Resource activePanel = Resource.Wood;
    public GameObject[] canvases;

    private int clickPower = 1;
    private List<int> clickPowers = new List<int> { 1, 1, 1 };
    public int autoClickPower = 1;
    private List<int> autoClickPowers = new List<int> { 1, 1, 1 };
    public AutoClicker autoClicker;
    private int totalLumber = 0;

    public bool isPaused = false;
    public GameObject pauseCanvas;

    public GameObject clawMarks;
    public float clawMarksDuration = 2f;

    private Animator ClawMarksAnimator;
    private Coroutine clawMarksCoroutine;

    public AudioManager audioManager;

    public GameObject winCanvas;
    public TextMeshProUGUI winTimeText;
    public TextMeshProUGUI winMessageText;
    public AudioSource winAudioSource;
    public AudioClip winClip;
    [TextArea] public string winMessage = "The forest is gone, but that is of no concern to you. You have what is truly important in life. 10,000 Cat TCG cards!";
    private float playTime = 0f;
    private bool hasWon = false;
    public Tutorial tut;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTrees();
        autoClicker = GetComponent<AutoClicker>();
        tut = gameObject.GetComponent<Tutorial>();

        if (clawMarks != null)
        {
            ClawMarksAnimator = clawMarks.GetComponent<Animator>();
            clawMarks.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWon)
        {
            return;
        }

        if (!isPaused)
        { 
            playTime += Time.deltaTime;
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ChangePauseState();
        }
    }

    public void ChopTree()
    {

        //trees += clickPower;
        trees += clickPowers[0];
        UpdateTrees();
        //totalLumber += clickPower;
        totalLumber += clickPowers[0];
        UpdateStats();

        PlayClawMarks();

        if(tut.tutorialStep == 1)
        {
            tut.StepOne();
        }
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
        trees += autoClickPowers[0];
        UpdateTrees();

        totalLumber += autoClickPowers[0];
        UpdateStats();
    }

    public void UpgradeAutoClickPower(ShopItem _item)
    {
        autoClickPowers[(int)_item.resource] += _item.value;
    }

    public void MakePaper()
    {
        if(trees >= treeConversion)
        {
            paper+= clickPowers[1];
            trees -= treeConversion;
            UpdateTrees();
            UpdatePaper();
        }
    }

    public void AutoMakePaper()
    {
        if (trees >= treeConversion)
        {
            paper += autoClickPowers[1];
            trees -= treeConversion;
            UpdateTrees();
            UpdatePaper();
        }
    }

    public void AutoMakeCard()
    {
        if (paper >= paperConversion)
        {
            cards += autoClickPowers[2];
            totalCardsCreated += autoClickPowers[2];
            paper -= paperConversion;
            UpdatePaper();
            UpdateCards();
        }
    }

    public void MakeCard()
    {
        if (paper >= paperConversion)
        {
            cards += clickPowers[2];
            totalCardsCreated += clickPowers[2];
            paper -= paperConversion;
            UpdatePaper();
            UpdateCards();
            if (tut.tutorialStep == 6)
            {
                tut.StepSix();
            }
        }
    }

    public int GetMoney(Resource resource)
    {
        switch(resource)
        {
            case Resource.Wood:
                return trees;
            case Resource.Paper:
                return paper;
            case Resource.Card:
                return cards;
            default:
                return -1;
        }
    }

    public bool BuyItem(int price, ShopItem item)
    {
        if (item.option == ShopOptions.Autoclick &&
            item.value + item.valueScaling <1)
        {
            return false;
        }
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
                    paper -= price;
                    UpdatePaper();
                    break;
                case Resource.Card:
                    cards -= price;
                    UpdateCards();
                    break;
                default:
                    break;
            }
            item.value += item.valueScaling;
            switch (item.option)
            {
                case ShopOptions.ClickPower:
                    clickPowers[(int)item.resource] = item.value;
                    break;

                case ShopOptions.Autoclick:
                    if(!autoClicker.IsResourceOn(item.resource))
                    {
                        autoClicker.StartAutoClicker(item.resource);
                    }
                    autoClicker.SetTimeBetween(item.resource, item.value);
                    if(item.value == 1)
                    {
                        item.GetComponentInChildren<Button>().interactable = false;
                    }
                    break;

                case ShopOptions.Cheaper:

                    foreach(ShopItem shopItem in canvases[(int)item.resource].GetComponentsInChildren<ShopItem>())
                    {
                        shopItem.cost = Mathf.Max(1, (int)(shopItem.cost * ((10 - item.value) / 10.0)));
                        shopItem.UpdateValue();
                        Debug.Log("frown");
                    }
                    break;

                case ShopOptions.AutoclickPower:
                    UpgradeAutoClickPower(item);
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
        if(tut.tutorialStep == 3)
        {
            tut.StepThree();
        }
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
        return clickPowers[0];
    }

    public void ChangeActiveCanvas(int resource)
    {
        canvases[(int)activePanel].SetActive(false);
        canvases[resource].SetActive(true);
        activePanel = (Resource)resource;

        if(GetComponent<Tutorial>().tutorialStep == 4)
        {
            GetComponent<Tutorial>().StepFour();
        }
    }

    private void UpdateStats()
    {
        statTexts[0].text = (totalLumber / 10).ToString();
        statTexts[1].text = (totalLumber / 10000).ToString();
    }

    private string ToScience(int num)
    {
        return num.ToString();
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ShowWinScreen()
    {
        if (hasWon) return;
        hasWon = true;

        pauseCanvas.SetActive(false);
        winCanvas.SetActive(true);

        int total = Mathf.FloorToInt(playTime);
        winTimeText.text = $"Time: {total / 3600:00}:{(total % 3600) / 60:00}:{total % 60:00}";
        winMessageText.text = winMessage;

        if (winAudioSource != null && winClip != null)
            winAudioSource.PlayOneShot(winClip);

        Time.timeScale = 0f;
    }

}

public enum Resource
{
    Wood, Paper, Card
}