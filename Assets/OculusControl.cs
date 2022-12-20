using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusControl : MonoBehaviour
{
    private Rigidbody rb;
    public Transform trackingSpace;
    private bool vibrating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        MovePaddle();
    }

    public void MovePaddle()
    {
        Vector3 rightPosition = trackingSpace.TransformPoint(
            OVRInput.GetLocalControllerPosition(
                OVRInput.Controller.RTouch));
        RaycastHit hit;
        Ray ray = new Ray(rightPosition, Vector3.down);
        if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
        {
            Vector3 pos = transform.position;
            pos.x = hit.point.x;
            pos.z = hit.point.z;
            rb.MovePosition(pos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Puck" && !vibrating)
        {
            StartCoroutine(Vibrate());
        }
    }

    IEnumerator Vibrate()
    {
        vibrating = true;
        OVRInput.SetControllerVibration(1.0f, 0.5f,
            OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(0.05f);
        OVRInput.SetControllerVibration(0f, 0f,
            OVRInput.Controller.RTouch);
        vibrating = false;
    }
}
