using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    private Vector3 scaleUp;
    // Start is called before the first frame update
    void Start()
    {
        scaleUp = new Vector3(3f, 3f, 3f);
        StartCoroutine(Ded());
    }

    private void Update()
    {
        gameObject.transform.localScale += scaleUp * Time.deltaTime;
    }

    IEnumerator Ded()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
