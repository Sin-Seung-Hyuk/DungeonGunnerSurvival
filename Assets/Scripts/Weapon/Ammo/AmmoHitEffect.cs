using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHitEffect : MonoBehaviour
{
    private ParticleSystem hitEffect;

    private void Awake()
    {
        hitEffect = GetComponent<ParticleSystem>();
    }
}
