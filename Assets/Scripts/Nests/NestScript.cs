using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class NestScript : MonoBehaviour
{
    public string Monster;

    public int maxDistance;
    public Vector3 oldPos;
    private List<Vector2> linePoints = new List<Vector2>();
    public int order;
    public List<GameObject> nextNest = new List<GameObject>();
    public bool hasFought = false;

    void Start()
    {
        maxDistance = Random.Range(1, 5);   //Multiply by difficulty modifier - perhaps have script assigned to each map that sets things like difficulty and potential species?
        if(oldPos != new Vector3 (0,0,0))
        {
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        }
        gameObject.GetComponent<LineRenderer>().SetPosition(0, oldPos);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, this.gameObject.transform.position);
        //uses distance generated from MapScript. 
        //Keeps track of current KM Walked. Can have max - current taken away from banked distance.
        linePoints = (new List<Vector2>() { (new Vector2 (oldPos.x - gameObject.transform.position.x, oldPos.y - gameObject.transform.position.y) ), new Vector2(gameObject.transform.position.x - gameObject.transform.position.x, gameObject.transform.position.y - gameObject.transform.position.y) });
        gameObject.GetComponent<EdgeCollider2D>().SetPoints(linePoints); 
    }
}
