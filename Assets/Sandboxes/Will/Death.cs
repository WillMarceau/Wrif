using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    // Update is called once per frame

    public void Die()
    {
        // for now just reload the scene
        StartCoroutine(Dying());

    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        SceneManager.LoadScene("MainScene");
    }
}
