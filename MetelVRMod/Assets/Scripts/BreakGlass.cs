using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    [SerializeField] AudioSource AudioSrc;
    [SerializeField] AudioClip Clip;
    [SerializeField] GameObject[] ForSetRigidbody;

    Vector3 Pos;

    private void Start ()
    {
        if (AudioSrc == null) AudioSrc = GetComponent<AudioSource> ();
        Pos = transform.position;
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag != "Soft" && collision.transform.parent != transform && (Pos - transform.position).magnitude > 1) { // Твердая поверхность, разбиваем стеклянное изделие
            Break ();
        }
    }

    void Break ()
    {
        AudioSrc.clip = Clip;
        AudioSrc.Play ();
        GetComponent<SphereCollider> ().enabled = false;
        GetComponent<Rigidbody> ().isKinematic = true;
        foreach (GameObject ForSetRigidbodyTemp in ForSetRigidbody) {
            Rigidbody Rb = ForSetRigidbodyTemp.AddComponent(typeof(Rigidbody)) as Rigidbody;
            Rb.velocity = GetComponent<Rigidbody> ().velocity;
        }
        if (!Noise.instance.makedNoise) Noise.instance.MakeNoise ();

        if (GetComponent<Interactive> () != null) {
            GetComponent<Interactive> ().enabled = true;
            GetComponent<Interactive> ().PowerUse ();
        }
        foreach (GameObject ForSetRigidbodyTemp in ForSetRigidbody) {
            if (ForSetRigidbodyTemp.GetComponent<Interactive> () != null) {
                ForSetRigidbodyTemp.GetComponent<Interactive> ().enabled = true;
            }
        }
    }
}
