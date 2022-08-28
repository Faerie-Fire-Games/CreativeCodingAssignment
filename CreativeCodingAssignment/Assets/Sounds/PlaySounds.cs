using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioSource sound1, sound2;

    [ContextMenu("StartSound")]
    public void Start()
    {
        sound1.Play();
        sound2.Play();
        sound2.loop = true;
        sound1.loop = true;
    }
}
