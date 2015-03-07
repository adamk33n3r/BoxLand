using UnityEngine;
using System.Collections;

public class SunFollow : MonoBehaviour {

    public Transform player;

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(player.position.x, player.position.y + 30, player.position.z);
    }
}
