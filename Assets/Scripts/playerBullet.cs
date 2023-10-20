using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] private Transform[] enemyBase;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime = 2.0f;
    private enemyHealth collidableObject;
    private Transform currentTarget;
    private Vector3 originalVelocity;
    private Vector3 spawnPosition;
    private Vector3 bulletColPosition;
    private bool isDirection = false;
    private bool isEnabled = true;
    private int twice;

    // 色変化
    private Vector3 initialPosition;
    private Color   initialColor;
    private Renderer renderer;
    private bool isMove = true;

    private void Start()
    {
        DisableFunctionTemporarily(0.5f);

        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;

        transform.position += new Vector3(0.0f, 0, -0.3f);

        StartCoroutine(GetVelosity());

        // BaseDestroy取得
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMove)
        {
            if (currentTarget != null && isDirection)
            {
                // ターゲットの方向を向く
                Vector3 targetDirection = currentTarget.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0.0f, targetDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f * Time.deltaTime);

                // 前進
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // ターゲットが存在しない場合、Z軸に向いて進む
                transform.rotation = Quaternion.Euler(0, 0, 0); // Z軸に向く
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // 前進
            }
        }
        else if(!isMove)
        {
            gameObject.transform.position = initialPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Gate"))
        {
            textDisplay _textDisplay = other.GetComponent<textDisplay>();
            twice = _textDisplay.GetTwice();
            createTwice();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DirectionCollision"))
            FindClosestTarget();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBase"))
            colorChange();
        if (collision.gameObject.CompareTag("EnemyBullet"))
            colorChange();
        if (collision.gameObject.CompareTag("BulletBackCollision"))
        {
            bulletColPosition = transform.position;
            Debug.Log("あた");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("BulletBackCollision"))
        {
            transform.position = bulletColPosition;
            Debug.Log("あた");
        }
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            isDirection = false;
            Invoke("bulletAllDestory", 2.0f);
        }
    }

    private void colorChange()
    {
        if (collidableObject != null)
        {
            collidableObject.OnDestroy -= HandleObjectDestroyed;

            initialPosition = gameObject.transform.position;
            isMove = false;

            Color fadedColor = initialColor;
            fadedColor.g = 1.0f;
            renderer.material.color = fadedColor;

            StartCoroutine(ResetColorAfterDelay());
        }
    }

    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(0.4f); // 0.5秒待つ
        renderer.material.color = initialColor; // 元の色に戻す
        Destroy(gameObject, 0.1f);
    }
    private IEnumerator GetVelosity()
    {
        yield return new WaitForSeconds(0.5f); // 0.5秒待つ
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalVelocity = rb.velocity;
        }
    }

    private void bulletAllDestory()
    {
        // タグを持つオブジェクトを検索
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("Bullet");
        GameObject[] objectsSBulletDestroy = GameObject.FindGameObjectsWithTag("SpecialBullet");

        // タグを持つオブジェクトを破棄
        foreach (GameObject nobj in objectsBulletDestroy)
        {
            Destroy(nobj);
        }
        // タグを持つオブジェクトを破棄
        foreach (GameObject sobj in objectsSBulletDestroy)
        {
            Destroy(sobj);
        }
    }

    void FindClosestTarget()
    {
        isDirection = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = originalVelocity;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag("EnemyBase");

        if (targets.Length > 0)
        {
            Transform closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            currentTarget = closestTarget;
        }
    }

    private void createTwice()
    {
        if (isEnabled)
        {
            for (int i = 1; i < twice; i++)
            {
                int j = i % 2;
                if (j == 0)
                    spawnPosition = new Vector3(i * 0.5f, 0, 0);
                else
                    spawnPosition = new Vector3(i * -0.5f, 0, 0);

                Instantiate(gameObject, gameObject.transform.position + spawnPosition, Quaternion.identity);
            }
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
