using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block {

    public enum Direction {
        north,
        east,
        south,
        west,
        up,
        down
    }

    public struct Tile {
        public int x;
        public int y;
    }

    const float tileSize = 0.25f;
    public bool changed = true;

    // Base block constructor
    public Block () {

    }

    public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData) {
        // REMEMBER: to set this every override or you will use last blocks setting
        meshData.useRenderDataForCol = true;
        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down)) {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up)) {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south)) {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north)) {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west)) {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }
        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east)) {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }
        return meshData;
    }

    protected virtual MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.up));

        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataDown (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.down));

        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataNorth (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.north));

        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataEast (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.east));

        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataSouth (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.south));

        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataWest (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        
        meshData.uv.AddRange(FaceUVs(Direction.west));

        meshData.AddQuadTriangles();
        return meshData;
    }

    public virtual bool IsSolid (Direction direction) {
        switch (direction) {
            case Direction.north:
            case Direction.south:
            case Direction.east:
            case Direction.west:
            case Direction.up:
            case Direction.down:
                return true;
            default:
                return false;
        }
    
    }

    public virtual Tile TexturePosition (Direction direction) {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;

        return tile;
    }

    public virtual Vector2[] FaceUVs (Direction direction) {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);
        
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);
        
        return UVs;
    }
}
