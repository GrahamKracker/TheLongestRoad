using System;
using MelonLoader;
using UnityEngine;

namespace TheLongestRoad.MonoBehaviors;

[RegisterTypeInIl2Cpp(false)]
public class WindmillSpin : MonoBehaviour
{
    public WindmillSpin(IntPtr ptr) : base(ptr) { }
    
    public float speed = 15f;
    
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
