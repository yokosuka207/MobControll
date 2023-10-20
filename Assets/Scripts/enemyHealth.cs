using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private int bulletNumber;
    [SerializeField] bool isBulletBool;
    private textEnemyBase textEnemy;
    public event System.Action OnDestroy;

    // 弾
    private GameObject bulleObjChild;
    private int bulletCount = 0;

    // 揺れ
    private Vector3 initialPosition;
    private bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        Transform child = gameObject.transform.GetChild(0);
        textEnemy = child.GetComponent<textEnemyBase>();

        if (isBulletBool)
        {
            // オブジェクトを生成関数呼び出し
            InvokeRepeating("createBullet", 0.0f, 0.1f);
        }

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.05f;
            transform.position = initialPosition + randomOffset;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartShake();
            textEnemy.number--;

            if (textEnemy.number <= 0)
            {
                OnDestroy?.Invoke();
                Destroy(gameObject);

                for (int i = 0; i < 7; i++)
                {
                    Instantiate(explosionObj, initialPosition, Quaternion.identity);
                }
            }
        }

        if (collision.gameObject.CompareTag("SpecialBullet"))
        {
            StartShake();
            textEnemy.number -= 2;

            if (textEnemy.number <= 0)
            {
                OnDestroy?.Invoke();
                Destroy(gameObject);

                for (int i = 0; i < 7; i++)
                {
                    Instantiate(explosionObj, initialPosition, Quaternion.identity);
                }
            }
        }
    }

    private void createBullet()
    {
        if (bulletCount < bulletNumber)
        {
            float randomX = Random.Range(-1.0f, 1.0f); // -1.5から1.5の範囲でランダムなX座標を生成
            Vector3 spawnPosition = new Vector3(randomX, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 1.0f);
            bulleObjChild = Instantiate(bulletObj, spawnPosition, Quaternion.identity);
            bulleObjChild.transform.parent = null;
            bulletCount++;
        }
        else
        {
            CancelInvoke();
        }
    }

    void StartShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            Invoke("StopShake", 0.05f);
        }
    }
    void StopShake()
    {
        isShaking = false;
        transform.position = initialPosition;
    }
}
