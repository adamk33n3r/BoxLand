using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBGGen : MonoBehaviour {

    private List<BackgroundGenerator> generators = new List<BackgroundGenerator>();

    // Use this for initialization
    void Start() {
        for (int i = 0; i < 10; i++)
            generators.Add(new BackgroundGenerator(i));
    }
}
