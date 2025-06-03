using UnityEngine;

public class Tile : MonoBehaviour
{

    public Color normal, offset;
    public GameObject mouseOverObject;
    public GameObject clickedObject;
    public SpriteRenderer renderer;
    private bool clicked;
    private bool hasBeenShot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start(){
        clicked = false;
        hasBeenShot = false;
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

            renderer.color = new Color(1f,1f,0f);
            clicked = true;

    }
}
