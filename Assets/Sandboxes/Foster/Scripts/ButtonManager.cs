using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Image leftLight;
    public Image centerLight;
    public Image rightLight;
    public Button KeyCodeButton;

    public GameObject NorthWall;
    public GameObject WestWall;
    public GameObject SouthWall;


    public float timer;
    private float nextReset = 0.0f;

    private bool leftOn;
    private bool centerOn;
    private bool rightOn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void KeyButton()
    {
        NorthWall.SetActive(false);
        WestWall.SetActive(false);
        SouthWall.SetActive(false);
    }
    public void left()
    {
        leftLight.color = new Color (0, 255, 0);
        leftOn = true;
    }

    public void center()
    {
        centerLight.color = new Color (0, 255, 0);
        centerOn = true;

    }

    public void right()
    {
        rightLight.color = new Color (0, 255, 0);
        rightOn = true;

    }

    private bool isSolved()
    {
        if(leftOn && rightOn && centerOn){
            return true;
        }
        else{
            return false;
        }
    }

    void Start(){
        KeyCodeButton.interactable = false;

    }

    void Update()

    {
        if(isSolved())
        {
            KeyCodeButton.GetComponent<Image>().color = new Color (0, 255, 0);
            KeyCodeButton.interactable = true;


        }
        if(Time.time > nextReset){
            nextReset = Time.time + timer;
            leftLight.color = new Color (255, 255, 255);
            centerLight.color = new Color (255, 255, 255);
            rightLight.color = new Color (255, 255, 255);
            KeyCodeButton.GetComponent<Image>().color = new Color (255, 255, 255);
            leftOn = false;
            centerOn = false;
            rightOn = false;
            KeyCodeButton.interactable = false;


        }
    }


}
