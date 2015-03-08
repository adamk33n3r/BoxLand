using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();

    public GameObject chunkPrefab;

    public string worldName = "world";

    private WorldPos startingSize = new WorldPos(8, 3, 8);

//    void Start () {
//        int xsize = Mathf.FloorToInt(startingSize.x / 2);
//        int zsize = Mathf.FloorToInt(startingSize.z / 2);
//        for (int x = -xsize; x < xsize; x++) {
//            for (int y = -1; y < 3; y++) {
//                for (int z = -zsize; z < zsize; z++) {
//                    CreateChunk(x * 16, y * 16, z * 16);
//                }
//            }
//        }
//    }

    public void CreateChunk(int x, int y, int z) {
        WorldPos worldPos = new WorldPos(x, y, z);

        // Instantiate a new chunk from prefab
        GameObject newChunkObject = Instantiate(
            chunkPrefab,
            new Vector3(x, y, z),
            Quaternion.Euler(Vector3.zero)) as GameObject;
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        // Add it to the chunks dict with the pos as key
        chunks.Add(worldPos, newChunk);

//        for (int xi = 0; xi < 16; xi++) {
//            for (int yi = 0; yi < 16; yi++) {
//                for (int zi = 0; zi < 16; zi++) {
//                    if (yi <= 7) {
//                        SetBlock(x + xi, y + yi, z + zi, new BlockGrass());
//                    } else {
//                        SetBlock(x + xi, y + yi, z + zi, new BlockAir());
//                    }
//                }
//            }
//        }

        TerrainGen terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);

        newChunk.SetBlocksUnmodified();
        if (Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WebGLPlayer)
            Serialization.Load(newChunk);
    }

    public void DestroyChunk(int x, int y, int z) {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk)) {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z) {
        WorldPos pos = new WorldPos(x, y, z);
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);
        return containerChunk;
    }

    public Block GetBlock(int x, int y, int z) {
        Chunk containerChunk = GetChunk(x, y, z);

        if (containerChunk != null) {
            return containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);
        } else {
            return new BlockAir();
        }
    }

    public void SetBlock(int x, int y, int z, Block block) {
        Chunk chunk = GetChunk(x, y, z);
        
        if (chunk != null) {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

            // Update adjacent chunk if block on edge is updated
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos) {
        if (value1 == value2) {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null) {
                chunk.update = true;
            }
        }
    }
}
