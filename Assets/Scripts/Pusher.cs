using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    //スタートポジションを記憶
    Vector3 startPosition;

    public float amplitude;
    public float speed;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // 変位を計算
        //時間の経過に対して常に0～1を滑らかに繰り返す　nomalizeすると、0と1だけ繰り返す
        float z = amplitude * Mathf.Sin(Time.time * speed);

        // zを変位させたポジションに再設定
        //もともとあったポジションに奥行きだけ押し引きさせたい
        transform.localPosition = startPosition + new Vector3(0, 0, z);
    }
}
