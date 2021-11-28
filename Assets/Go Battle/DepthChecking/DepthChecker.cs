using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DepthChecker : MonoBehaviour
{

    private Rigidbody _rigidbody;

    private ARRaycastManager _raycastManager;

    private ARAnchorManager _anchorManager;

    public int speed;

    public Quaternion fireRotation;

    public GameObject floorPiece;
    public GameObject ceilingPiece;
    public GameObject wallPiece;
    public GameObject errorPiece;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _raycastManager = FindObjectOfType<ARRaycastManager>();
        Debug.Assert(_raycastManager);

        _anchorManager = FindObjectOfType<ARAnchorManager>();
        Debug.Assert(_anchorManager);

        StartCoroutine(DestroyCount());
    }
    void Update()
    {

        if (_rigidbody.IsSleeping() && !_rigidbody.isKinematic)
        {
            _rigidbody.isKinematic = true;
            CreateAnchor();
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private IEnumerator DestroyCount()
    {
        yield return new WaitForSeconds(15);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("EnvironmentPiece"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DepthMeshCollider>() != null)
        {

            if (transform.rotation.eulerAngles.x >= 25 && transform.rotation.eulerAngles.x <= 140)
            {
                Instantiate(floorPiece, transform.position, Quaternion.identity * transform.rotation);
            }

            else if (transform.rotation.eulerAngles.x >= -140 && transform.rotation.eulerAngles.x <= -40 || transform.rotation.eulerAngles.x >= 220 && transform.rotation.eulerAngles.x <= 320)
            {
                Instantiate(ceilingPiece, transform.position, Quaternion.identity * transform.rotation);
            }

            else
            {
                Instantiate(wallPiece, transform.position, Quaternion.identity * transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}