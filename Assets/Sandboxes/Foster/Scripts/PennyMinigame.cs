using UnityEngine;
using UnityEngine.UI;

public class PennyMinigame : MonoBehaviour
{

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
    private bool checkWin;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkWin = false;

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

    void reset(){
        for(int i = 0; i < 12; i++){
            slots[i].resetAll();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(checkWin){

        }
        
        if(Input.GetKeyDown(KeyCode.X)){
            reset();
        }


    }

private void searchRight(Slots slot, bool high){
        //search right
        int nextSlot = slot.getSlot() + 1;
        int buttonsJumped = 0;
        int count = 0;
        while(buttonsJumped != 2){
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
                if(buttonsJumped == 2){
                    break;
                }
                if(slotToCheck.isStacked()){
                    buttonsJumped ++;
                    if(buttonsJumped == 2){
                        break;
                    }
                }
            }
            nextSlot ++;
        }

        int currentSlot = nextSlot;

        if (!slots[currentSlot].isStacked()){
            slots[currentSlot].highlightHigh();
        }
        else{
            slots[currentSlot].highlightLow();
        }

    }

   private void searchLeft(Slots slot, bool high){
        //search right
        int nextSlot = slot.getSlot() - 1;
        int buttonsJumped = 0;
        int count = 0;
        while(buttonsJumped != 2){
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
                if(buttonsJumped == 2){
                    break;
                }
                if(slotToCheck.isStacked()){
                    buttonsJumped ++;
                    if(buttonsJumped == 2){
                        break;
                    }
                }
            }
            nextSlot --;
        }

        int currentSlot = nextSlot;

        if (!slots[currentSlot].isStacked()){
            slots[currentSlot].highlightHigh();
        }
        else{
            slots[currentSlot].highlightLow();
        }

    }

    public void ButtonClicked(Button button){

        for(int i = 0; i < 12; i++){
            if(slots[i].getHighlightedLow()){
                    slots[i].resetButton1();
                }
                if(slots[i].getHighlightedHigh()){
                    slots[i].resetButton2();
                }
        }

        ButtonMapping buttonMap = button.GetComponent<ButtonMapping>();
        bool high = buttonMap.high;
        Slots slot = slots[buttonMap.index];
        if(slot.isOn() && !high){
            searchLeft(slot, high);
            searchRight(slot, high);
        }
        else if(slot.isStacked() && high){
            searchLeft(slot, high);
            searchRight(slot, high);
        }
    }

}
