using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public GM gm;
    public Vector3 force;
    public Rigidbody rb;
    public float minVelocity;
    public float maxVelocity;
    public float playerGoalZ;
    public Vector3 startPosition;
    public float zClear = 1.2f;
    private MeshRenderer meshRenderer;
    public float zVelocityMin = 0.5f;
    public float xVelocityMin = 0.1f;

    private bool goal = false;

    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(force);
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // disable low angle
        Vector3 vel = rb.velocity;
        if (rb.velocity.sqrMagnitude > 0f && Mathf.Abs(rb.velocity.z) < zVelocityMin)
        {
            vel.z = zVelocityMin * Mathf.Sign(rb.velocity.z);
            rb.velocity = vel;
        }
        Vector3 velX = rb.velocity;
        if (rb.velocity.sqrMagnitude > 0f && Mathf.Abs(rb.velocity.x) < xVelocityMin)
        {
            velX.x = xVelocityMin * Mathf.Sign(rb.velocity.x);
            rb.velocity = velX;
        }

        //unstuck 
        //Vector3 velStuck = rb.velocity;
        //if (rb.velocity.sqrMagnitude > 0f && Mathf.Abs(rb.velocity.x) < 1e-5)
        //{
        //    velStuck.x = xVelocityMin * -Mathf.Sign(rb.velocity.x);
        //    rb.velocity = vel;
        //}

        if (rb.velocity.magnitude > maxVelocity)
        {

            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
        else if (rb.velocity.magnitude < minVelocity)
        {
            rb.velocity = rb.velocity.normalized * minVelocity;
        }

        if (rb.position.z > playerGoalZ && !goal)
        {
            gm.Scored(true);
            //ResetPosition();
            goal = true;
            StartCoroutine(resetDealayed());
        }
        else if (rb.position.z < -playerGoalZ && !goal)
        {
            gm.Scored(false);
            //ResetPosition();
            goal = true;
            StartCoroutine(resetDealayed());
        }
        
        bool disapear = Mathf.Abs(transform.position.z) > zClear;
        meshRenderer.enabled = !disapear;

        
    }

    void ResetPosition()
    {
        rb.velocity *= 0;
        rb.MovePosition(startPosition);
    }

    IEnumerator resetDealayed()
    {
        yield return new WaitForSeconds(1);
        rb.velocity *= 0;
        rb.MovePosition(startPosition);
        goal = false;
    }
}
