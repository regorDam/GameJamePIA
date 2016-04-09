using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour
{


    public float gravity = -10f;

    public void Attract(Transform body)
    {
        Attract(body, gravity ,false,false);
    }

    public void Attract(Transform body, float customGravity,bool dontAffectForce, bool dontAffectRot)
    {
        Vector3 targetDir = (body.position - transform.position).normalized;

        Vector3 bodyUp = body.up;
        Quaternion targetRotation = Quaternion.identity;
        if (!dontAffectForce)
        {
            body.GetComponent<Rigidbody>().AddForce(targetDir * customGravity);
        }
        if(!dontAffectRot)
            targetRotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;

        body.rotation = Quaternion.Slerp(body.rotation, targetRotation,1);
    }
}