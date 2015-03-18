using UnityEngine;
using System.Collections;

public class BackgroundMeshBuilder : BackgroundThread {

    public MeshData meshData;
    private Chunk chunk;

    private BackgroundMeshBuilder(object data) : base(data) {
    }
    
    public static BackgroundMeshBuilder Go(object chunk) {
        return new BackgroundMeshBuilder(chunk).Start() as BackgroundMeshBuilder;
    }
    
    protected override void ThreadFunction() {
        chunk = (Chunk)this.data;
        meshData = new MeshData();
        for (int x = 0; x < Chunk.chunkSize; x++) {
            for (int y = 0; y < Chunk.chunkSize; y++) {
                for (int z = 0; z < Chunk.chunkSize; z++) {
                    meshData = chunk.blocks[x, y, z].Blockdata(chunk, x, y, z, meshData);
                }
            }
        }
    }
    
    protected override void OnFinished() {
//        Debug.Log("Finished gen msh");
    }
}