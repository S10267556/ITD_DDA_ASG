using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject startPage;
    public GameObject choosePetPage;
    public GameObject scanPage;
    public GameObject howToPlayPage;
    public GameObject leaderboardPage;

    void Start()
    {
        ShowStart();
    }

    void HideAll()
    {
        startPage.SetActive(false);
        choosePetPage.SetActive(false);
        scanPage.SetActive(false);
        howToPlayPage.SetActive(false);
        leaderboardPage.SetActive(false);
    }

    public void ShowStart()
    {
        HideAll();
        startPage.SetActive(true);
    }

    public void ShowChoosePet()
    {
        HideAll();
        choosePetPage.SetActive(true);
    }

    public void ShowScan()
    {
        HideAll();
        scanPage.SetActive(true);
    }

    public void ShowHowToPlay()
    {
        HideAll();
        howToPlayPage.SetActive(true);
    }

    public void ShowLeaderboard()
    {
        HideAll();
        leaderboardPage.SetActive(true);
    }
}
