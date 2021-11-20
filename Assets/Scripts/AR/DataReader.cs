using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>().monster.Contains("e"))
        {
            Instantiate(Resources.Load("Foe"));
        }

        else
        {
            Instantiate(Resources.Load("Opponent"));
        }
    }
}
