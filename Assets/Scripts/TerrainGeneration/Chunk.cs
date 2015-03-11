using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Chunk : MonoBehaviour {

    public World world;
    public WorldPos pos;
    public Block[ , , ] blocks = new Block[chunkSize, chunkSize, chunkSize];
    public static int chunkSize = 16;
    public bool rendered;
    
    ChunkObject chunkObj;
    
    public struct MeshObjects {
        public MeshFilter filter;
        public MeshCollider coll;
        public MeshData meshData;
        public Block[ , , ] blocks;
        public Chunk chunk;
    }
    
    public MeshObjects? meshToRender = null;

    // Use this for initialization
    void Start() {
	    
    }
	
    // Update is called once per frame
    void Update() {
	
    }

    public Chunk(ChunkObject chunkObject) {
        chunkObj = chunkObject;
        chunkObj.AttachChunk(this);
    }
    
    public Block GetBlock(int x, int y, int z) {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }
    
    public void SetBlock(int x, int y, int z, Block block) {
        if (InRange(x) && InRange(y) && InRange(z))
            blocks[x, y, z] = block;
        else
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
    }
    
    public static bool InRange(int index) {
        if (index < 0 || index >= chunkSize)
            return false;
        return true;
    }
    
    // Updates the chunk based on its contents
    public void UpdateChunk(MeshFilter filter, MeshCollider coll, bool multithread = false) {
        if (meshToRender != null) {
            rendered = true;
            
            //            Debug.Log("Building mesh");
            MeshData meshData = ((MeshObjects)meshToRender).meshData;
            filter.mesh.Clear();
            filter.mesh.vertices = meshData.vertices.ToArray();
            filter.mesh.triangles = meshData.triangles.ToArray();
            
            filter.mesh.uv = meshData.uv.ToArray();
            filter.mesh.RecalculateNormals();
            
            coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colVertices.ToArray();
            mesh.triangles = meshData.colTriangles.ToArray();
            mesh.RecalculateNormals();
            
            coll.sharedMesh = mesh;
            meshToRender = null;
        }
        if (world.multithread) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(UpdateMeshData);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RenderMeshData);
            MeshObjects meshObjects = new MeshObjects();
            meshObjects.filter = filter;
            meshObjects.coll = coll;
            meshObjects.blocks = blocks;
            meshObjects.chunk = this;
            bw.RunWorkerAsync(meshObjects);
        } else {
            rendered = true;
            MeshData meshData = new MeshData();
            for (int x = 0; x < chunkSize; x++) {
                for (int y = 0; y < chunkSize; y++) {
                    for (int z = 0; z < chunkSize; z++) {
                        //meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData, filter, coll);
        }
    }
    
    static void UpdateMeshData(object sender, DoWorkEventArgs e) {
        MeshObjects meshObjects = (MeshObjects)e.Argument;
        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                for (int z = 0; z < chunkSize; z++) {
                    //meshData = meshObjects.blocks[x, y, z].Blockdata(meshObjects.chunk, x, y, z, meshData);
                }
            }
        }
        meshObjects.meshData = meshData;
        e.Result = meshObjects;
    }
    
    static void RenderMeshData(object sender, RunWorkerCompletedEventArgs e) {
        MeshObjects meshObjects = (MeshObjects)e.Result;
        meshObjects.chunk.meshToRender = meshObjects;
    }
    
    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData, MeshFilter filter, MeshCollider coll) {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();
        
        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();
        
        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();
        
        coll.sharedMesh = mesh;
    }
    
    public void SetBlocksUnmodified() {
        foreach (Block block in blocks) {
            block.changed = false;
        }
    }
    
    public void SetUpdate(bool state) {
        chunkObj.update = state;
    }
    
    public ChunkObject GetObj() {
        return chunkObj;
    }
}
