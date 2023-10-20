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

        // BaseDestroy�擾
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
        yield return new WaitForSeconds(0.2f); // 0.5�b�҂�
        renderer.material.color = initialColor; // ���̐F�ɖ߂�
        Destroy(gameObject, 0.2f);
    }

    private void bulletAllDestory()
    {
        // �^�O�����I�u�W�F�N�g������
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("EnemyBullet");

        // �^�O�����I�u�W�F�N�g��j��
        foreach (GameObject obj in objectsBulletDestroy)
        {
            Destroy(obj);
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
