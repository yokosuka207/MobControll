using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool isEnabled = true;
    private int twice;
    private float disableTime = 1.0f;


    private void Start()
    {
        DisableFunctionTemporarily(2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
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

    private void createTwice()
    {
        if (isEnabled)
        {
            for (int i = 0; i < twice; i++)
            {
                int rand = Random.Range(0, 2) * 2 - 1;
                Instantiate(gameObject, gameObject.transform.position + new Vector3(i * 0.5f * rand, 0.0f, 0.0f), Quaternion.identity);
            }
            Destroy(this.gameObject);
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

}
