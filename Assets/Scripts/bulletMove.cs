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
