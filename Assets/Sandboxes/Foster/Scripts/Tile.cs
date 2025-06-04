using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color normal, offset;
    public GameObject mouseOverObject;
    public GameObject clickedObject;
    public SpriteRenderer renderer;
    public bool clicked;
    private bool hasBeenShot;
    public bool strong;
    public bool isPlayer;
    public bool isGoal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start(){
        hasBeenShot = false;
    }

    public void setPlayer(){
        changeColor(new Color(0f,1f,0f));
        isPlayer = true;
    }
    public void notPlayer(){
        isPlayer = false;
    }

    public void setGoal(){
        changeColor(new Color(0f,0f,1f));
        isGoal = true;
    }

    public bool isClicked(){
        return clicked;
    }

    public void setClicked(){
        clicked = true;
    Debug.Log($"{gameObject.name}: setClicked called — clicked = {clicked}");

    }

    public void unClicked(){
        clicked = false;
    }

    public bool isStrong(){
        return strong;
    }

    public void makeStrong(){
        strong = true;
    }

    public void makeWeak(){
        strong = false;
    }

    public bool isShot(){
        return hasBeenShot;
    }

    public void Shot(){
        hasBeenShot = true;
    }

    public void notShot(){
        hasBeenShot = false;
    }

    public void changeColor(Color color){
        renderer.color = color;
    }
    
    public void Generate(bool hasOffset)
    {
        renderer.color = hasOffset ? offset : normal;
    }

    void OnMouseEnter(){
        mouseOverObject.SetActive(true);
    }

    void OnMouseExit(){
        mouseOverObject.SetActive(false);
    }

    void OnMouseDown(){
            if(isPlayer){
                return;
            }
           if (strong){
                return;
           }
            else if(!clicked){
                    changeColor(new Color(0.25f,0.25f,0.25f));
                    clicked = true;
            }
            else{
                changeColor(new Color(0f,0f,0f));
                strong = true;
            }

                

    }
}
