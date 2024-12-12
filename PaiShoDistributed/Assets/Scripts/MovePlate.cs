using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,0.0f,0.0f,1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if(attack)
        {
            GameObject tile = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            Destroy(tile);
        }

        controller.GetComponent<Game>().SetPostitionEmpty(reference.GetComponent<PaiShoTile>().GetXBoard(),
        reference.GetComponent<PaiShoTile>().GetYBoard());

        reference.GetComponent<PaiShoTile>().SetXBoard(matrixX);
        reference.GetComponent<PaiShoTile>().SetYBoard(matrixY);
        reference.GetComponent<PaiShoTile>().SetCoords();

        controller.GetComponent<Game>().SetPostition(reference);
        
        reference.GetComponent<PaiShoTile>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {

        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
