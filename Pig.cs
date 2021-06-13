using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    Rigidbody rb;
    Animator an;
    public float moveSpeed;
    public Vector3 dir;
    public Item samgyupsal;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();

        StartCoroutine(WalkAround());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator WalkAround()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 1.5f));

            float rot = Random.Range(-360, 360);
            transform.eulerAngles = new Vector3(0, rot, 0);


            dir = transform.forward * moveSpeed;
            an.SetBool("walking", true);

            yield return new WaitForSeconds(Random.Range(2.5f, 4.5f));

            dir = Vector3.zero;
            an.SetBool("walking", false);
        }
    }
    private void Update()
    {
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);
    }
}
