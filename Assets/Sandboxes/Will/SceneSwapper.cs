using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public void LoadMaze()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadTut()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("WinScene");
    }

}
