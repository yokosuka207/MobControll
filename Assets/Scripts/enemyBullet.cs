using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private List<string> objectsToAvoid = new List<string>();
    public enemyBullet scEnemy;
    private string objectName;
    private enemyHealth collidableObject;
    private playerMove scPlayer;
    private Vector3 originalVelocity;
    private Vector3 bulletDirection;
    private bool isEnabled = true;
    private bool isLoose = false;
    private int twice;

    private Vector3 initialPosition;
    private Color initialColor;
    private Renderer renderer;


    private void Start()
    {
        //DisableFunctionTemporarily(0.5f);

        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;

        GameObject obj = GameObject.Find("Player");
        scPlayer = obj.GetComponent<playerMove>();

        // BaseDestroy取得
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;

            bulletDirection = -collidableObject.transform.forward;

            Rigidbody rb = collidableObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                originalVelocity = rb.velocity;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += bulletDirection * moveSpeed * Time.deltaTime;

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

            objectName = other.gameObject.name;

            if (!objectsToAvoid.Contains(objectName))
            {
                createTwice();
                objectsToAvoid.Add(objectName);
            }
        }

        if (other.CompareTag("Line"))
        {
            isLoose = true;
            collidableObject.CancelInvoke();
            scPlayer.isPlayerDeath = true;
            cameraMove cameraShake = Camera.main.GetComponent<cameraMove>();
            cameraShake.StartShake();
            colorChange();
            Invoke("bulletAllDestory", 0);
            GameObject fadePanel = GameObject.Find("Canvas1");
            Transform panelChild = fadePanel.transform.Find("Panel");
            panelChild.gameObject.SetActive(true);
            GameObject playerPanel = GameObject.Find("Player");
            Transform playerChild = playerPanel.transform.Find("Canvas");
            playerChild.gameObject.SetActive(false);
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
                    spawnPosition = new Vector3(i * 0.1f, 0, 0);
                else
                    spawnPosition = new Vector3(i * -0.1f, 0, 0);

                if (gameObject.CompareTag("SpecialBullet"))
                {
                    if (j == 0)
                        spawnPosition = transform.right * i * 0.2f;
                    else
                        spawnPosition = transform.right * i * -0.2f;
                }
                enemyBullet p = null;
                p = Instantiate(scEnemy, gameObject.transform.position + spawnPosition, Quaternion.identity);
                p.objectsToAvoid.Add(objectName);

                StartCoroutine(waitSpawn());
            }
        }
    }

    private void colorChange()
    {

            collidableObject.OnDestroy -= HandleObjectDestroyed;

            initialPosition = gameObject.transform.position;

            Color fadedColor = initialColor;
            fadedColor.r = 1.0f;
            renderer.material.color = fadedColor;

            StartCoroutine(ResetColorAfterDelay());
        
    }

    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(0.0f); // 0.5秒待つ
        renderer.material.color = initialColor; // 元の色に戻す
        Destroy(gameObject, 0.2f);
    }

    private IEnumerator waitSpawn()
    {
        yield return new WaitForSeconds(0.1f);
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

    private bool GetLoose()
    {
        return isLoose;
    }
}
