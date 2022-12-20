using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEasy : MonoBehaviour
{
    public Rigidbody puck;
    private Rigidbody rb;
    public float maxVelocity;

    public Vector3 basePos;
    public Vector3 baseTargetPos;
    private Vector3 targetPos;

    public float smoothTimeIdle; //smooth kad ceka
    public float smoothTimeAction; //smooth kad juri lopticu
    private float smoothTime; //u nju ide smooth izbor od dva iznad

    public Vector2 randomMove;
    private float tableWidth;
    private float tableHeight;
    public Vector2 tableStartPos; //pocetna koordinata stola ukljucujuci dimenzije paka

    public float maxX = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        basePos.y = rb.position.y;
        targetPos = rb.position;
        baseTargetPos = targetPos;
        StartCoroutine(MoveAround());
        tableHeight = Mathf.Abs(tableStartPos.y) * 2f;
        tableWidth = Mathf.Abs(tableStartPos.x) * 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementLogic();

        //easy enemy
        /*Vector3 pos = transform.position;
        pos.x = puckTransform.position.x; //prati x koordinatu paka
        pos.y = transform.position.y;
        pos.z = transform.position.z;
        // transform.position = pos;
        Vector3 vel = rb.velocity;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, pos, ref vel, smoothTime);
        rb.MovePosition(smoothPos);*/
    }
    void MovementLogic()
    {
        // limit speed
        if (rb.velocity.magnitude > maxVelocity)
        {
            Vector3 vel = rb.velocity.normalized;
            rb.velocity = vel * maxVelocity;
        }

        // conditions 
        bool react_vel = puck.velocity.z < 0;
        bool react_z = puck.position.z < 0;
        //react_z = true;

        if (react_vel && react_z)
        {
            // follow
            Vector2 idealPos = idealPosition(transform.position.z);
            //Vector2 idealPos = idealPosition(0);
            targetPos.x = idealPos.x + tableStartPos.x;
            targetPos.z = idealPos.y + tableStartPos.y;
            targetPos.z = baseTargetPos.z;
            // clamp x
            targetPos.x = Mathf.Clamp(targetPos.x, -maxX, maxX);
            smoothTime = smoothTimeAction;
        }
        else
        {
            // idle
            targetPos = baseTargetPos;
            smoothTime = smoothTimeIdle;
        }
        // go to target
        MoveToTarget();
    }


    void MoveToTarget()
    {
        Vector3 vel = rb.velocity;
        rb.MovePosition(Vector3.SmoothDamp(rb.position, targetPos, ref vel, smoothTime));
        //print(targetPos);
    }
    /*x=-0.72, z=1.12*/
    private Vector2 idealPosition(float offsetGlobal)
    {
        float width = tableWidth;
        float height = tableHeight;


        // get local puck pos
        Vector2 puckPos = new Vector2(puck.position.x, puck.position.z);
        Vector2 puckPosLocal = puckPos - tableStartPos;
        float offsetLocal = height - (offsetGlobal - tableStartPos.y);

        // get alpha angle
        float alpha = (Mathf.PI / 2.0f) - Mathf.Atan(puck.velocity.z / puck.velocity.x);
        bool goingRight = puck.velocity.x > 0;
        //alpha = Mathf.Abs(alpha);

        // math stuff
        float b = height - offsetLocal - puckPosLocal.y;
        float c = Mathf.Abs(b / Mathf.Cos(alpha));
        float a = Mathf.Abs(c * Mathf.Sin(alpha));

        //print("---------");
        //print(goingRight);
        //print(a + " " + b + " " + c + " ");

        float toEdge = (goingRight ? puckPosLocal.x : (width - puckPosLocal.x));
        float fa = a + toEdge;
        float da = fa % width;

        int bounces = Mathf.FloorToInt(fa / width) + (goingRight ? 0 : 1);

        float targetX = 0;

        if (bounces % 2 == 0)
        {
            targetX = da;
        }
        else
        {
            targetX = width - da;
        }

        //print(alpha / (Mathf.PI / 2.0) * 90);

        Vector2 pos = new Vector2(targetX, height - offsetLocal);

        //print(a + " " + toEdge + " " + fa + " " + bounces);

        // debug draw
        Vector2 A = puckPos;
        Vector2 C = A;
        C.y += b;
        Vector3 B = C;
        B.x += a * (goingRight ? 1 : -1);
        DrawLine(A, B, Color.green);
        DrawLine(A, C, Color.green);
        DrawLine(C, B, Color.green);

        Vector2 FB = B;
        Vector2 FC = FB;
        FC.x -= fa * (goingRight ? 1 : -1);
        Vector2 FA = FC;
        FA.y -= b * (fa / a);
        DrawLine(FA, FB, Color.red);
        DrawLine(FA, FC, Color.red);
        DrawLine(FC, FB, Color.red);


        DrawLine(puckPos, puckPos + new Vector2(puck.velocity.x, puck.velocity.z), Color.blue);


        ////DrawLine(terrainStart, terrainStart + terrainDims);
        return pos;
    }

    private void DrawLine(Vector2 p1, Vector2 p2, Color c)
    {
        float y = 0.021f;
        Debug.DrawLine(
            new Vector3(p1.x, y, p1.y),
            new Vector3(p2.x, y, p2.y),
            c
        );
    }

    IEnumerator MoveAround()
    {
        while (true)
        {
            baseTargetPos = basePos;
            baseTargetPos.x += Random.Range(-randomMove.x, randomMove.x);
            baseTargetPos.z += Random.Range(-randomMove.y, randomMove.y);

            yield return new WaitForSeconds(1f);
        }
    }


}