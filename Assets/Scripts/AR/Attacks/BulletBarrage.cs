using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBarrage : MonoBehaviour
{
    private bool projectileCooldown = false;
    private GameObject localProjectile;
    public GameObject projectile;
    public float speed = 1;
    public Transform player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = player.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, playerDirection, singleStep, 0);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (projectileCooldown == false)
        {
            localProjectile = Instantiate(projectile, this.transform.position, Quaternion.LookRotation(playerDirection));
            localProjectile.GetComponent<ProjectileScript>().Origin = this.gameObject;
            projectileCooldown = true;
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2);
        projectileCooldown = false;
    }
}
