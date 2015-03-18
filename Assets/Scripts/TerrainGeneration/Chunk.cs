using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Chunk : MonoBehaviour {

    public World world;
    public WorldPos pos;
    public Block[ , , ] blocks = new Block[chunkSize, chunkSize, chunkSize];
    public static int chunkSize = 16;
    public bool rendered, update;

    private MeshFilter filter;
    private MeshCollider coll;

    private BackgroundMeshBuilder thread;

    // Use this for initialization
    void Start() {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }
	
    // Update is called once per frame
    void Update() {
        if (update) {
            update = false;
//            System.DateTime before = System.DateTime.Now;
//            UpdateChunk();
//            System.DateTime after = System.DateTime.Now;
            if (thread != null)
                thread.Abort();
            thread = BackgroundMeshBuilder.Go(this);
            return;
//            System.TimeSpan time = Utils.timeFunction(new System.Action(() => {
//                UpdateChunk();
//            }));
//            Debug.Log("Time to generate chunk mesh: " + time);
        } else if (thread != null && thread.Update()) {
            RenderMesh(thread.meshData);
            thread = null;
//            Debug.Log("updateing thread");
//            RenderMesh(Mesh);
        }
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
//    public void UpdateChunk() {
//        rendered = true;
//        MeshData meshData = new MeshData();
//        for (int x = 0; x < chunkSize; x++) {
//            for (int y = 0; y < chunkSize; y++) {
//                for (int z = 0; z < chunkSize; z++) {
//                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
//                }
//            }
//        }
//        RenderMesh(meshData);
//    }
    
    // Sends the calculated mesh information
    // to the mesh and collision components
    public void RenderMesh(MeshData meshData) {
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
        rendered = true;
    }
    
    public void SetBlocksUnmodified() {
        foreach (Block block in blocks) {
            block.changed = false;
        }
    }
    
    public void SetUpdate(bool state) {
        update = state;
    }
}
