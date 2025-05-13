using UnityEngine;
using UnityEngine.UI;

public class Slots
{
    private Button button1;
    private Button button2;
    private Image battery;
    private int slot;
    private bool stacked;
    private bool on;
    private bool highlighetedLow;
    private bool highlighetedHigh;
    
    public Slots(Button button1, Button button2, Image battery, int slot){
        this.button1 = button1;
        this.button2 = button2;
        this.battery = battery;
        this.slot = slot;
        this.stacked = false;
        this.on = true;
        this.highlighetedLow = false;
        this.highlighetedHigh = false;

    }

    public int getSlot(){
        return slot;
    }

    public Image getBattery(){
        return battery;
    }

    public Button getButton1(){
        return button1;
    }

    public bool getHighlightedLow(){
        return highlighetedLow;
    }

    public bool getHighlightedHigh(){
        return highlighetedHigh;
    }

    public Button getButton2(){
        return button2;
    }

    public void resetButton1(){
        button1.GetComponent<Image>().color = new Color (0.0f, 1.0f, 0.0f);
    }

    public void resetButton2(){
        button2.GetComponent<Image>().color = new Color (1f, 1f, 1f);
    }

    public void resetBattery(){
        battery.GetComponent<Image>().color = new Color (1f, 1f, 1f);
    }

    public void stack(){
        stacked = true;
        button2.GetComponent<Image>().color = new Color (0.25f, 1f, 0.25f);

    }

    public void unStack(){
        stacked = false;
        button2.GetComponent<Image>().color = new Color (1.0f, 1.0f, 1.0f);

    }

    public void turnOn(){
        on = true;
        button1.GetComponent<Image>().color = new Color (0.25f, 1f, 0.25f);

    }

    public void turnOff(){
        on = false;
        button1.GetComponent<Image>().color = new Color (1.0f, 1.0f, 1.0f);

    }

    public void resetAll(){
        resetButton1();
        resetButton2();
        resetBattery();
        unStack();
    }

    public bool isStacked(){
        return stacked;
    }

    public bool isOn(){
        return on;
    }

    public void highlightLow(){
        highlighetedLow = true;
        button1.GetComponent<Image>().color = new Color (1.0f, 1.0f, 0.0f);
    }

    public void highlightHigh(){
        highlighetedHigh = true;
        button2.GetComponent<Image>().color = new Color (1.0f, 1.0f, 0.0f);
    }

    public void unHighlightHigh(){
        highlighetedHigh = false;
    }

    public void unHighlightLow(){
        highlighetedLow = false;
    }

}
