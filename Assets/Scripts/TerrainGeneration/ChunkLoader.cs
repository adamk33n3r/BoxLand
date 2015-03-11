using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour {

    public World world;
    int timer = 0;
    int chunksToLoadPerFrame = 1;
    
    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    static  WorldPos[] chunkPositions = {
        new WorldPos(0, 0, 0)
    };

    // Use this for initialization
    void Start() {
	
    }
	
    void Update() {
        DeleteChunks();
        FindChunksToLoad();
        LoadAndRenderChunks();
    }
    
    void FindChunksToLoad() {
        //Get the position of this gameobject to generate around
        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
        );
        
        //If there aren't already chunks to generate
        if (buildList.Count == 0) {
            //Cycle through the array of positions around the player
            for (int i = 0; i < chunkPositions.Length; i++) {
                //translate the player position and array position into chunk position
                WorldPos newChunkPos = new WorldPos(
                    chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                    0, 
                    chunkPositions[i].z * Chunk.chunkSize + playerPos.z
                );
                
                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);
                
                // If the chunk already exists and it's already
                // rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;
                
                //load a column of chunks in this position
                for (int y = 0; y < 3; y++) {
                    buildList.Add(new WorldPos(
                        newChunkPos.x,
                        y * Chunk.chunkSize,
                        newChunkPos.z));
                }
//                return; //What is this doing here?
            }
        }
    }
    
    void BuildChunk(WorldPos pos) {
        for (int y = pos.y - Chunk.chunkSize; y <= pos.y + Chunk.chunkSize; y += Chunk.chunkSize) {
            if (y > 64 || y < -64)
                continue;
            if (y != 0)
                continue;
            
            for (int x = pos.x - Chunk.chunkSize; x <= pos.x + Chunk.chunkSize; x += Chunk.chunkSize) {
                for (int z = pos.z - Chunk.chunkSize; z <= pos.z + Chunk.chunkSize; z += Chunk.chunkSize) {
                    
                    if (y < 0) {
                        if (world.GetChunk(x, -16, z) == null)
                            world.CreateChunk(x, -16, z);
                    } else {
                        if (world.GetChunk(x, y, z) == null)
                            world.CreateChunk(x, y, z);
                    }
                }
            }
        }
        
        updateList.Add(pos);
    }
    
    void LoadAndRenderChunks() {
        for (int i = 0; i < chunksToLoadPerFrame; i++) {
            if (buildList.Count == 0)
                break;
            BuildChunk(buildList[0]);
            buildList.RemoveAt(0);
        }
        
        for (int i = 0; i < updateList.Count; i++) {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.SetUpdate(true);
            else
                Debug.Log("loop update" + updateList.Count);
            updateList.RemoveAt(0);
        }
    }
    
    void DeleteChunks() {
        
        if (timer == 10) {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks) {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));
                
                if (distance > 256)
                    chunksToDelete.Add(chunk.Key);
            }
            
            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);
            
            timer = 0;
        }
        
        timer++;
    }
}
