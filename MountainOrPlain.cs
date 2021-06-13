using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainOrPlain : MonoBehaviour
{
    public ChunkLoader[] cl;
    private void Awake()
    {
        cl[Random.Range(0, cl.Length)].enabled = true;
    }
}
