using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyListener : MonoBehaviour
{
    private void Start()
    {
        // CollidableObjectのOnDestroyedイベントを購読
        Destroyable collidableObject = FindObjectOfType<Destroyable>();
        if (collidableObject != null)
        {
            //Debug.Log("はいってる");
            collidableObject.OnDestroyed += HandleObjectDestroyed;
        }
    }

    // CollidableObjectが破壊されたときに呼ばれる関数
    private void HandleObjectDestroyed()
    {
        Debug.Log("はいってる");
        // このオブジェクトを破壊
        Destroy(gameObject);
    }
}
