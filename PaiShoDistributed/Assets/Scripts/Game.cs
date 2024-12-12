using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class Game : MonoBehaviour
{
    public GameObject PaiShoTile;
    public GameObject movePlate;
    private GameObject[,] positions = new GameObject[17,17];
    private GameObject[] playerWhite = new GameObject[11];
    private GameObject[] playerBlue = new GameObject[11];

    public QueueProducer gameLogicProducer {get; private set; }
    public QueueConsumer gameLogicConsumer {get; private set; }
    private string currentPlayer;
    public bool myTurn {get;  set; }
    public bool movePlateClicked {get;  set; }
    private Guid guid;
    private bool gameOver;
    // Start is called before the first frame update
    async void Start()
    { 
        gameOver = false;
        myTurn = false;
        movePlateClicked = true;
        guid = Guid.NewGuid();
        QueueProducer requestMatchMaking = new QueueProducer();
        await requestMatchMaking.DeclareQueue("MatchMaking");
        Debug.Log("Queue Declared");
        await requestMatchMaking.SendMessage(Encoding.UTF8.GetBytes(guid.ToString()));
        requestMatchMaking.rabbitMQConnection.Disconnect();

        QueueConsumer awaitMatchMaking = new QueueConsumer();
        await awaitMatchMaking.DeclareQueue(guid.ToString());
        await awaitMatchMaking.InitConsumer();
        string message = await awaitMatchMaking.ConsumeMessageAsync();
        awaitMatchMaking.rabbitMQConnection.Disconnect();

        gameLogicConsumer = new QueueConsumer();
        await gameLogicConsumer.DeclareQueue($"{message}{guid.ToString()}");
        await gameLogicConsumer.InitConsumer();

        gameLogicProducer = new QueueProducer();
        await gameLogicProducer.DeclareQueue($"{guid.ToString()}{message}");
        Debug.Log($"{message}{guid.ToString()}");
        
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
        currentPlayer = await gameLogicConsumer.ConsumeMessageAsync();
        Debug.Log(currentPlayer);
        while (!gameOver)
        {
            //wait for turn 
            string turn = await gameLogicConsumer.ConsumeMessageAsync();
            Debug.Log($"{turn}");
            if (turn == "Loss")
            {
                gameOver = true;
                Loss();
                break;
            }
            else
            {
                await MakeMove();
            }
            string gameState = await gameLogicConsumer.ConsumeMessageAsync();
            if (gameState == "Win")
            {
                gameOver = true;
                Win();
                break;
            }
            else
            {
                UpdateState(gameState);
            }
        }
    }

    public void UpdateState(string state)
    {
        string[] boardSpots = state.Split(',');
        
    }

    public async Task MakeMove()
    {
        movePlateClicked = false;
        while (!movePlateClicked)
        {
            myTurn = true;
            string movePlates = await gameLogicConsumer.ConsumeMessageAsync();
            generateMovePlates(movePlates);
            Debug.Log($"MovePlatesReceived:{movePlates}");
        }
    }

    public void generateMovePlates(string movePlates)
    {
        string[] movePlateCoords = movePlates.Split(',');
        foreach (string movePlateCoord in movePlateCoords)
        {
            string[] coords = movePlateCoord.Split(',');
            MovePlateSpawn(Int32.Parse(coords[0]), Int32.Parse(coords[1]));
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
    public void Win()
    {
    }

    public void Loss()
    {
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(PaiShoTile, new Vector3(0,0,-1), Quaternion.identity);
        PaiShoTile tile = obj.GetComponent<PaiShoTile>();
        tile.game = this;
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

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }
    
    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }
}
