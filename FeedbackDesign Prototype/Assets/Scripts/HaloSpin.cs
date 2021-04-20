using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloSpin : MonoBehaviour
{
    public float spinAmount;
    public GameObject ball;
    void Start()
    {
        ball = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ball.transform.Rotate(new Vector3(0, 0, spinAmount));
    }
}
