﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    const int MaxShotPower = 5; //マックスで連続投入できる数 5という設定
    const int RecoverySeconds = 3; //回復までタイム

    int shotPower = MaxShotPower; //MaxShotPowerで設定した数がそのままshotパワーになっている

    AudioSource shotSound;

    //public GameObject candyPrefab; //Instantiateで生成する対象
    public GameObject[] candyPrefabs; //Instantiateでランダム生成する対象（配列）

    public Transform candyParentTransform; //生成されたCandyの親役
    public CandyManager candyManager; //CandyManagerクラスの変数を使えるようにする


    public float shotForce; //AddForceで使うパワー
    public float shotTorque; //AddTorqueで使う回転力

    public float baseWidth; //Candyが飛んでいく位置の上限幅 "5"の幅をめがけて飛んでいく



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Screen.width:" + Screen.width);

        shotSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Shot();
        //}

        //特定のボタンが押された時にShot()メソッドを発動
        if (Input.GetButtonDown("Fire1")) Shot();
    }

    //voidじゃなくGameObjectが返ってくる
    //配列candyPrefabsの中からランダムにオブジェクトを1個取り出す
    GameObject sampleCandy()
    {
        int index = Random.Range(0, candyPrefabs.Length);
        return candyPrefabs[index];
    }

    //voidじゃなくVector3が返ってくる
    //マウスが押された位置と連動するようにBaseのどこをめがけてCandyを飛ばすか、
    //その位置を決めている
    Vector3 GetInstantiatePosition()
    {
        //ゲーム画面上のマウスのX座標を取得する
        float xx = Input.mousePosition.x;
        Debug.Log(xx);

        //Screen.width＝画面幅マウスの位置を元にどのへんに飛ばすか計算する
        float x = baseWidth * (xx / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x, 0, 0);
    }

    public void Shot()
    {
        if (candyManager.GetCandyAmount() <= 0) return; //残数0なら何もやらずにメソッド終了

        if (shotPower <= 0) return;


        //①Candyの生成 Instantiate(対象物,位置,回転)
        //GameObject candy = Instantiate(
        //    candyPrefab,
        //    transform.position,
        //    Quaternion.identity
        //    );

        GameObject candy = Instantiate(
            sampleCandy(),  //配列でランダムのキャンディーを選ぶ自作メソッド
            GetInstantiatePosition(),   //マウスの位置に応じた位置の自作メソッド
            Quaternion.identity
            );

        //生成したcandyオブジェクトの親は = candyParentTransform変数に指定したオブジェクト(Candies)
        candy.transform.parent = candyParentTransform;


        //②生成したCandyのRigidbodyを使えるようにしている
        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();
        //③生成したCandyにAddForce()メソッドをかけて飛ばしている
        //transform.forward→オブジェクトの前方
        //ForceModeを省略すると、「ForceMode.Force」が初期設定になる
        candyRigidBody.AddForce(transform.forward * shotForce);
        //④横にスピンさせる力
        candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));

        //Candyのストックを消費
        candyManager.ConsumeCandy();

        ConsumePower();

        shotSound.Play(); //AudioSourceに設置されているAudioClipを再生する
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        string label = "";
        for (int i = 0; i < shotPower; i++) label = label + "+";

        GUI.Label(new Rect(50, 65, 100, 30), label);
    }

    void ConsumePower()
    {
        shotPower--;　//パワーを1減らす
        StartCoroutine(RecoverPower());　//コルーチン（回復開始）
    }

    IEnumerator RecoverPower()
    {
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}
