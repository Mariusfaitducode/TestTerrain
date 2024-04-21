using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    
    public enum DrawMode {NoiseMap, ColourMap, MeshMap, FalloffMap};
    public DrawMode drawMode;
    //public int mapWidth = Constants.MapSize_1;
    
    
    public int mapChunkSize = Constants.MapSize_1;
    
    //const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public CaseType.TerrainType[] regions;

    public PlateauJeu plateau;
    
    private System.Random ran = new System.Random();

    public InitTerrain terrain;
    
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool useFallof;

    private float[,] fallofMap;
    
    public Noise.NormalizeMode normalizeMode;

    public bool autoUpdate;

    
    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public int level;

    
    public void Start()
    {
        fallofMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
        int size = Constants.GetConstant(level);
        print(size);

        mapChunkSize = size;
        mapChunkSize = size;
    }


    public void GenerateMap(){
        
        float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize,seed, noiseScale, octaves, persistance, lacunarity, offset, normalizeMode);
        
        Color[] colourMap = new Color[mapChunkSize* mapChunkSize];
        for ( int y = 0; y < mapChunkSize; y++){
            for ( int x = 0; x < mapChunkSize; x++){
                float currentHeight = noiseMap [x, y];
                for (int i = 0; i < regions.Length; i++){
                    if (currentHeight <= regions[i].height){
                        colourMap [y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay> ();
        if (drawMode == DrawMode.NoiseMap){
            display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap){
            display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.MeshMap)
        {
            //display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD,useFlatShading), 
              //  TextureGenerator.TextureFromColourMap (mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }
    }

    public void Generate3dMap(bool rand, int level)
    {
        
        
        int size = Constants.GetConstant(level);
        print(size);

        mapChunkSize = size;
        mapChunkSize = size;

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance,
            lacunarity, offset, normalizeMode);
        
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        
        for ( int y = 0; y < mapChunkSize; y++){
            for ( int x = 0; x < mapChunkSize; x++){
                
                if (this.useFallof) {
                    noiseMap [x, y] -= fallofMap[x, y];
                }
                float currentHeight = noiseMap [x, y];
                
                
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight >= regions [i].height) {
                        print(regions[i]);
                        colourMap [y * mapChunkSize + x] = regions[i].colour;
                    } else {
                        break;
                    }
                }
            }
        }
        print(noiseMap);
        if (rand)
        {
            seed = ran.Next(0, 100);
            print("random = "+ seed);
        }
        
        print("length noise");
        print(noiseMap.Length);
        
        MapDisplay display = FindObjectOfType<MapDisplay> ();
        
        if (drawMode == DrawMode.MeshMap)
        {
           //display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
             //   TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
        }
        else
        {
            terrain.GenerateTerrain(noiseMap, regions, level);
        }
    }
    
    
    
    MapData GenerateMapData(Vector2 centre) {
        float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) {
            for (int x = 0; x < mapChunkSize; x++) {
                if (this.useFallof) {
                    noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - fallofMap[x, y]);
                }
                float currentHeight = noiseMap [x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight >= regions [i].height) {
                        colourMap [y * mapChunkSize + x] = regions [i].colour;
                    } else {
                        break;
                    }
                }
            }
        }


        return new MapData (noiseMap, colourMap);
    }
    
    void OnValidate(){
        if (mapChunkSize < 1){
            mapChunkSize = 1;
        }
        if (mapChunkSize < 1){
            mapChunkSize = 1;
        }
        if (lacunarity < 1){
            lacunarity = 1;
        }
        if (octaves < 0){
            octaves = 0;
        }
    }
    struct MapThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo (Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }

    }
    
    public struct MapData {
        public readonly float[,] heightMap;
        public readonly Color[] colourMap;

        public MapData (float[,] heightMap, Color[] colourMap)
        {
            this.heightMap = heightMap;
            this.colourMap = colourMap;
        }
    }
}


