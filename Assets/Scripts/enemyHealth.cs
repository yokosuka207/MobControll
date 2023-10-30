using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private GameObject SPbulletObj;
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private int bulletNumber;
    [SerializeField] public bool isBulletBool;
    [SerializeField] public bool isSPBulletBool;
    [SerializeField] public float waitTime = 3.0f;
    private playerMove scPlayer;
    private textEnemyBase textEnemy;
    private bool isLastEnemyBase;
    private int remainingEnemies;
    public event System.Action OnDestroy;

    // ’e
    private GameObject bulleObjChild;

    // —h‚ê
    private Vector3 initialPosition;
    private bool isShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        Transform child = gameObject.transform.GetChild(0);
        textEnemy = child.GetComponent<textEnemyBase>();

        GameObject obj = GameObject.Find("Player");
        scPlayer = obj.GetComponent<playerMove>();

        initialPosition = transform.position;

        if (isBulletBool)
        {
            InvokeRepeating("createBullet", 1.0f, waitTime);
        }
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
        if (collision.gameObject.CompareTag("LastStage"))
        {
            isLastEnemyBase = true;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartShake();
            textEnemy.number--;

            if (textEnemy.number == 0)
            {
                if (isLastEnemyBase)
                {
                    if (!IsLastEnemy())
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        scPlayer.isLastEnemy = true;
                        scPlayer.isEnemyBaseDestory = true;
                        OnDestroy?.Invoke();
                    }
                }
                else
                {
                    Destroy(gameObject);
                    OnDestroy?.Invoke();

                    for (int i = 0; i < 7; i++)
                    {
                        Instantiate(explosionObj, initialPosition, Quaternion.identity);
                    }
                }
                
            }
        }

        if (collision.gameObject.CompareTag("SpecialBullet"))
        {
            StartShake();
            textEnemy.number -= 2;

            if (textEnemy.number == 0)
            {
                if (isLastEnemyBase)
                {
                    if (!IsLastEnemy())
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        scPlayer.isLastEnemy = true;
                        scPlayer.isEnemyBaseDestory = true;
                        OnDestroy?.Invoke();
                    }
                }

                else
                {
                    Destroy(gameObject);
                    OnDestroy?.Invoke();

                    for (int i = 0; i < 7; i++)
                    {
                        Instantiate(explosionObj, initialPosition, Quaternion.identity);
                    }
                }

            }
        }
    }

    private void createBullet()
    {
        for (int i = 0; i < bulletNumber; i++) 
        {
            float randomX = Random.Range(-1.0f, 1.0f); // -1.5‚©‚ç1.5‚Ì”ÍˆÍ‚Åƒ‰ƒ“ƒ_ƒ€‚ÈXÀ•W‚ð¶¬
            Vector3 spawnPosition = transform.right * randomX;
            Vector3 pos = new Vector3(0.0f, -0.7f, 0.0f);
            bulleObjChild = Instantiate(bulletObj, transform.position + spawnPosition + pos, Quaternion.identity);
            bulleObjChild.transform.parent = null;

            if(isSPBulletBool && (i == bulletNumber / 2))
            {
                float s_randomX = Random.Range(-1.0f, 1.0f); // -1.5‚©‚ç1.5‚Ì”ÍˆÍ‚Åƒ‰ƒ“ƒ_ƒ€‚ÈXÀ•W‚ð¶¬
                Vector3 s_spawnPosition = transform.right * s_randomX;
                Vector3 s_pos = new Vector3(0.0f, -0.5f, 0.0f);
                bulleObjChild = Instantiate(SPbulletObj, transform.position + s_spawnPosition + s_pos, Quaternion.identity);
                bulleObjChild.transform.parent = null;
            }
        }
    }

    private bool IsLastEnemy()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("EnemyBase");
        return objectsWithTag.Length == 1;
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
