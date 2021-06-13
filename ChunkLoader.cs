using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public WorldGenerator wg;
    public GameObject player;
    public Vector2 playerPos;
    public List<Vector2> closeChunk = new List<Vector2>();
    public Biome[] biomes;
    public int renderDistance;

    private void Start()
    {
        UpdateRenderDistance();
        UpdateChunk();
    }
    private void Update()
    {
        if(closeChunk.Count != Mathf.Pow(renderDistance * 2 + 1, 2))
        {
            Debug.Log("List Count : " + closeChunk.Count + ", Required Count : " + Mathf.Pow(renderDistance * 2 + 1, 2));
            UpdateRenderDistance();
            UpdateChunk();
        }


        Vector2 p = playerPos;

        playerPos = new Vector2(player.transform.position.x, player.transform.position.z) * 0.0625f;
        playerPos = new Vector2((int)playerPos.x * 16, (int)playerPos.y * 16);

        if(playerPos != p)
        {
            UpdateChunk();
        }
    }
    public void UpdateChunk()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject c = transform.GetChild(i).gameObject;
            c.SetActive(false);
        }
        List<Mesh> meshes = new List<Mesh>();
        for (int i = 0; i < closeChunk.Count; i++)
        {
            if (wg.chunkDictionary.ContainsKey(playerPos + closeChunk[i]))
            {
                GameObject c = wg.chunkDictionary[playerPos + closeChunk[i]]; 
                c.SetActive(true);
                meshes.Add(c.GetComponent<MeshFilter>().mesh);
            }
            else
            {
                Vector2 pos = playerPos + closeChunk[i];
                meshes.Add(wg.MakeChunk(pos, randomBiome(pos)));
            }
        }
        foreach(Mesh m in meshes)
        {
            m.RecalculateNormals();
        }
    }
    public void UpdateRenderDistance()
    {
        //Set Render Distance
        closeChunk.Clear();
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                closeChunk.Add(new Vector2(x, y) * 16);
            }
        }


    }


    public Biome randomBiome(Vector2 pos)
    {
        pos *= 0.0625f;
        float noiseValue = (Mathf.PerlinNoise(pos.x * 0.1f, pos.y * 0.1f));


        if (noiseValue <= 0.3f)
        {
            return biomes[0];
        }
        else if (noiseValue <= 0.5f)
        {
            return biomes[1];
        }
        else 
        {
            return biomes[2];
        }
    }
}
