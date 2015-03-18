using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBGGen : MonoBehaviour {

    public int threadCount = 10;

    private List<BackgroundThread> generators = new List<BackgroundThread>();

    // Use this for initialization
    void Start() {
        Debug.Log("Testing BGGen");
        for (int i = 0; i < threadCount; i++)
            generators.Add(BackgroundSleeper.Go(Random.Range(1000, 10000)));
    }

    void Update() {
        foreach (BackgroundSleeper generator in generators) {
            if (generator.Update()) {
                Debug.Log("Thread finished with: " + generator.data);
            }
        }
        generators.RemoveAll(generator => generator.isDone);
    }
}
