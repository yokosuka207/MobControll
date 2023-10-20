using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private enemyHealth collidableObject;
    private Vector3 originalVelocity;
    private bool isEnabled = true;
    private int twice;

    private Vector3 initialPosition;
    private Color initialColor;
    private Renderer renderer;


    private void Start()
    {
        DisableFunctionTemporarily(0.5f);

        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalVelocity = rb.velocity;
        }

        // BaseDestroy取得
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += -Vector3.forward * moveSpeed * Time.deltaTime;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = originalVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            textDisplay _textDisplay = other.GetComponent<textDisplay>();
            twice = _textDisplay.GetTwice();

            createTwice();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
            colorChange();
        if (collision.gameObject.CompareTag("SpecialBullet"))
            colorChange();
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            Invoke("bulletAllDestory", 0);
        }
    }

    private void createTwice()
    {
        if (isEnabled)
        {
            for (int i = 1; i < twice; i++)
            {
                Vector3 spawnPosition;
                int j = i % 2;
                if (j == 0)
                    spawnPosition = new Vector3(i * 0.3f, 0, 0);
                else
                    spawnPosition = new Vector3(i * -0.3f, 0, 0);

                GameObject enemyBulletObj = Instantiate(gameObject, gameObject.transform.position + spawnPosition, Quaternion.identity);
                enemyBulletObj.transform.Rotate(Vector3.up, 90f);
            }
        }
    }

    private void colorChange()
    {
        if (collidableObject != null)
        {
            collidableObject.OnDestroy -= HandleObjectDestroyed;

            initialPosition = gameObject.transform.position;

            Color fadedColor = initialColor;
            fadedColor.r = 1.0f;
            renderer.material.color = fadedColor;

            StartCoroutine(ResetColorAfterDelay());
        }
    }

    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // 0.5秒待つ
        renderer.material.color = initialColor; // 元の色に戻す
        Destroy(gameObject, 0.2f);
    }

    private void bulletAllDestory()
    {
        // タグを持つオブジェクトを検索
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("EnemyBullet");

        // タグを持つオブジェクトを破棄
        foreach (GameObject obj in objectsBulletDestroy)
        {
            Destroy(obj);
        }
    }

    // この関数を一定時間無効にする
    private void DisableFunctionTemporarily(float waitTime)
    {
        if (isEnabled)
        {
            isEnabled = false; // 関数を無効にする
            StartCoroutine(EnableFunctionAfterDelay(waitTime)); // 一定時間後に関数を再度有効にする
        }
    }

    // 一定時間後に関数を再度有効にする
    private IEnumerator EnableFunctionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isEnabled = true; // 関数を再度有効にする
    }
}
