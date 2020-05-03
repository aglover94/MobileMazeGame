using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base of this code comes from the Richard Hawkes video found here: https://www.youtube.com/watch?v=IrO4mswO2o4

public class RandomMazeLoader : MonoBehaviour
{
    //Public Variables
    public int mazeRows, mazeColumns;
    public GameObject wall, playerStart, goal, mazeHolder, spawnPoint, player;
    public float size = 1f;

    //Private Variables
    private MazeCell[,] mazeCells;
    private List<GameObject> children = new List<GameObject>();
    private Rigidbody playerRB;

    // Use this for initialization
    void Start()
    {
        //Set the maze rows and columns value to a random value between 4 and 21
        mazeRows = Random.Range(4, 21);
        mazeColumns = Random.Range(4, 21);

        //Call the InitializeMaze method
        InitializeMaze();

        MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
        
        //Call the CreateMaze method from MazeAlgorithm script
        ma.CreateMaze();

        //Get the Rigidbody component from the player
        playerRB = player.GetComponent<Rigidbody>();
    }

    private void InitializeMaze()
    {
        mazeCells = new MazeCell[mazeRows, mazeColumns];

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

                //Check if r and c is 0
                if (r == 0 && c == 0)
                {
                    //If it is then instantiate the player start tile
                    mazeCells[r, c].floor = Instantiate(playerStart, 
                        new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].floor.name = "PlayerStart" + r + "," + c;
                    mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                    mazeCells[r, c].floor.transform.parent = mazeHolder.transform;

                    spawnPoint.transform.position = new Vector3(mazeCells[r, c].floor.transform.position.x, 4, mazeCells[r, c].floor.transform.position.z);
                    player.transform.position = new Vector3(mazeCells[r, c].floor.transform.position.x, 4, mazeCells[r, c].floor.transform.position.z);
                }
                else if (r == mazeRows - 1 && c == mazeColumns - 1) //Check if r is mazeRows - 1 and c is mazeColumns - 1
                {
                    //If it is then instantiate the goal tile
                    mazeCells[r, c].floor = Instantiate(goal, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].floor.name = "Goal" + r + "," + c;
                    mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                    mazeCells[r, c].floor.transform.parent = mazeHolder.transform;
                }
                else
                {
                    //Else then instantiate the wall tile and rotate to create the floor
                    mazeCells[r, c].floor = Instantiate(wall, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].floor.name = "Floor " + r + "," + c;
                    mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                    mazeCells[r, c].floor.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                    mazeCells[r, c].floor.transform.parent = mazeHolder.transform;
                }

                //Check if c equals 0
                if (c == 0)
                {
                    //If it is then instantiate the west walls and make it a child of the mazeHolder gameObject
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
                    mazeCells[r, c].westWall.transform.parent = mazeHolder.transform;
                }

                //Instantiate the east walls and make it a child of the mazeHolder gameObject
                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;
                mazeCells[r, c].eastWall.transform.parent = mazeHolder.transform;

                //Check if r equals 0
                if (r == 0)
                {
                    //If it is then instantiate the north walls, then rotate it 90°, then make it a child of the mazeHolder gameObject
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = mazeHolder.transform;
                }

                //Instantiate the south walls, then rotate it 90°, then make it a child of the mazeHolder gameObject
                mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = mazeHolder.transform;
            }
        }
    }

    public void CreateNew()
    {
        //Clear the children list
        children.Clear();

        //Used to destroy any older maze wall and floors when creating a new maze
        for (int i = 0; i < mazeHolder.transform.childCount; i++)
        {
            children.Add(mazeHolder.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < children.Count; i++)
        {
            Destroy(children[i]);
        }

        //Set the maze rows and columns value to a random value between 4 and 21
        mazeRows = Random.Range(4, 21);
        mazeColumns = Random.Range(4, 21);

        //Call the InitializeMaze method
        InitializeMaze();

        MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
        //Call the CreateMaze method from MazeAlgorithm script
        ma.CreateMaze();
    }
}
