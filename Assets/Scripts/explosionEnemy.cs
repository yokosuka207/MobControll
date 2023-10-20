using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class explosionEnemy : MonoBehaviour
{
    public event Action OnDestoryed;
    public float eruptionForce = 10.0f; // 噴火の力
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小のスケール
    public Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f); // 最大のスケール
    public float shrinkDuration = 1.0f; // 縮小にかかる時間
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent <Rigidbody>();

        // ランダムなサイズを設定
        Vector3 randomScale = new Vector3(UnityEngine.Random.Range(minScale.x, maxScale.x), UnityEngine.Random.Range(minScale.y, maxScale.y), UnityEngine.Random.Range(minScale.z, maxScale.z));
        transform.localScale = randomScale;

        // ランダムな方向に力を加える
        Vector3 eruptionDirection = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(0.5f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
        eruptionDirection.Normalize(); // ベクトルの正規化
        rb.AddForce(eruptionDirection * eruptionForce, ForceMode.Impulse);

        // 一定時間後に縮小と消滅を開始
        Invoke("ShrinkAndDestroy", shrinkDuration);
    }

    void ShrinkAndDestroy()
    {
        // オブジェクトを縮小
        StartCoroutine(ShrinkObject());

        // 一定時間後にオブジェクトを破棄
        Destroy(gameObject, shrinkDuration);
    }

    IEnumerator ShrinkObject()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0;

        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    void OnDestory()
    {
        OnDestoryed?.Invoke();
    }
}

