using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator_2 : MonoBehaviour {

	public enum DrawMode {NoiseMap, Mesh, FalloffMap};
	public DrawMode drawMode;

	// public Noise.NormalizeMode normalizeMode;

	public TerrainData terrainData;
	public NoiseData noiseData;
	public TextureData textureData;
	
	public Material terrainMaterial;
	
	
	// public bool useFlatShading;

	[Range(0,6)]
	public int editorPreviewLOD;
	// public float noiseScale;
	
	// public int octaves;
	// [Range(0,1)]
	// public float persistance;
	// public float lacunarity;

	// public int seed;
	// public Vector2 offset;

	// public bool useFalloff;

	// public float meshHeightMultiplier;
	// public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	// public TerrainType[] regions;
	// static MapGenerator_2 instance;

	float[,] falloffMap;

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	// void Awake() {
	// 	falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize);
	// }
	
	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}
	
	void OnTextureValuesUpdated() {
		textureData.ApplyToMaterial (terrainMaterial);
	}

	public int mapChunkSize {
		get {
			if (terrainData.useFlatShading) {
				return 95;
			} else {
				return 239;
			}
		}
	}

	// Function called when generate button clicked
	public void DrawMapInEditor() {
		
		// Generate map data : PerlinNoise map and colour map with all parameters
		MapData mapData = GenerateMapData (Vector2.zero);

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading));
		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
		}
	}

	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		ThreadStart threadStart = delegate {
			MapDataThread (centre, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MapDataThread(Vector2 centre, Action<MapData> callback) {
		MapData mapData = GenerateMapData (centre);
		lock (mapDataThreadInfoQueue) {
			mapDataThreadInfoQueue.Enqueue (new MapThreadInfo<MapData> (callback, mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
		ThreadStart threadStart = delegate {
			MeshDataThread (mapData, lod, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
		MeshData meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod, terrainData.useFlatShading);
		lock (meshDataThreadInfoQueue) {
			meshDataThreadInfoQueue.Enqueue (new MapThreadInfo<MeshData> (callback, meshData));
		}
	}

	void Update() {
		if (mapDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0) {
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue ();
				threadInfo.callback (threadInfo.parameter);
			}
		}
	}

	MapData GenerateMapData(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

		if (terrainData.useFalloff)
		{
			if (falloffMap == null)
			{
				falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize+2);
			}
			for (int y = 0; y < mapChunkSize+2; y++) {
				for (int x = 0; x < mapChunkSize+2; x++) {
					if (terrainData.useFalloff) {
						noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
					}
				}
			}
		}
		
		
		textureData.UpdateMeshHeights (terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

		return new MapData (noiseMap);
	}

	void OnValidate() {

		if (terrainData != null)
		{
			terrainData.OnValuesUpdated -= OnValuesUpdated;
			terrainData.OnValuesUpdated += OnValuesUpdated;
		}
		if (noiseData != null)
		{
			noiseData.OnValuesUpdated -= OnValuesUpdated;
			noiseData.OnValuesUpdated += OnValuesUpdated;
		}
		if (textureData != null)
		{
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
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

}

[System.Serializable]


public struct MapData {
	public readonly float[,] heightMap;

	public MapData (float[,] heightMap)
	{
		this.heightMap = heightMap;
	}
	
}