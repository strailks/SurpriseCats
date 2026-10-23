using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    public bool isTreeOn = false;
    public bool isPaperOn = false;
    public bool isCardOn = false;

    public int timeBetweenTreeClicks = 100;
    public int timeBetweenPaperClicks = 100;
    public int timeBetweenCardClicks = 100;

    private float lastTreeClick;
    private float lastPaperClick;
    private float lastCardClick;
    private GameManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isTreeOn)
        {
            if(Time.time - lastTreeClick >= timeBetweenTreeClicks)
            {
                manager.AutoChopTree();
                lastTreeClick = Time.time;
            }
        }
        if (isPaperOn)
        {
            if (Time.time - lastPaperClick >= timeBetweenPaperClicks)
            {
                manager.MakePaper();
                lastPaperClick = Time.time;
            }
        }
        if (isCardOn)
        {
            if (Time.time - lastCardClick >= timeBetweenCardClicks)
            {
                manager.MakeCard();
                lastCardClick = Time.time;
            }
        }
    }

    public void StartAutoClicker(Resource resource)
    {
        switch (resource)
        {
            case Resource.Wood:
                isTreeOn = true;
                lastTreeClick = Time.time;
                break;
            case Resource.Paper:
                isPaperOn = true;
                lastPaperClick = Time.time;
                break;
            case Resource.Card:
                isCardOn = true;
                lastCardClick = Time.time;
                break;
            default:
                break;
        }
    }

    public bool IsResourceOn(Resource resouce)
    {
        switch(resouce)
        {
            case Resource.Wood:
                return isTreeOn;
                break;
            case Resource.Paper:
                return isPaperOn;
                break;
            case Resource.Card:
                return isCardOn;
                break;
            default:
                return false;
                break;
        }
    }

    public void SetTimeBetween(Resource resource, int timeBetween)
    {
        switch (resource)
        {
            case Resource.Wood:
                timeBetweenTreeClicks = timeBetween;
                break;
            case Resource.Paper:
                timeBetweenPaperClicks = timeBetween;
                break;
            case Resource.Card:
                timeBetweenCardClicks = timeBetween;
                break;
            default:
                break;
        }
    }
}
