using UnityEngine;
using System.Collections;

public class StaticTerrainGen : TerrainGen {

    public override Chunk ChunkColumnGen(Chunk chunk, int x, int z) {
        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++) {
            if (y <= 0)
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockGrass());
            else
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
        }
        return chunk;
    }
}
