﻿using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour {

    public float speed = .2f;

    Vector2 rot;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
                Terrain.SetBlock(hit, new BlockAir());
            }
        }
        
        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 3,
            rot.y + Input.GetAxis("Mouse Y") * 3);
        
        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);
        
        transform.position += transform.forward * Input.GetAxis("Vertical") * speed;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed;
    }
}
