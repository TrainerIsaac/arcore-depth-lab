using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRegenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>().map.SetActive(true);
    }
}
