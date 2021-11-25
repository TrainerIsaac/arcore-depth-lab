using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDepth : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<TouchingDepth>() != null)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
