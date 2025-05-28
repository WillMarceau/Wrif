using UnityEngine;
using UnityEngine.UI;


public class BeepBoop : MonoBehaviour
{

    public Button BeepBoopButton;

    public GameObject ceiling;

    public void BeepBoopButtonClick()
        {
            ceiling.SetActive(false);

        }
}
