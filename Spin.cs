using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 rot;
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += rot * Time.deltaTime;
    }
}
