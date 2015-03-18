using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class MultiThreadedChunkGen {
    public static void GenerateNoiseData(object sender, DoWorkEventArgs e) {
        Chunk newChunk = (Chunk)e.Argument;
        TerrainGen terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);
        e.Result = newChunk;
    }

    public static void GenerateMesh() {
        BackgroundSleeper.Go(null);
    }

    static void generateMesh(object sender, DoWorkEventArgs e) {
        Debug.Log("Generating mesh...");
    }

    static void generateMeshCallback(object sender, RunWorkerCompletedEventArgs e) {
        Debug.Log("Generating mesh callback");
    }
}
