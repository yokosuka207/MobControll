using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    [SerializeField] private GameObject bulletObj; // 生成するオブジェクト
    [SerializeField] private GameObject specialBulletObj; // 生成するオブジェクト
    private enemyHealth collidableObject;
    private GameObject bulleObjChild;
    private Vector3 playerDirection;
    private Vector2 startTouchPos;
    private float moveSpeed = 7.0f;
    private float cooldownTime = 1.0f; // クールダウン時間（秒
    private float lastShotTime = 0.5f;
    private int shotNumber = 0;
    private bool isTouch = false;
    public bool isEnemyBaseDestory = false;
    public bool isPlayerDeath = false;
    private bool isShot = true;

    private Transform nextObj;
    private GameObject lineObj;
    public bool isNextMove = false;
    private bool isNextDirection = false;
    private bool hasArrived = false;
    public bool isLastEnemy = false;
    [SerializeField] private int stageCount = 0;
    private int count = 0;

    Coroutine coroutine;

    [SerializeField] GameObject[] fieldObj;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemyBaseDestory)
        {
            if (!isPlayerDeath)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerDirection = transform.forward;

                    startTouchPos = Input.mousePosition;
                    if (!isShot)
                    {
                        StartCoroutine(ShootWithCooldown());
                    }
                    isTouch = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isTouch = false;
                    isShot = false;


                    if (shotNumber >= 20)
                    {
                        createSpecialBullet();
                    }

                }

                if (isTouch)
                {
                    Vector2 nowTouchPos = (Vector2)Input.mousePosition - startTouchPos;

                    if (Mathf.Abs(nowTouchPos.x) > 4)
                    {
                        float moveX = nowTouchPos.x > 0 ? 1.0f : -1.0f;
                        transform.position += transform.right * moveX * moveSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                isShot = false;
            }
        }
        else
        {
            if (!hasArrived)
            {
                Vector3 moveDirection = (new Vector3(lineObj.transform.position.x, transform.position.y, lineObj.transform.position.z)
                    - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                float distanceToTarget = Vector3.Distance(transform.position, lineObj.transform.position);

                if (distanceToTarget <= 0.5)
                {
                    hasArrived = true;
                }
            }

            else
            {
                if (!isLastEnemy)
                {
                    StartCoroutine(moveWait());
                    if(!isNextMove)
                    {
                        isNextMove = true;
                    }

                    if (isNextMove)
                    {

                        if (isNextDirection)
                        {
                            Vector3 targetDirection = nextObj.parent.position - transform.position;
                            Quaternion rotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0.0f, targetDirection.z));
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f * Time.deltaTime);
                        }

                        transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    }
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NextStage"))
        {
            nextObj = other.transform;
            isNextDirection = true;
        }

        if (other.CompareTag("Line"))
        {
            collidableObject = FindObjectOfType<enemyHealth>();
            if (collidableObject != null)
            {
                collidableObject.OnDestroy += HandleObjectDestroyed;
            }

            lineObj = other.gameObject;
            transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            transform.rotation = other.transform.rotation;
            isNextDirection = false;
            isNextMove = false;
            hasArrived = false;
            isEnemyBaseDestory = false;
            count++;
        }
    }

    private void createBullet()
    {
        bulleObjChild = Instantiate(bulletObj, gameObject.transform.position, Quaternion.identity);
        bulleObjChild.transform.parent = null;
        bulleObjChild.transform.forward = playerDirection;

        Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 初速度を設定
            rb.velocity = playerDirection * 10;
        }
        StartCoroutine(IniVelDelay());
        shotNumber++;
    }
    private void createSpecialBullet()
    {
        bulleObjChild = Instantiate(specialBulletObj, this.gameObject.transform.position, Quaternion.identity);
        bulleObjChild.transform.parent = null;
        bulleObjChild.transform.forward = playerDirection;

        Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 初速度を設定
            rb.velocity = playerDirection * 10.0f;
        }

        StartCoroutine(IniVelDelay());
        shotNumber = 0;
    }

    public void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            if (collidableObject != null)
            {
                collidableObject.OnDestroy -= HandleObjectDestroyed;
                collidableObject = null;
            }

            StartCoroutine(WaitForFrames());
        }
    }
    private enemyHealth FindNewCollidableObject()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("EnemyBase");

        foreach (var obj in objectsWithTag)
        {
            enemyHealth healthComponent = obj.GetComponent<enemyHealth>();
            if (healthComponent != null && obj.activeSelf)
            {
                return healthComponent;
            }
        }        
        return null;
    }

    private IEnumerator WaitForFrames()
    {
        yield return null;

        collidableObject = FindNewCollidableObject();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
        else
        {
            if(count == stageCount)
            {
                isShot = false;
                isLastEnemy = true;
            }
            isEnemyBaseDestory = true;
        }
    }

    IEnumerator ShootWithCooldown()
    {
        isShot = true;

        while (isShot)
        {
            if (Time.time - lastShotTime >= cooldownTime)
            {
                createBullet();
                lastShotTime = Time.time;

                if (isEnemyBaseDestory)
                {
                    isShot = false;
                    yield break;
                }
            }
            yield return null;
        }
    }

    IEnumerator moveWait()
    {
        if (!isNextMove)
        {
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator IniVelDelay()
    {
        yield return new WaitForSeconds(0.4f);

        if (bulleObjChild != null)
        {
            Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 初速度を設定
                rb.velocity = playerDirection;
            }
        }
    }

    public int GetShotNumber()
    {
        return shotNumber;
    }

    public bool GetLastEnemy()
    {
        return isLastEnemy;
    }

    public int GetStageCount()
    {
        return stageCount;
    }
}
