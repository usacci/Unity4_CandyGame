using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    const int DefaultCandyAmount = 30;  //const 定数
    const int RecoverySeconds = 10;

    // 現在のキャンディのストック数
    public int candy = DefaultCandyAmount;
    // ストック回復までの残り秒数 
    int counter;

    //キャンディの使用・消費
    public void ConsumeCandy()
    {
        if (candy > 0) candy--;
    }

    //残数の数字をintで取得
    public int GetCandyAmount()
    {
        return candy;
    }

    //キャンディの補充
    public void AddCandy(int amount)
    {
        candy += amount;
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        // キャンディのストック数を表示
        string label = "Candy : " + candy;

        // 回復カウントしている時だけ秒数を表示
        if (counter > 0) label = label + " (" + counter + "s)";

        GUI.Label(new Rect(50, 50, 100, 30), label);
    }

    void Update()
    {
        // キャンディのストックがデフォルトより少なく、
        // 回復カウントをしていないときにカウントをスタートさせる
        //candu < 30 かつ conterが0以下になった場合
        if (candy < DefaultCandyAmount && counter <= 0)
        {
            //コルーチンでRecoveryCandyを実行
            StartCoroutine(RecoverCandy());
        }
    }

    //コルーチン実行用メソッド
    //数秒待つ必要がある場合に使う
    IEnumerator RecoverCandy()
    {
        counter = RecoverySeconds;

        // 1秒ずつカウントを進める
        //カウンターが0未満の間繰り返し
        while (counter > 0)
        {
            //1秒待つ
            yield return new WaitForSeconds(1.0f);
            //1減らす
            counter--;
        }
        //conterが0になったらcandyを2個増やす
        candy++;
    }
}