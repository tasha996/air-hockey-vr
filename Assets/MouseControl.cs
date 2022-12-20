using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePaddle();
    }

    public void MovePaddle()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1<<8))
        {
            Vector3 pos = transform.position;
            pos.x = hit.point.x;
            pos.z = hit.point.z;
            //transform.position = pos;

            rb.MovePosition(pos);
        }
    }
}
