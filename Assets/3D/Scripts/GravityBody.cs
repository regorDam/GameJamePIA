using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{

    GravityAttractor planet;
    Rigidbody rgb;

    public bool dontAffectForce;

    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        rgb = GetComponent<Rigidbody>();
        rgb.useGravity = false;
        rgb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    void FixedUpdate()
    {
        if(!dontAffectForce)
            planet.Attract(transform);
        else
            planet.Attract(transform);
    }

}
