using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCon : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player wins");
            SceneManager.LoadScene("WinScene");
        }
    }
}
