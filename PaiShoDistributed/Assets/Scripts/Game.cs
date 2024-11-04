using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject PaiShoTile;

    private GameObject[,] positions = new GameObject[17,17];
    private GameObject[] playerWhite = new GameObject[11];
    private GameObject[] playerBlue = new GameObject[11];

    private string currentPlayer;

    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        playerWhite = new GameObject[]
        {
            Create("WheelWhite", 5,3), Create("WheelWhite", 11,3), Create("GinsengWhite", 4,4), 
            Create("GinsengWhite", 12, 4), Create("OrchidWhite", 3,4 ),  Create("OrchidWhite", 13,4 ),  
            Create("LionTurtleWhite", 8, 4), Create("WhiteLotusWhite", 8,0 ), Create("KoiFishWhite", 6,2 ),
            Create("SkyBisonWhite", 10, 2), Create("BadgerMoleWhite", 7, 1), Create("DragonWhite", 9, 1)
        };
        playerBlue = new GameObject[]
        {
            Create("WheelBlue", 11,13), Create("WheelBlue",5,13), Create("GinsengBlue", 12,12), 
            Create("GinsengBlue", 4, 12), Create("OrchidBlue", 13,12),  Create("OrchidBlue", 3,12 ),  
            Create("LionTurtleBlue", 8, 12), Create("WhiteLotusBlue", 8,16 ), Create("KoiFishBlue", 10,14 ),
            Create("SkyBisonBlue", 6, 14), Create("BadgerMoleBlue", 9, 15), Create("DragonBlue", 7, 15)
        };
        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPostition(playerWhite[i]);
            SetPostition(playerBlue[i]);
        }
    }
    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(PaiShoTile, new Vector3(0,0,-1), Quaternion.identity);
        PaiShoTile tile = obj.GetComponent<PaiShoTile>();
        tile.name = name;
        tile.SetXBoard(x);
        tile.SetYBoard(y);
        tile.Activate();
        return obj;
    }

    public void SetPostition(GameObject obj)
    {
        PaiShoTile tile = obj.GetComponent<PaiShoTile>();

        positions[tile.GetXBoard(), tile.GetYBoard()] = obj;
    }

    public void SetPostitionEmpty(int x, int y)
    {
        positions[x,y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x > positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        return true;
    }
}