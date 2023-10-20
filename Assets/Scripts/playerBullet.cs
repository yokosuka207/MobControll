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

    // �F�ω�
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
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // �^�[�Q�b�g�����݂��Ȃ��ꍇ�AZ���Ɍ����Đi��
                transform.rotation = Quaternion.Euler(0, 0, 0); // Z���Ɍ���
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // �O�i
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
            Debug.Log("����");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("BulletBackCollision"))
        {
            transform.position = bulletColPosition;
            Debug.Log("����");
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
        yield return new WaitForSeconds(0.4f); // 0.5�b�҂�
        renderer.material.color = initialColor; // ���̐F�ɖ߂�
        Destroy(gameObject, 0.1f);
    }
    private IEnumerator GetVelosity()
    {
        yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            originalVelocity = rb.velocity;
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
                    spawnPosition = new Vector3(i * 0.5f, 0, 0);
                else
                    spawnPosition = new Vector3(i * -0.5f, 0, 0);

                Instantiate(gameObject, gameObject.transform.position + spawnPosition, Quaternion.identity);
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
