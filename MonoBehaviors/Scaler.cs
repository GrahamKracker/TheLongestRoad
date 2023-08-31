using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TheLongestRoad.MonoBehaviors;

[RegisterTypeInIl2Cpp(false)]
public class Scaler : MonoBehaviour
{
    public Scaler(IntPtr ptr) : base(ptr)
    {
    }

    private void LateUpdate()
    {
        transform.localScale = new Vector3(Main.TowerScale, Main.TowerScale, Main.TowerScale);
    }
}