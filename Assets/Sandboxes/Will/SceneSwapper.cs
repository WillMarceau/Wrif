using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public void LoadMaze()
    {
        SceneManager.LoadScene("MainScene");
    }

}
