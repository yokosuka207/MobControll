using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    [SerializeField] private GameObject bulletObj; // 生成するオブジェクト
    private float moveSpeed = 6.0f;
    private GameObject bulleObjChild;
    private Vector2 startTouchPos;
    private bool isTouch = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            // オブジェクトを生成
            InvokeRepeating("createBullet", 0.0f, 1.5f);
            isTouch = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouch = false;

            if(IsInvoking())
            {
                CancelInvoke();
            }
        }

        if (isTouch)
        {
            Vector2 nowTouchPos = (Vector2)Input.mousePosition - startTouchPos;

            if (Mathf.Abs(nowTouchPos.x) > 50)
            {
                float moveX = nowTouchPos.x > 0 ? 1.0f : -1.0f;
                transform.position += Vector3.right * moveX * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void createBullet()
    {
        bulleObjChild = Instantiate(bulletObj, this.gameObject.transform);
        bulleObjChild.transform.parent = null;
    }
}
