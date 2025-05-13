using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PennyMinigame : MonoBehaviour
{

    //text
    public TextMeshProUGUI counterText;

    //ints
    private int textCounterNum = 6;

    //inner Buttons
    public Button inner1;
    public Button inner2;
    public Button inner3;
    public Button inner4;
    public Button inner5;
    public Button inner6;
    public Button inner7;
    public Button inner8;
    public Button inner9;
    public Button inner10;
    public Button inner11;
    public Button inner12;

    //outer Buttons
    public Button outer1;
    public Button outer2;
    public Button outer3;
    public Button outer4;
    public Button outer5;
    public Button outer6;
    public Button outer7;
    public Button outer8;
    public Button outer9;
    public Button outer10;
    public Button outer11;
    public Button outer12;

    //other buttons
    public Button scramble;
    private Button selectedButton;

    //batteries 
    public Image battery1;
    public Image battery2;
    public Image battery3;
    public Image battery4;
    public Image battery5;
    public Image battery6;
    public Image battery7;
    public Image battery8;
    public Image battery9;
    public Image battery10;
    public Image battery11;
    public Image battery12;

    //Arrays
    private Button[] outerArray;
    private Button[] innerArray;
    private Image[] batteries;
    private Slots[] slots;


    //bools
    private bool buttonsChanged;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scramble.interactable = false;
        string counterString = textCounterNum.ToString();
        counterText.text = counterString;

        outerArray = new Button[] {outer1, outer2, outer3, outer4, outer5, outer6, outer7, outer8, outer9, outer10, outer11, outer12};
        innerArray = new Button[] {inner1, inner2, inner3, inner4, inner5, inner6, inner7, inner8, inner9, inner10, inner11, inner12};
        batteries = new Image[] {battery1, battery2, battery3, battery4, battery5, battery6, battery7, battery8, battery9, battery10, battery11, battery12};

        slots = new Slots[12];

        for(int i = 0; i < 12; i++){
            slots[i] = new Slots(innerArray[i], outerArray[i], batteries[i], i);
            var buttonMapperOuter = outerArray[i].gameObject.AddComponent<ButtonMapping>();
            var buttonMapperInner = innerArray[i].gameObject.AddComponent<ButtonMapping>();
            buttonMapperInner.index = i;
            buttonMapperOuter.index = i;
            buttonMapperInner.high = false;
            buttonMapperOuter.high = true;


        }

        reset();
        
    }

    private void checkBatteries(){
        int j = 0;
        int p = 6;
        for(int i = 6; i < 12; i++){
            if(slots[i].isOn()){
                batteries[j].color = new Color (0.0f, 1.0f, 0.0f);
            }
            if(slots[i].isStacked()){
                batteries[p].color = new Color (0.0f, 1.0f, 0.0f);
            }
            j = j + 1;
            p = p + 1;
        }
    }

    void reset(){
        for(int i = 0; i < 12; i++){
            slots[i].resetAll();
        }
            textCounterNum = 6;
            string counterString = textCounterNum.ToString();
            counterText.text = counterString;
    }



    // Update is called once per frame
    void Update()
    {   
        checkBatteries();
        
        if(Input.GetKeyDown(KeyCode.X)){
            reset();
        }


    }

private void resetHighlights(){
    for(int i = 0; i < 12; i++){
        if(slots[i].getHighlightedLow()){
                slots[i].resetButton1();
                slots[i].unHighlightLow();
            }
            if(slots[i].getHighlightedHigh()){
                slots[i].unHighlightHigh();
                slots[i].resetButton2();
            }
    }
}

private void searchRight(Slots slot, bool high){
        //search right
        int nextSlot = slot.getSlot() + 1;
        int buttonsJumped = 0;
        int count = 0;
        while(buttonsJumped != 3){
            //debug info
            if (count > 100){
                Debug.Log("Error in the buttons jumped- looped too much");
            }
            
            //makes slots modulo
            if (nextSlot >= 12){
                nextSlot = 0;
            }

            Slots slotToCheck = slots[nextSlot];

            if(slotToCheck.isOn()){
                buttonsJumped ++;
                if(buttonsJumped == 3){
                    break;
                }
                if(slotToCheck.isStacked()){
                    buttonsJumped ++;
                    if(buttonsJumped == 3){
                        break;
                    }
                }
            }
            nextSlot ++;
        }

        int currentSlot = nextSlot;

        if (!slots[currentSlot].isStacked() && slots[currentSlot].isOn()){
            slots[currentSlot].highlightHigh();
        }
        else if(!slots[currentSlot].isOn() && !slots[currentSlot].isStacked()){
            slots[currentSlot].highlightLow();
        }

    }

   private void searchLeft(Slots slot, bool high){
        //search right
        int nextSlot = slot.getSlot() - 1;
        int buttonsJumped = 0;
        int count = 0;
        while(buttonsJumped != 3){
            //debug info
            if (count > 100){
                Debug.Log("Error in the buttons jumped- looped too much");
            }
            
            //makes slots modulo
            if (nextSlot < 0){
                nextSlot = 11;
            }

            Slots slotToCheck = slots[nextSlot];

            if(slotToCheck.isOn()){
                buttonsJumped ++;
                if(buttonsJumped == 3){
                    break;
                }
                if(slotToCheck.isStacked()){
                    buttonsJumped ++;
                    if(buttonsJumped == 3){
                        break;
                    }
                }
            }
            nextSlot --;
        }

        int currentSlot = nextSlot;

        if (!slots[currentSlot].isStacked() && slots[currentSlot].isOn()){
            slots[currentSlot].highlightHigh();
        }
        else if(!slots[currentSlot].isOn() && !slots[currentSlot].isStacked()){
            slots[currentSlot].highlightLow();
        }

    }


    public bool incremenetCounter(){
        if (textCounterNum <= 1 && !checkForWin()){
            textCounterNum = 6;
            string counterString = textCounterNum.ToString();
            counterText.text = counterString;
            reset();
            return true;
        }
        else{

            textCounterNum --;
            string counterString = textCounterNum.ToString();
            counterText.text = counterString;
            return false;
        }
    }

    private bool checkForWin(){
        int winningCounter = 0;
        for(int i = 6; i < 12; i++){
            if(slots[i].isOn() && slots[i].isStacked()){
                winningCounter ++ ;
            }
        }

        if (winningCounter >= 6){
            return true;
        }
        else{
            return false;
        }
    }

    private void win(bool checkWin){
        if(checkWin){
            scramble.GetComponent<Image>().color = new Color (0, 255, 0);
            scramble.interactable = true;
        }
    }

    public void ButtonClicked(Button button){

        Debug.Log(selectedButton);

        ButtonMapping buttonMap = button.GetComponent<ButtonMapping>();
        bool high = buttonMap.high;
        Slots slot = slots[buttonMap.index];
        bool actionDone = false;


        if(slot.getHighlightedHigh() && high){
            slot.stack();
            actionDone = true;
            slot.unHighlightHigh();
            win(checkForWin());
            if(incremenetCounter()){
                return;
            }

        }
        else if(slot.getHighlightedLow() && !high){
            slot.turnOn();
            slot.unHighlightLow();
            actionDone = true;
            win(checkForWin());
            if (incremenetCounter()){
                return;
            }

        }

        if(selectedButton != null){

            ButtonMapping selectedButtonMap = selectedButton.GetComponent<ButtonMapping>();
            bool selectedHigh = selectedButtonMap.high;
            Slots selectedSlot = slots[selectedButtonMap.index];    

            if(actionDone){
                if(selectedHigh){
                    selectedSlot.unStack();
                }
                else{
                    selectedSlot.turnOff();
                }
            }

        }

        resetHighlights();


        if(!actionDone){
            resetHighlights();
            if(slot.isOn() && !high && !slot.isStacked()){
                searchLeft(slot, high);
                searchRight(slot, high);
            }
            else if(slot.isStacked() && high){
                searchLeft(slot, high);
                searchRight(slot, high);
            }
        }


        selectedButton = button;
        for(int i = 0; i < 12; i++){
            Debug.Log($"Slot: {i}, \t is highlighted high: {slots[i].getHighlightedHigh()} \t is highlighted low: {slots[i].getHighlightedLow()}");
        }
    }

}
