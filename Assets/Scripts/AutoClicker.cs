using UnityEngine;

public class AutoClicker : MonoBehaviour

    // Resources active?
{
    public bool isTreeOn = false;
    public bool isPaperOn = false;
    public bool isCardOn = false;

    // Time between clicks in milliseconds

    public int timeBetweenTreeClicks = 100;
    public int timeBetweenPaperClicks = 100;
    public int timeBetweenCardClicks = 100;

    // Last click time

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

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.

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

    // Start the auto clicker for a specific resource

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

    // Stop the auto clicker for a specific resource

    public bool IsResourceOn(Resource resouce)
    {
        switch(resouce)
        {
            case Resource.Wood:
                return isTreeOn;
            case Resource.Paper:
                return isPaperOn;
            case Resource.Card:
                return isCardOn;
            default:
                return false;
        }
    }

    // Stop the auto clicker for a specific resource
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
