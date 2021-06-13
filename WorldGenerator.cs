using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunkPrefab, World, cloudPrefab, pigPrefab;


    public int xsize, zsize;
    //public Biome biome;
    public Dictionary<Vector2, GameObject> chunkDictionary = new Dictionary<Vector2, GameObject>();
    public GameObject player;
    [HideInInspector]public float spawnY;
    public float minY;
    public Vector2 spawnPos;
    public LayerMask world;
    public Material sandMat;

    public int cloudGen;


    private void Awake()
    {
        spawnPos = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        while (IsOcean(spawnPos))
        {
            spawnPos = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        }
        Debug.Log(IsOcean(spawnPos));
        player.transform.position = Vector3.up * spawnY + new Vector3(spawnPos.x, 0, spawnPos.y);
        Invoke(nameof(SpawnPlayer), 0.1f);
    }
    public bool IsOcean(Vector2 pos)
    {
        Vector2 oceanPos = pos * 0.0625f;
        oceanPos = new Vector2(Mathf.FloorToInt(oceanPos.x), Mathf.FloorToInt(oceanPos.y));
        return Mathf.PerlinNoise(oceanPos.x * 0.05f, oceanPos.y * 0.05f) >= 0.3f;
    }
    private void SpawnPlayer()
    {
        player.transform.position = Vector3.up * spawnY + new Vector3(spawnPos.x, 0, spawnPos.y);
        player.SetActive(true);
    }

    public Mesh MakeChunk(Vector2 pos, Biome b)
    {
        bool isOcean = false;//IsOcean(pos);

        Dictionary<Vector2, float> yDictionary = new Dictionary<Vector2, float>();


        //Make Prefab
        GameObject c = Instantiate(chunkPrefab, World.transform);
        c.name = "(" + pos.x.ToString() + ", " + pos.y.ToString() + ")";
        c.GetComponent<MeshRenderer>().material = isOcean ?sandMat :  b.Material;
        chunkDictionary.Add(pos, c);

        //Make Meshes
        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;


        List<Vector2> treepos = new List<Vector2>();
        List<Vector2> pigPos = new List<Vector2>();
        float TG = b.TreeGen;
        if (!isOcean)
        {

            if (TG >= 1)
            {
                TG += Random.Range(-1, 2);
                for (int i = 0; i < TG; i++)
                {
                    Vector2 p = new Vector2(pos.x + Random.Range(0, xsize), pos.y + Random.Range(0, xsize));
                    treepos.Add(p);
                }
            }
            else if (TG != 0)
            {
                TG *= 100;
                if (Random.Range(0, 100) < TG)
                {
                    Vector2 p = new Vector2(pos.x + Random.Range(0, xsize), pos.y + Random.Range(0, xsize));
                    treepos.Add(p);
                }
            }
        }

        //Spawn Pigs
        float pg = b.PigGen;
        if (isOcean)
        {
            pg = 0;
        }
        else if (pg >= 1)
        {
            pg += Random.Range(-1, 2);
            for (int i = 0; i < pg; i++)
            {
                Vector2 p = new Vector2(pos.x + Random.Range(0, xsize), pos.y + Random.Range(0, xsize));
                pigPos.Add(p);
            }
        }
        else if (pg != 0)
        {
            pg *= 100;
            if (Random.Range(0, 100) < TG)
            {
                Vector2 p = new Vector2(pos.x + Random.Range(0, xsize), pos.y + Random.Range(0, xsize));
                pigPos.Add(p);
            }
        }

        vertices = new Vector3[(xsize + 1) * (zsize + 1)];
        for(int i = 0, z = 0; z <= zsize; z++)
        {
            for(int x = 0; x <= xsize; x++)
            {
                float posx = x + pos.x;
                float posz = z + pos.y;


                float y = WorldNoise(posx * b.NoiseSpeed, posz * b.NoiseSpeed, b);//isOcean ? OceanNoise(posx * b.NoiseSpeed, posz * b.NoiseSpeed, b) : WorldNoise(posx * b.NoiseSpeed,posz * b.NoiseSpeed, b);
                y += b.YLevel;
                if (isOcean)
                    y -= 50;

                if(!yDictionary.ContainsKey(new Vector2(posx, posz)))
                {
                    yDictionary.Add(new Vector2(posx, posz), y);
                }
                if(posx == spawnPos.x && posz == spawnPos.y)
                {
                    spawnY = y;
                }
                if (treepos.Contains(new Vector2(posx, posz)))
                {
                    MakeTree(new Vector3(posx, y - 1, posz), c, b, b.IsDesert ? new Vector3(0, Random.Range(0, 360), 0) : new Vector3(-90, 0, Random.Range(0, 360)));
                }
                if (pigPos.Contains(new Vector2(posx, posz)))
                {
                    GameObject pig = Instantiate(pigPrefab, c.transform);
                    pig.transform.position = new Vector3(posx, y + 1, posz);
                }



                vertices[i] = new Vector3(posx, y,posz);




                i++;
            }
        }


        triangles = new int[xsize * zsize * 6];
        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zsize; z++)
        {
            for (int x = 0; x < xsize; x++)
            {
                triangles[tris] = vert + 0;
                triangles[tris + 1] = vert + xsize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xsize + 1;
                triangles[tris + 5] = vert + xsize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];


        for (int i = 0, z = 0; z <= zsize; z++)
        {
            for (int x = 0; x <= xsize; x++)
            {
                //uvs[i] = new Vector2((float)x * 0.9f, (float)z * 0.9f);
                uvs[i] = new Vector2(x, z);
                i++;
            }
        }

        Mesh mesh = c.GetComponent<MeshFilter>().mesh;
        MeshCollider mc = c.GetComponent<MeshCollider>();

        

        //Apply Mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mc.sharedMesh = mesh;
        mesh.uv = uvs;


        //Make Cloud
        if(Random.Range(0, cloudGen) == 0)
        {
            GameObject cloud = Instantiate(cloudPrefab, c.transform);
            cloud.transform.localScale = new Vector3(Random.Range(10, 60), 4, Random.Range(10, 60));
            cloud.transform.localPosition = new Vector3(pos.x + Random.Range(0, xsize), Random.Range(100, 150), pos.y + Random.Range(0, zsize));
        }



        return mesh;
    }

    public void MakeTree(Vector3 pos, GameObject parent, Biome b, Vector3 rot)
    {




        GameObject t = Instantiate(b.TreePrefab[Random.Range(0, b.TreePrefab.Length)], parent.transform);
        if(b.LeavesPrefab.Length != 0)
        {
            GameObject l = Instantiate(b.LeavesPrefab[Random.Range(0, b.LeavesPrefab.Length)], t.transform);
            l.name = "Leave";
            l.transform.position = t.transform.Find("LeafPoint").position;
            l.transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
        t.transform.position = pos;
        t.transform.eulerAngles = rot;
    }

    public float WorldNoise(float x, float y, Biome b)
    {
        float noise = Mathf.PerlinNoise(x, y) * Mathf.PerlinNoise(x * 0.01f, y * 0.01f) *b.NoiseScale;
        float mountainNoise = MoutainNoise(x * 0.2f, y * 0.2f, 300);
        return (mountainNoise * Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * 100 + noise * (1 - Mathf.PerlinNoise(x * 0.1f, y * 0.1f)) * 100) * 0.01f;
    }
    public float OceanNoise(float x, float y, Biome b)
    {
        return WorldNoise(x,y,b) * 0.1f - 1;
    }

    public float MoutainNoise(float x, float z, Biome b)
    {
        float y = Mathf.PerlinNoise(x * b.NoiseSpeed, z * b.NoiseSpeed) * b.NoiseScale;
        if(y <= b.MountainGround)
        {
            y = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 10 - b.YLevel + 5; ;
        }

        return y;
    }
    public float MoutainNoise(float x, float z, float scale)
    {
        float y = Mathf.PerlinNoise(x, z) * scale;
        if (y <= 130)
        {
            y = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 10 + 120 + 5; ;
        }

        return y - 130;
    }
}