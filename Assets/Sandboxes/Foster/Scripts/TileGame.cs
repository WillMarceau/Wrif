using UnityEngine;

//Used this video for help: https://www.youtube.com/watch?v=kkAjpQAM-jE

public class TileGame : MonoBehaviour
{

    public float height;
    public float width;
    public Tile tile;
    public Transform camera;
    public GameObject canvas;
    private Tile[,] tileArray;
    public float timer;
    private float nextShot = 0.0f;
    private float nextReset = 0.0f;
    public float resetTimer;

    void Generate()
    {
        for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                    {
                        var spawnedTile = Instantiate(tile, new Vector3(x,y), Quaternion.identity);
                        spawnedTile.transform.SetParent(canvas.transform);
                        spawnedTile.name = $"Tile {x} {y}";

                        var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                        spawnedTile.Generate(offset);
                        tileArray[x, y] = spawnedTile;
                    }
            }

        camera.position = new Vector3((float)(width/2 - 0.5f), (float)(height/2 - 0.5f), -10);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileArray = new Tile[(int)width,(int)height];
        Generate();
        CreateObstacles();
    }

    Tile Access(int x, int y)
    {
        return tileArray[x,y];
    }

    void CreateObstacles(){
        for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                    {
                        if(Random.Range(0f,10f) > 8){
                            Access(x, y).changeColor(new Color(0f,0f,0f));
                        }

                    }
            }

    }

    void Destroy(int x, int y){
        var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        Access(x, y).Generate(offset);
    }

    void Shoot(int x, int y, string selection){
        int currentX = x;
        int currentY = y;

        if(selection == "left"){
            while(currentX < width){
                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentY < height - 1 && currentY > 0){
                    currentY += choices[choice];
                }
                Debug.Log(currentX);
                Debug.Log(currentY);
                Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                Access(currentX, currentY).Shot();
                currentX += 1;


            }
        }
        else if(selection == "right"){
            while(currentX >= 0){
                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentY < height - 1 && currentY > 0){
                    currentY += choices[choice];
                }
                Debug.Log(currentX);
                Debug.Log(currentY);
                Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                Access(currentX, currentY).Shot();
                currentX -= 1;


            }
        }
        else if (selection == "bottom"){
            while(currentY < height){
                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentX < width - 1 && currentX > 0){
                    currentX += choices[choice];
                }
                Debug.Log(currentX);
                Debug.Log(currentY);
                Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                Access(currentX, currentY).Shot();
                currentY += 1;


            }  
        }
        else if (selection == "top"){
            while(currentY >= 0){
                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentX < width - 1 && currentX > 0){
                    currentX += choices[choice];
                }
                Debug.Log(currentX);
                Debug.Log(currentY);
                Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                Access(currentX, currentY).Shot();
                currentY -= 1;


            }  
        }
    }

    void Cannon(){
            int cannonX = 0;
            int cannonY = 0;
            bool orientation = false;
            int choice = Random.Range(0,3);
            string[] options = {"bottom", "top", "left", "right"};
            string selection = options[choice];
            if(selection == "bottom"){
                cannonX = (int)Random.Range(0, width - 1);
                cannonY = 0;
                orientation = false;
            }
            else if(selection == "top"){
                cannonX = (int)Random.Range(0, width - 1);
                cannonY = (int)height - 1;
                orientation = false;
   
            }

            else if(selection == "left"){
                cannonX = 0;
                cannonY = (int)Random.Range(0, height - 1);
                orientation = true;

            }
            else if(selection == "right"){
                cannonX = (int)width - 1;
                cannonY = (int)Random.Range(0, height - 1);
                orientation = true;

            }

        Shoot(cannonX, cannonY, selection);
    }

    void clearBlasts(){
        for(int x = 0; x < width; x++)
                    {
                        for(int y = 0; y < height; y++)
                            {
                                if(Access(x, y).isShot()){
                                    Destroy(x,y);
                                    Access(x, y).notShot();
                                }

                            }
                    }
    }

    void Update(){
        if(Time.time > nextShot - resetTimer){
            clearBlasts();
        }
        if(Time.time > nextShot){
            nextShot = Time.time + timer;
            Cannon();
        }

    }

}
