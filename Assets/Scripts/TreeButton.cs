using UnityEngine;

public class TreeButton : MonoBehaviour
{
    GameManager manager;

    public Resource resource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChopTree()
    {
        switch (resource)
        {
            case Resource.Wood:
                manager.ChopTree();
                break;
            case Resource.Paper:
                manager.MakePaper();
                break;
            case Resource.Card:
                manager.MakeCard();
                break;
            default:
                break;
        }
        //manager.AddTreesChopped(manager.GetClickPower());
    }
}
