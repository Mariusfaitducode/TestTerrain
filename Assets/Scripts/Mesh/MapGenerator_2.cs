using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator_2 : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
	public DrawMode drawMode;

	public Noise.NormalizeMode normalizeMode;

	public bool useFlatShading;

	[Range(0,6)]
	public int editorPreviewLOD;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool useFalloff;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;
	static MapGenerator_2 instance;

	float[,] falloffMap;

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	void Awake() {
		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize);
	}

	public static int mapChunkSize {
		get {
			if (instance == null) {
				instance = FindObjectOfType<MapGenerator_2> ();
			}

			if (instance.useFlatShading) {
				return 95;
			} else {
				return 239;
			}
		}
	}

	public void DrawMapInEditor() {
		MapData mapData = GenerateMapData (Vector2.zero);

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (mapData.colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD,useFlatShading), 
				TextureGenerator.TextureFromColourMap (mapData.colourMap, mapChunkSize, mapChunkSize));
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
		MeshData meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod,useFlatShading);
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
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize + 2, mapChunkSize + 2, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				if (useFalloff) {
					noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
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

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}

		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize);
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
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}

public struct MapData {
	public readonly float[,] heightMap;
	public readonly Color[] colourMap;

	public MapData (float[,] heightMap, Color[] colourMap)
	{
		this.heightMap = heightMap;
		this.colourMap = colourMap;
	}
	
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail, bool useFlatShading) {
		AnimationCurve heightCurve = new AnimationCurve (_heightCurve.keys);

		int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;

		int borderedSize = heightMap.GetLength (0);
		int meshSize = borderedSize - 2*meshSimplificationIncrement;
		int meshSizeUnsimplified = borderedSize - 2;

		float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		float topLeftZ = (meshSizeUnsimplified - 1) / 2f;


		int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

		MeshData meshData = new MeshData (verticesPerLine,useFlatShading);

		int[,] vertexIndicesMap = new int[borderedSize,borderedSize];
		int meshVertexIndex = 0;
		int borderVertexIndex = -1;

		for (int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
			for (int x = 0; x < borderedSize; x += meshSimplificationIncrement) {
				bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

				if (isBorderVertex) {
					vertexIndicesMap [x, y] = borderVertexIndex;
					borderVertexIndex--;
				} else {
					vertexIndicesMap [x, y] = meshVertexIndex;
					meshVertexIndex++;
				}
			}
		}

		for (int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
			for (int x = 0; x < borderedSize; x += meshSimplificationIncrement) {
				int vertexIndex = vertexIndicesMap [x, y];
				Vector2 percent = new Vector2 ((x-meshSimplificationIncrement) / (float)meshSize, (y-meshSimplificationIncrement) / (float)meshSize);
				float height = heightCurve.Evaluate (heightMap [x, y]) * heightMultiplier;
				Vector3 vertexPosition = new Vector3 (topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

				meshData.AddVertex (vertexPosition, percent, vertexIndex);

				if (x < borderedSize - 1 && y < borderedSize - 1) {
					int a = vertexIndicesMap [x, y];
					int b = vertexIndicesMap [x + meshSimplificationIncrement, y];
					int c = vertexIndicesMap [x, y + meshSimplificationIncrement];
					int d = vertexIndicesMap [x + meshSimplificationIncrement, y + meshSimplificationIncrement];
					meshData.AddTriangle (a,d,c);
					meshData.AddTriangle (d,a,b);
				}

				vertexIndex++;
			}
		}

		meshData.ProcessMesh ();

		return meshData;

	}
}