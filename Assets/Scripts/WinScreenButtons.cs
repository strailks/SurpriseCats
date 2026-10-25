using UnityEngine;

public class WinScreenButtons : MonoBehaviour
{
    public void MainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void Finish()
    {
        GameManager.Instance.ShowWinScreen();
    }
}