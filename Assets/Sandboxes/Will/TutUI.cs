using UnityEngine;
using TMPro;

public class TutUI : MonoBehaviour
{
    // text box
    public TMP_Text tutorialText;

    // new text
    public string newText;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.text = newText;
        }

    }

}
