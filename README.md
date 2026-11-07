# TestTerrain

A small Unity sandbox for procedural terrain generation: Perlin-noise height maps turned into textured 3D meshes, tweakable live from the editor.

## What it does

- Generates a height map from layered Perlin noise (octaves, persistence, lacunarity, seed, offset).
- Optionally applies a falloff map so the terrain becomes an island.
- Builds a mesh from the height map with a height curve, level-of-detail settings and optional flat shading.
- Paints the terrain with a custom shader that blends texture layers (water, sand, grass, rock, snow) by height.
- Stores noise, terrain and texture settings in ScriptableObjects that regenerate the map as soon as a value changes; the editor can preview the noise map, the falloff map or the final mesh.

## Tech stack

Unity 2022.3, C#, HLSL shader, ScriptableObjects, custom editor scripts.

## Run it locally

1. Install Unity **2022.3.23f1**, clone the repo and open the folder with Unity Hub.
2. Open `Assets/Scenes/world_2_mesh.unity`, select the map generator object and use **Generate** in the Inspector (or turn on auto update).

## Context

Personal prototype from April 2024, following the structure of Sebastian Lague's procedural landmass generation series. The same generator was then reused and extended for the procedural city map of [EcoQuestXR](https://github.com/Mariusfaitducode/EcoQuestXR).
