using UnityEngine;

public class TreeButton : MonoBehaviour
{
    GameManager manager;
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
        manager.ChopTree();
        //manager.AddTreesChopped(manager.GetClickPower());
    }
}
