using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ui : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private string nextStage;
    private float countdownDuration = 3.0f;
    private bool countdownStarted = false;

    private float currentTime;

    void Start()
    {
        currentTime = countdownDuration;
    }

    void Update()
    {
        if (countdownStarted)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                bulletEnemyAllDestory();
                Invoke("bulletAllDestory", 1.0f);
                StartCoroutine(changeScene());
                Destroy(countdownText, 1.0f);
            }

            UpdateCountdownText();
        }
        else
        {
            StartCoroutine(StartCountdown());
        }
    }

    void UpdateCountdownText()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        countdownText.text = seconds.ToString();
    }

    IEnumerator StartCountdown()
    {
        countdownText.text = "Winner!";
        yield return new WaitForSeconds(1);
        countdownStarted = true;
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextStage);
    }

    private void bulletAllDestory()
    {
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("Bullet");
        GameObject[] objectsSBulletDestroy = GameObject.FindGameObjectsWithTag("SpecialBullet");

        foreach (GameObject nobj in objectsBulletDestroy)
        {
            Destroy(nobj);
        }
        foreach (GameObject sobj in objectsSBulletDestroy)
        {
            Destroy(sobj);
        }
    }
    private void bulletEnemyAllDestory()
    {
        GameObject[] objectsBulletDestroy = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject eobj in objectsBulletDestroy)
        {
            Destroy(eobj);
        }
    }
}
