using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class ChunkObject : MonoBehaviour {
    
    MeshFilter filter;
    MeshCollider coll;
    Legacy.Chunk chunk;
    public bool update = false;
    
    // Use this for initialization
    void Start() {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }
    
    // Update is called once per frame
    void Update() {
        if (update) {
            Debug.Log("Still update");
            if (!chunk.world.multithread || chunk.meshToRender != null)
                update = false;
            chunk.UpdateChunk(filter, coll);
        }
    }

    public void AttachChunk(Legacy.Chunk chunk) {
        this.chunk = chunk;
    }
}
