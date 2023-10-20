using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    [SerializeField] private GameObject bulletObj; // 生成するオブジェクト
    [SerializeField] private GameObject specialBulletObj; // 生成するオブジェクト
    private enemyHealth collidableObject;
    private GameObject bulleObjChild;
    private Vector2 startTouchPos;
    private float moveSpeed = 7.0f;
    private float cooldownTime = 0.5f; // クールダウン時間（秒
    private float lastShotTime = 0.5f;
    private int shotNumber = 0;
    private bool isTouch = false;
    private bool isEnemyBaseDestory = false;
    private bool isShot = true;

    private void Start()
    {
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
        if (!isEnemyBaseDestory)
        {
            if (Input.GetMouseButtonDown(0))
            {
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

                if (IsInvoking())
                {
                    CancelInvoke();
                    if (shotNumber >= 20)
                    {
                        createSpecialBullet();
                    }
                }
            }

            if (isTouch)
            {
                Vector2 nowTouchPos = (Vector2)Input.mousePosition - startTouchPos;

                if (Mathf.Abs(nowTouchPos.x) > 4)
                {
                    float moveX = nowTouchPos.x > 0 ? 1.0f : -1.0f;
                    transform.position += Vector3.right * moveX * moveSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            if (IsInvoking())
            {
                CancelInvoke();
            }
        }
    }

    private void createBullet()
    {
        bulleObjChild = Instantiate(bulletObj, gameObject.transform.position + new Vector3(0.0f, 0, 1.0f), Quaternion.identity);
        bulleObjChild.transform.parent = null;

        Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 初速度を設定
            rb.velocity = new Vector3(0, 0, 10.0f);
        }
        StartCoroutine(IniVelDelay());
        shotNumber++;
    }
    private void createSpecialBullet()
    {
        bulleObjChild = Instantiate(specialBulletObj, this.gameObject.transform.position + new Vector3(0.0f, 0, 1.0f), Quaternion.identity);
        bulleObjChild.transform.parent = null;

        Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 初速度を設定
            rb.velocity = new Vector3(0, 0, 10.0f);
        }
        StartCoroutine(IniVelDelay());
        shotNumber = 0;
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
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

    IEnumerator IniVelDelay()
    {
        yield return new WaitForSeconds(0.2f);

        if (bulleObjChild != null)
        {
            Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 初速度を設定
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    private void IniVel()
    {
        Rigidbody rb = bulleObjChild.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 初速度を設定
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public int GetShotNumber()
    {
        return shotNumber;
    }
}
