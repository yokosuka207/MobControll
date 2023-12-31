using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public event System.Action OnDestroyed;
    public GameObject objectToDeactivate;

    private void Start()
    {
        objectToDeactivate.SetActive(false);
        StartCoroutine(DelayedDestroy());
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.right * 3 * Time.deltaTime;
    }

    // 衝突時に呼ばれる関数
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //objectToDeactivate.SetActive(false);
            //StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2.0f); // 遅延させたい秒数を設定
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
