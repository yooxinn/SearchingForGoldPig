using UnityEngine;

[CreateAssetMenu(fileName = "Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] Material material;
    public Material Material { get { return material; } }

    [SerializeField] float noiseScale;
    public float NoiseScale { get { return noiseScale; } }

    [SerializeField] float noiseSpeed;
    public float NoiseSpeed { get { return noiseSpeed; } }

    [SerializeField] float treeGen;
    public float TreeGen { get { return treeGen; } }

    [SerializeField] bool isMountain;
    public bool IsMountain { get { return isMountain; } }
    [SerializeField] bool isDesert;
    public bool IsDesert { get { return isDesert; } }


    [SerializeField] float mountainGround;
    public float MountainGround { get { return mountainGround; } }

    [SerializeField] float yLevel;
    public float YLevel { get { return yLevel; } }


    [SerializeField] GameObject[] treePrefab, leavesPrefab;
    public GameObject[] TreePrefab { get { return treePrefab; } }
    public GameObject[] LeavesPrefab { get { return leavesPrefab; } }

    [SerializeField] float pigGen;
    public float PigGen { get { return pigGen; } }

}
