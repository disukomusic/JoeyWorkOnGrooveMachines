using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{
    public void PlayLevelUpEffect()
    {
        GetComponent<Animation>().Play();
    }
}
