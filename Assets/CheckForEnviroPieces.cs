using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnviroPieces : MonoBehaviour
{
    public int piecesInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TouchingDepth>() != null)
        {
            piecesInRange += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TouchingDepth>() != null)
        {
            piecesInRange = 1;
        }
    }
}
