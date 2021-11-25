using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProjectileScript : MonoBehaviour
{
    public int attackPower;
    public int attackSpeed;
    private float distanceDivider;
    public float totalDamage;
    public GameObject Origin;

    public GameObject redConfetti;
    public GameObject blueConfetti;

    private Rigidbody _rigidbody;

    private ARRaycastManager _raycastManager;

    private ARAnchorManager _anchorManager;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _raycastManager = FindObjectOfType<ARRaycastManager>();
        Debug.Assert(_raycastManager);

        _anchorManager = FindObjectOfType<ARAnchorManager>();
        Debug.Assert(_anchorManager);

        StartCoroutine(DestroyCount());
        distanceDivider = (Vector3.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Opponent").transform.position));
        DamageCalc();
    }
    void Update()
    {

        if (_rigidbody.IsSleeping() && !_rigidbody.isKinematic)
        {
            _rigidbody.isKinematic = true;
            CreateAnchor();
        }

        transform.Translate(Vector3.forward * Time.deltaTime * attackSpeed);
    }

    private IEnumerator DestroyCount()
    {
        yield return new WaitForSeconds(7.5f);
        Destroy(gameObject);
    }

    private void CreateAnchor()
    {
        Vector3 screenPoint = DepthSource.ARCamera.WorldToScreenPoint(transform.position);

        // Raycasts against the location the object stopped.
        TrackableType trackableTypes = TrackableType.Planes | TrackableType.FeaturePoint;
        List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
        if (_raycastManager.Raycast(screenPoint, raycastHits, trackableTypes))
        {
            ARRaycastHit raycastHit = raycastHits[0];
            ARAnchor anchor = null;
            if ((raycastHit.trackable is ARPlane) &&
                Vector3.Dot(DepthSource.ARCamera.transform.position - raycastHit.pose.position,
                raycastHit.pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current ARPlane.");
            }
            else if (raycastHit.trackable is ARPlane plane)
            {
                Debug.Log("Create ARAnchor attached to ARPlane.");
                anchor = _anchorManager.AttachAnchor(plane, raycastHit.pose);
            }
            else
            {
                Debug.Log("Create a regular ARAnchor.");
                anchor = new GameObject().AddComponent<ARAnchor>();
                anchor.gameObject.name = "ARAnchor";
                anchor.transform.position = raycastHit.pose.position;
                anchor.transform.rotation = raycastHit.pose.rotation;
            }

            if (anchor != null)
            {
                transform.SetParent(anchor.transform, true);
            }
        }
    }

    public void DamageCalc()
    {
        if (Origin.gameObject.tag == ("Opponent"))
        {
            totalDamage = (attackPower * Origin.GetComponent<Enemy>().attackStat / GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerScript>().defenceStat);
        }

        if (Origin.gameObject.tag == ("Player"))
        {
            totalDamage = ((attackPower * Origin.GetComponent<PlayerScript>().attackStat / GameObject.FindGameObjectWithTag("Opponent").GetComponent<Enemy>().defenceStat) / ((distanceDivider)));
            Debug.Log(totalDamage);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (Origin.gameObject.tag == ("Opponent"))
        {

            if (collision.gameObject.tag == ("MainCamera"))
            {
                Instantiate(redConfetti, this.transform);
                Destroy(gameObject);
            }
        }
        if (Origin.gameObject.tag == ("MainCamera"))
        {
            if (collision.gameObject.tag == ("Opponent"))
            {
                Instantiate(blueConfetti, this.transform);
                Destroy(gameObject);
            }
        }
    }
}