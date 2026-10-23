using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    public bool isOn = false;

    public int timeBetweenClicks = 100;

    private float lastClick;
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
        if(isOn)
        {
            if(Time.time - lastClick >= timeBetweenClicks)
            {
                manager.ChopTree();
                lastClick = Time.time;
            }
        }
    }

    public void StartAutoClicker()
    {
        isOn = true;
        lastClick = Time.time;
    }
}
