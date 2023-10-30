using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cameraMove : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private TMP_Text textObj;
    private playerMove scPlayer;
    private bool isMove;
    public float moveSpeed = 5.0f;
    public float moveDistance = 2.0f;

    // óhÇÍ
    public float shakeDuration = 0.2f; // óhÇÍÇÈéûä‘
    public float shakeAmount = 0.5f;  // óhÇÍÇÃã≠Ç≥

    private Vector3 originalPosition;
    private float shakeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = null;

        textObj.gameObject.SetActive(false);

        GameObject obj = GameObject.Find("Player");
        scPlayer = obj.GetComponent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scPlayer.GetLastEnemy())
        {
            isMove = true;
            gameObject.transform.DetachChildren();
        }

        if (shakeTime > 0)
        {
            // ÉJÉÅÉâÇÃà íuÇÉâÉìÉ_ÉÄÇ…ïœçXÇµÇƒóhÇÁÇ∑
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;

            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0f;
            //transform.position = originalPosition;
        }

        if (isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == targetObj.transform.position)
            {
                isMove = false;
                if (textObj != null)
                    textObj.gameObject.SetActive(true);
            }
        }

        if (scPlayer.isNextMove)
        {
            gameObject.transform.parent = parentObj.transform;
        }
        else
        {
            gameObject.transform.parent = null;
            originalPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Ç†Ç†Ç†");
        if(other.CompareTag("MainCamera"))
        {
            isMove = false;
            textObj.gameObject.SetActive(true);
        }
    }

    public void StartShake()
    {
        shakeTime = shakeDuration;
    }
}
