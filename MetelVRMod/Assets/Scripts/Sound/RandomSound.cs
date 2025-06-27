using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] float MinDelay;
    [SerializeField] float MaxDelay;
    [SerializeField] AudioClip[] Clips;
    [SerializeField] GameObject SoundPrefab;

    float Waiting = 0f;
    float NextPlay = 0f;

    private void Start ()
    {
        NextPlay = Random.Range (MinDelay, MaxDelay);
    }

    void Update ()
    {
        Waiting += Time.deltaTime;
        if (Waiting >= NextPlay) {
            Waiting = 0f;
            NextPlay = Random.Range (MinDelay, MaxDelay);
            Instantiate (SoundPrefab, transform).GetComponent<NewSound> ().PlaySound (Clips[Random.Range (0, Clips.Length)]);
        }
    }
}
