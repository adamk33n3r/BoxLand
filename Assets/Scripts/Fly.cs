using UnityEngine;
using System.Collections;

public class Fly : MonoBehaviour {

    public float speed = .5f;
    public float _speed;

    // Use this for initialization
    void Start() {
        _speed = speed;
    }
	
    // Update is called once per frame
    void Update() {
        if (!Input.GetKey(KeyCode.LeftShift))
            _speed = speed;
        else
            _speed = speed * 4;
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        float y = 0;
        if (Input.GetKey(KeyCode.Q))
            y = -1;
        else if (Input.GetKey(KeyCode.E))
            y = 1;
        else
            y = 0;
        Vector3 dir = new Vector3(x, y, z);
        if (dir != Vector3.zero) {
            dir *= _speed;
            dir = transform.rotation * dir;
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z + dir.z);
        }
    }
}
