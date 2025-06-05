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
    private int playerX;
    private int playerY;
    private int origPlayerX;
    private int origPlayerY;
    private int goalX;
    private int goalY;
    public float cameraHeight;
    public RadioController radio;
    public GameObject ceiling;

    void Generate()
    {
        for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                    {
                        var spawnedTile = Instantiate(tile, new Vector3(x,y), Quaternion.identity);
                        spawnedTile.transform.SetParent(canvas.transform);
                        spawnedTile.transform.localPosition = new Vector3(x,y);
                        spawnedTile.name = $"Tile {x} {y}";

                        var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                        spawnedTile.Generate(offset);
                        tileArray[x, y] = spawnedTile;
                    }
            }

        camera.transform.localPosition = new Vector3((float)(width/2 - 0.5f), (float)(height/2 - 0.5f), -cameraHeight);
    }

    void setPlayerandGoal(){
        Tile player = Access(playerX,playerY);
        Tile goal = Access(goalX,goalY);
        player.setPlayer();
        goal.setGoal();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origPlayerX = 0;
        origPlayerY = 0;
        playerX = 0;
        playerY = 0;
        goalX = (int) (width - 1);
        goalY = (int) (height - 1);
        tileArray = new Tile[(int)width,(int)height];
        Generate();
        CreateObstacles();
        setPlayerandGoal();
    }

    void Reset(){
        for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                    {
                      Destroy(x,y);
                    }
            }
        Access(playerX, playerY).notPlayer();
        playerX = origPlayerX;
        playerY = origPlayerY;
        CreateObstacles();
        setPlayerandGoal();


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

                        float num = Random.Range(0f,10f);

                        if(num > 9.5){
                            Tile tile = Access(x,y);
                            tile.changeColor(new Color(0f,0f,0f));
                            tile.makeStrong();
                            tile.setClicked();




                        }

                        else if(num > 9){
                            Tile tile = Access(x,y);
                            tile.changeColor(new Color(0.25f,0.25f,0.25f));
                            tile.makeWeak();
                            tile.setClicked();
                        }

                    }
            }

    }

    void moveUp(int x, int y){
        Tile tile = Access(x,y);
        if(tile.isPlayer){
            if(y + 1 < height){
                if(!Access(x,y+1).isClicked()){
                    Tile newPlayer = Access(x,y+1);
                    newPlayer.setPlayer();
                    Destroy(x,y);
                    tile.notPlayer();
                    playerY += 1;
                }
            }
        }
    }
    void moveDown(int x, int y){
        Tile tile = Access(x,y);
        if(tile.isPlayer){
            if(y - 1 >= 0){
                if(!Access(x,y-1).isClicked()){
                    Tile newPlayer = Access(x,y-1);
                    newPlayer.setPlayer();
                    tile.notPlayer();
                    Destroy(x,y);
                    playerY -= 1;
                }
            }
        }
    }
    void moveRight(int x, int y){
        Tile tile = Access(x,y);
        if(tile.isPlayer){
            if(x + 1 < width){
                if(!Access(x + 1,y).isClicked()){
                    Tile newPlayer = Access(x + 1,y);
                    newPlayer.setPlayer();
                    tile.notPlayer();
                    Destroy(x,y);
                    playerX += 1;
                }
            }
        }
    }
    void moveLeft(int x, int y){
        Tile tile = Access(x,y);
        if(tile.isPlayer){
            if(x - 1 >= 0 ){
                if(!Access(x - 1,y).isClicked()){
                    Tile newPlayer = Access(x - 1,y);
                    newPlayer.setPlayer();
                    tile.notPlayer();
                    Destroy(x,y);
                    playerX -= 1;
                }
            }
        }
    }

    void win(){
        ceiling.SetActive(false);
        radio.KeyGetPress();
        Debug.Log("win");
        return;
    }

    void Destroy(int x, int y){

        var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        Access(x,y).Generate(offset);
        Access(x,y).unClicked();
        Access(x,y).makeWeak();
    }

    void Shoot(int x, int y, string selection){
        int currentX = x;
        int currentY = y;

        if(Access(currentX,currentY).isPlayer){
            Reset();
        }

        if(Access(currentX,currentY).isGoal){
            return;
        }

        if(selection == "left"){
            while(currentX < width){

                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentY < height - 1 && currentY > 0){
                    currentY += choices[choice];
                }

                Tile tile = Access(currentX, currentY);
                if(Access(currentX,currentY).isPlayer){
                    Reset();
                }
                if(Access(currentX,currentY).isGoal){
                    return;
                }
                if(tile.isStrong()){
                    tile.makeWeak();
                    tile.changeColor(new Color(0.25f,0.25f,0.25f));
                    return;
                }
                else if(!tile.isStrong() && tile.isClicked()){
                    Destroy(currentX,currentY);
                    return;
                }
                else{



                    Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                    Access(currentX, currentY).Shot();

                    currentX += 1;
                }


            }
        }
        else if(selection == "right"){
            while(currentX >= 0){

                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentY < height - 1 && currentY > 0){
                    currentY += choices[choice];
                }

                Tile tile = Access(currentX, currentY);
                if(Access(currentX,currentY).isPlayer){
                    Reset();
                }
                if(Access(currentX,currentY).isGoal){
                    return;
                }
                if(tile.isStrong()){
                    tile.makeWeak();
                    tile.changeColor(new Color(0.25f,0.25f,0.25f));
                    return;
                }
                else if(!tile.isStrong() && tile.isClicked()){
                    Destroy(currentX,currentY);
                    return;
                }
                else{

                    Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                    Access(currentX, currentY).Shot();


                    currentX -= 1;
                }


            }
        }
        else if (selection == "bottom"){
            while(currentY < height){

                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentX < width - 1 && currentX > 0){
                    currentX += choices[choice];
                }

                Tile tile = Access(currentX, currentY);
                if(Access(currentX,currentY).isPlayer){
                    Reset();
                }
                if(Access(currentX,currentY).isGoal){
                    return;
                }
                if(tile.isStrong()){
                    tile.makeWeak();
                    tile.changeColor(new Color(0.25f,0.25f,0.25f));
                    return;
                }
                else if(!tile.isStrong() && tile.isClicked()){
                    Destroy(currentX,currentY);
                    return;
                }
                else{
                    Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                    Access(currentX, currentY).Shot();



                    currentY += 1;
                }


            }  
        }
        else if (selection == "top"){

            while(currentY >= 0){
                int[] choices = {0, 1, -1};
                int choice = Random.Range(0,2);
                if (currentX < width - 1 && currentX > 0){
                    currentX += choices[choice];
                }

                Tile tile = Access(currentX, currentY);
                if(Access(currentX,currentY).isPlayer){
                    Reset();
                }
                if(Access(currentX,currentY).isGoal){
                    return;
                }
                if(tile.isStrong()){
                    tile.makeWeak();
                    tile.changeColor(new Color(0.25f,0.25f,0.25f));
                    return;
                }
                else if(!tile.isStrong() && tile.isClicked()){
                    Destroy(currentX,currentY);
                    return;
                }
                else{


                    Access(currentX, currentY).changeColor(new Color(1f,0f,0f));
                    Access(currentX, currentY).Shot();



                    currentY -= 1;
                }


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
                                    Access(x, y).notShot();
                                    if(Access(x, y).isPlayer){
                                        Access(x,y).changeColor(new Color(0f,1f,0f));
                                    }
                                    else{
                                        Destroy(x,y);
                                    }

                                }

                            }
                    }
    }

    void Update(){

        if(playerX == goalX && playerY == goalY){
            win();
        }

        if(Input.GetKeyDown(KeyCode.W)){
            moveUp(playerX,playerY);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            moveDown(playerX,playerY);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            moveRight(playerX,playerY);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            moveLeft(playerX,playerY);
        }
        if(Input.GetKeyDown(KeyCode.X)){
            Reset();
        }




        if(Time.time > nextShot - resetTimer){
            clearBlasts();
        }
        if(Time.time > nextShot){
            nextShot = Time.time + timer;
            Cannon();
        }

    }

}
