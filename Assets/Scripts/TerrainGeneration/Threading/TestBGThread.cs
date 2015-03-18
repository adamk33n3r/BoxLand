using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class TestBGThread : MonoBehaviour {

    // Use this for initialization
    void Start() {
//        new BackgroundThread(generateMesh, generateMeshCallback).Start(5);
    }
    
    static void generateMesh(object sender, DoWorkEventArgs e) {
        Debug.Log("Worker: Got " + e.Argument);
        Debug.Log("Worker: Adding 3");
        e.Result = (int)e.Argument + 3;
    }
    
    static void generateMeshCallback(object sender, RunWorkerCompletedEventArgs e) {
        Debug.Log("Callback: Result is " + (int)e.Result);
    }
}
