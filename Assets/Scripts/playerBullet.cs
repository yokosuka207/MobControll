using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] private Transform[] enemyBase;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime = 2.0f;
    public playerBullet scPlayer;
    private List<string> objectsToAvoid = new List<string>();
    private string objectName;
    private enemyHealth collidableObject;
    private Transform currentTarget;
    private Vector3 originalVelocity;
    private Vector3 spawnPosition;
    private Vector3 bulletColPosition;
    private Vector3 bulletDirection;
    private bool isDirection = false;
    private bool isEnabled = true;
    private int twice;

    // �F�ω�
    private Vector3 initialPosition;
    private Color   initialColor;
    private Renderer renderer;
    private bool isMove = true;

    private void Start()
    {
        //DisableFunctionTemporarily(0.3f);

        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;

        bulletDirection = transform.forward;

        StartCoroutine(GetVelosity());

        // BaseDestroy�擾
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
                // �^�[�Q�b�g�̕���������
                Vector3 targetDirection = currentTarget.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0.0f, targetDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f * Time.deltaTime);

                // �O�i
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += bulletDirection * moveSpeed * Time.deltaTime; // �O�i
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
           
            objectName = other.gameObject.name;

            if (!objectsToAvoid.Contains(objectName))
            {
                createTwice();
                objectsToAvoid.Add(objectName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //string collidingObjectName = other.gameObject.name;

        //if (objectsToAvoid.Contains(collidingObjectName))
        //{
        //    createTwice();
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DirectionCollision"))
        {
            FindClosestTarget();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBase"))
            colorChange();
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("���肶���");
            colorChange();
        }
        if (collision.gameObject.CompareTag("BulletBackCollision"))
            bulletColPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("BulletBackCollision"))
            transform.position = bulletColPosition;
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            if (collidableObject != null)
            {
                collidableObject.OnDestroy -= HandleObjectDestroyed;
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
                isDirection = false;
                Invoke("bulletAllDestory", 1.0f);
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

            Debug.Log("kannsuu");

            StartCoroutine(ResetColorAfterDelay());
        }
    }

    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // 0.5�b�҂�
        renderer.material.color = initialColor; // ���̐F�ɖ߂�
        Destroy(gameObject, 0.1f);

        Debug.Log("������");
    }
    private IEnumerator GetVelosity()
    {
        yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            //originalVelocity = rb.velocity;
        }
    }

    private void bulletAllDestory()
    {
        // �^�O�����I�u�W�F�N�g������
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("Bullet");
        GameObject[] objectsSBulletDestroy = GameObject.FindGameObjectsWithTag("SpecialBullet");

        // �^�O�����I�u�W�F�N�g��j��
        foreach (GameObject nobj in objectsBulletDestroy)
        {
            Destroy(nobj);
        }
        // �^�O�����I�u�W�F�N�g��j��
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
                    spawnPosition = transform.right * i * 0.1f;
                else
                    spawnPosition = transform.right * i * -0.1f;

                if(gameObject.CompareTag("SpecialBullet"))
                {
                    if (j == 0)
                        spawnPosition = transform.right * i * 0.2f;
                    else
                        spawnPosition = transform.right * i * -0.2f;
                }

                playerBullet p = null;
                p = Instantiate(scPlayer, gameObject.transform.position + spawnPosition, Quaternion.identity);
                p.gameObject.transform.forward = bulletDirection;
                p.objectsToAvoid.Add(objectName);
            }
        }
    }

    // ���̊֐�����莞�Ԗ����ɂ���
    private void DisableFunctionTemporarily(float waitTime)
    {
        if (isEnabled)
        {
            isEnabled = false; // �֐��𖳌��ɂ���
            StartCoroutine(EnableFunctionAfterDelay(waitTime)); // ��莞�Ԍ�Ɋ֐����ēx�L���ɂ���
        }
    }

    // ��莞�Ԍ�Ɋ֐����ēx�L���ɂ���
    private IEnumerator EnableFunctionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isEnabled = true; // �֐����ēx�L���ɂ���
    }
}
