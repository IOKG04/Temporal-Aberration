using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TAServer : MonoBehaviour{
    public float timeScale, distanceScale;

    public float LocalTimeScale(Vector2 position){
        return 1 + Mathf.Pow(((Vector2)transform.position - position).magnitude * distanceScale, 0.5f) * timeScale;
    }
}
