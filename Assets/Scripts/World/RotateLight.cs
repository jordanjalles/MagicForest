using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3(0,0 , Time.deltaTime * speed), Space.World);
    }
}
