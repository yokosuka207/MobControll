using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyListener : MonoBehaviour
{
    private void Start()
    {
        // CollidableObject��OnDestroyed�C�x���g���w��
        Destroyable collidableObject = FindObjectOfType<Destroyable>();
        if (collidableObject != null)
        {
            //Debug.Log("�͂����Ă�");
            collidableObject.OnDestroyed += HandleObjectDestroyed;
        }
    }

    // CollidableObject���j�󂳂ꂽ�Ƃ��ɌĂ΂��֐�
    private void HandleObjectDestroyed()
    {
        Debug.Log("�͂����Ă�");
        // ���̃I�u�W�F�N�g��j��
        Destroy(gameObject);
    }
}
