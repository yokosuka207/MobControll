using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerGage : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // �Ǐ]����3D�I�u�W�F�N�g
    [SerializeField] private Slider slider; // �X���C�_�[
    private playerMove scParent;
    private enemyHealth collidableObject;
    private bool isEnemyBaseDestory = false;

    private void Start()
    {
        scParent = GetComponent<playerMove>();

        // BaseDestroy�擾
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
    }

    private void Update()
    {
        if (targetObject != null)
        {
            // 3D�I�u�W�F�N�g�̈ʒu���X�N���[�����W�ɕϊ�
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.position);
            slider.transform.position = screenPosition + new Vector3(-25.0f, 5.0f, 0.0f);

            if(!isEnemyBaseDestory)
                slider.value = scParent.GetShotNumber();
            
            else if(isEnemyBaseDestory)
                slider.value = 0;
        }
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            isEnemyBaseDestory = true;
        }
    }
}
