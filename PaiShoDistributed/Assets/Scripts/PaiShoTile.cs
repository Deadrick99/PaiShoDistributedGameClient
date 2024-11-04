using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PaiShoTile : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    //position
    private int xBoard = -1;
    private int yBoard = -1;

    // Variable to keep track of "white or blue" player
    private string player;

    // References for all the sprites that the chesspiece can be
    public Sprite KoiFishBlue, KoiFishWhite, BadgerMoleBlue, BadgerMoleWhite, DragonBlue, DragonWhite,
     GinsengBlue, GinsengWhtie, LionTurtleBlue, LionTurtleWhite, OrchidBlue, OrchidWhite, 
     WheelBlue, WheelWhite, WhiteLotusWhite, WhiteLotusBlue, SkyBisonBlue,SkyBisonWhite;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take the instantiated location and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "KoiFishBlue": 
                this.GetComponent<SpriteRenderer>().sprite = KoiFishBlue;
                player = "blue";
                break;

            case "KoiFishWhite": 
                this.GetComponent<SpriteRenderer>().sprite = KoiFishWhite;
                player = "white";
                break; 

            case "BadgerMoleBlue": 
                this.GetComponent<SpriteRenderer>().sprite = BadgerMoleBlue;
                player = "blue";
                break;

            case "BadgerMoleWhite": 
                this.GetComponent<SpriteRenderer>().sprite = BadgerMoleWhite;
                player = "white";
                break; 

            case "DragonBlue": 
                this.GetComponent<SpriteRenderer>().sprite = DragonBlue;
                player = "blue";
                break;

            case "DragonWhite": 
                this.GetComponent<SpriteRenderer>().sprite = DragonWhite;
                player = "white";
                break; 

            case "GinsengBlue": 
                this.GetComponent<SpriteRenderer>().sprite = GinsengBlue;
                player = "blue";
                break;

            case "GinsengWhite": 
                this.GetComponent<SpriteRenderer>().sprite = GinsengWhtie;
                player = "white";
                break; 

            case "LionTurtleBlue": 
                this.GetComponent<SpriteRenderer>().sprite = LionTurtleBlue;
                player = "blue";
                break;

            case "LionTurtleWhite": 
                this.GetComponent<SpriteRenderer>().sprite = LionTurtleWhite;
                player = "white";
                break; 

            case "OrchidBlue": 
                this.GetComponent<SpriteRenderer>().sprite = OrchidBlue;
                player = "blue";
                break;

            case "OrchidWhite": 
                this.GetComponent<SpriteRenderer>().sprite = OrchidWhite;
                player = "white";
                break; 

            case "WheelBlue": 
                this.GetComponent<SpriteRenderer>().sprite = WheelBlue;
                player = "blue";
                break;

            case "WheelWhite": 
                this.GetComponent<SpriteRenderer>().sprite = WheelWhite;
                player = "white";
                break; 

            case "WhiteLotusWhite": 
                this.GetComponent<SpriteRenderer>().sprite = WhiteLotusWhite;
                player = "white";
                break;

            case "WhiteLotusBlue": 
                this.GetComponent<SpriteRenderer>().sprite = WhiteLotusBlue;
                player = "blue";
                break; 

            case "SkyBisonBlue": 
                this.GetComponent<SpriteRenderer>().sprite = SkyBisonBlue;
                player = "blue";
                break;

            case "SkyBisonWhite": 
                this.GetComponent<SpriteRenderer>().sprite = SkyBisonWhite;
                player = "white";
                break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 4.2f;
        y *= 4.2f;

        x += -33.5f;
        y += -33.5f;

        this.transform.position = new Vector3(x,y,-1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();
        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i< movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "KoiFishBlue":
            case "KoiFishWhite": 
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "BadgerMoleBlue": 
            case "BadgerMoleWhite": 
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "DragonBlue":
            case "DragonWhite":
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "GinsengBlue":
            case "GinsengWhite": 
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "LionTurtleBlue":
            case "LionTurtleWhite":
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "OrchidBlue":
            case "OrchidWhite": 
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "WheelBlue":
            case "WheelWhite": 
                WheelPlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "WhiteLotusWhite":
            case "WhiteLotusBlue": 
                WhiteLotusMovePlate(this.GetXBoard(), this.GetYBoard());
                break; 

            case "SkyBisonBlue":
            case "SkyBisonWhite": 
                MovePlate(this.GetXBoard(), this.GetYBoard());
                break;
        }
    }

    public void WheelPlate(int xPosition, int yPosition)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard;
        int y = yBoard;

        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += 1;
            y += 0;
        } 
        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += -1;
            y += 0;
        } 
        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += 0;
            y += -1;
        } 
        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += 0;
            y += 1;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x,y).GetComponent<PaiShoTile>().player != player)
        {
            MovePlateAttackSpawn(x,y);
        }
    }

    public void MovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        // Dynamically compute valid moves
        for (int dx = -5; dx <= 5; dx++)
        {
            for (int dy = -5; dy <= 5; dy++)
            {
                if ((Math.Abs(dy) + Math.Abs(dx)) <= 5) // Check movement constraint
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    // Check bounds
                    if (nx >= 0 && nx < 17 && ny >= 0 && ny < 17)
                    {
                        // Ensure path between (x, y) and (nx, ny) is free of obstacles
                        if (IsPathClear(sc, x, y, nx, ny, sc.GetPosition(x,y).GetComponent<PaiShoTile>().player))
                        {
                            if (sc.PositionOnBoard(nx, ny))
                            {
                                
                                GameObject tile = sc.GetPosition(nx, ny);
                                if (tile == null)
                                {
                                    MovePlateSpawn(nx, ny); // Fix: Spawn at (nx, ny)
                                }
                                else if (tile.GetComponent<PaiShoTile>().player != player)
                                {
                                    MovePlateAttackSpawn(nx, ny); // Fix: Attack at (nx, ny)
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Function to check if the path between (x1, y1) and (x2, y2) is clear
    public static bool IsPathClear(Game sc, int x1, int y1, int x2, int y2, string currentPlayer)
    {
        // Direction vectors for up, down, left, right movement
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };
        // Queue for BFS with tracking path distance
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue((x1, y1, 0)); // Include path length in the queue

        // Set to keep track of visited nodes
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        visited.Add((x1, y1));

        while (queue.Count > 0)
        {
            var (currentX, currentY, distance) = queue.Dequeue();

            // Stop further exploration if distance exceeds max allowable
            if (distance > 4)
            {
                continue;
            }

            // Explore neighbors
            for (int i = 0; i < 4; i++)
            {
                int newX = currentX + dx[i];
                int newY = currentY + dy[i];

                // Check boundaries
                if (newX < 0 || newY < 0 || newX > 16 || newY > 16)
                {
                    continue;
                }

                // Check if reached the destination
                if (newX == x2 && newY == y2)
                {
                    GameObject destinationTile = sc.GetPosition(newX, newY);
                    if (destinationTile == null)
                    {
                        return true; // Destination is open
                    }

                    PaiShoTile destinationPaiShoTile = destinationTile.GetComponent<PaiShoTile>();
                    if (destinationPaiShoTile != null && destinationPaiShoTile.player != currentPlayer)
                    {
                        return true; // Allow moving onto an enemy tile
                    }

                    return false; // Can't move to a tile occupied by a friendly player
                }

                // Check the tile for possible movement
                GameObject tile = sc.GetPosition(newX, newY);
                if (tile != null && !visited.Contains((newX, newY)))
                {
                    PaiShoTile paiShoTile = tile.GetComponent<PaiShoTile>();
                    if(paiShoTile.player != currentPlayer)
                    {
                        Debug.Log("hi");
                        visited.Add((newX, newY));
                        queue.Enqueue((newX, newY, distance + 5));
                    }
                }
                else if (tile == null && !visited.Contains((newX, newY)))
                {
                    visited.Add((newX, newY));
                    queue.Enqueue((newX, newY, distance + 1));
                }
            }
        }

        return false; // If queue is empty and destination not found
    }

    public void WhiteLotusMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        // Dynamically compute diagonal moves
        for (int dx = -2; dx <= 2; dx += 2) // Only diagonal jumps (±2)
        {
            for (int dy = -2; dy <= 2; dy += 2) // Only diagonal jumps (±2)
            {
                int nx = x + dx;
                int ny = y + dy;

                // Check bounds for target tile
                if (nx >= 0 && nx < 16 && ny >= 0 && ny < 16)
                {
                    // Calculate the intermediate position
                    int mx = x + dx / 2;
                    int my = y + dy / 2;

                    // Ensure the intermediate tile is occupied, and the target tile is empty
                    if (sc.PositionOnBoard(mx, my) && sc.GetPosition(mx, my) != null) // Intermediate must be occupied
                    {
                        if (sc.PositionOnBoard(nx, ny) && sc.GetPosition(nx, ny) == null) // Target must be empty
                        {
                            MovePlateSpawn(nx, ny); // Spawn a movement plate for valid empty tiles
                        }
                    }
                }
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        x *= 4.2f;
        y *= 4.2f;

        x += -33.5f;
        y += -33.5f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        x *= 4.2f;
        y *= 4.2f;

        x += -33.5f;
        y += -33.5f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}