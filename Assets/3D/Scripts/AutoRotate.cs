using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

    public float speed = 30;
    void Start()
    {

    }
    void Update()
    {
        transform.Rotate(new Vector3(0.5f,0.3f, 0.5f) * speed * Time.deltaTime);

    }

}
