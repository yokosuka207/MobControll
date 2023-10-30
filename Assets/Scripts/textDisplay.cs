using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int twice;
    [SerializeField] private float waitTime = 2.0f;
    [SerializeField] private bool isMove = false;
    private enemyHealth collidableObject;
    private bool isFall = false;

    [SerializeField] private float moveDistance = 3.0f;  // �ړ����鋗��
    private float moveSpeed = 2.0f;    // �ړ����x

    private Vector3 startPosition;
    private Vector3 endPosition;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "�~" + twice.ToString();

        startPosition = transform.position;
        endPosition = startPosition + transform.right * moveDistance; // �E�Ɉړ�

        // BaseDestroy�擾
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
    }

    private void Update()
    {
        if(isFall)
        {
            gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f) * 3 * Time.deltaTime;

            if (gameObject.transform.position.y < -0.6f)
            {
                Destroy(gameObject);
            }
        }

        if(isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

            // �ړI�n�ɓ��B�����甽�Ε����Ɉړ�
            if (transform.position == endPosition)
            {
                SwapDirection();
            }
        }
    }

    void SwapDirection()
    {
        direction *= -1;
        if (direction == 1)
        {
            endPosition = startPosition;
        }
        else
        {
            endPosition = startPosition + transform.right * moveDistance;
        }
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
            StartCoroutine(destoryCoroutine());
        }
    }

    private IEnumerator destoryCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        isFall = true;
    }

    public int GetTwice()
    {
        return twice;
    }
}
