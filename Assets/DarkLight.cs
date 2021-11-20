using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Light>().color = new Color(-255f, -255f, -255f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
