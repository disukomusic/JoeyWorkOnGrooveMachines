using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Animator>().Play("FadeIn");
    }
}