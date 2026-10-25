using UnityEngine;

public class WinScreenButtons : MonoBehaviour
{
    public void MainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void Finish()
    {
        Debug.Log("Finish clicked");
        GameManager.Instance.ShowWinScreen();
        GameManager.Instance.audioManager.mainSong.source.Stop();
    }
}