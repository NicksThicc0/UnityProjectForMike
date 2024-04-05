using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Mathematics;

public enum Biomes { Plains, Desert, Tundra, Ocean}

public class WorldGenManager : MonoBehaviour
{
    public Biome[] allBiomes;

    [SerializeField] Vector2 genSize;


    private void Start()
    {
        CreateBiome();
    }


    void CreateBiome()
    {
        for (int i = 0; i < allBiomes.Length; i++)
        {

            for (int amountOfTiles = 0; amountOfTiles < allBiomes[i].biomeSize; amountOfTiles++)
            {
                Vector3Int randomPos = new Vector3Int((int)UnityEngine.Random.Range(genSize.x, -genSize.x), (int)UnityEngine.Random.Range(genSize.y, -genSize.y));

                allBiomes[i].biomeTileMap.SetTile(randomPos, allBiomes[i].BiomeTile);
                //

                //llBiomes[i].biomeTileMap.SetTile(randomPos, allBiomes[i].BiomeTile);
            }
        }
    }


    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateBiome();
        }
    }


}

[System.Serializable]
public class Biome 
{
    [SerializeField] string biomeName;

    [Header("Tiles")]
    public Tile BiomeTile;
    public AnimatedTile animatedTile;
    public Tilemap biomeTileMap;
    [Header("Biome Stats")]
    public Biomes BiomeType;
    public Vector2 test;
    //
    public int biomeSize;

}
