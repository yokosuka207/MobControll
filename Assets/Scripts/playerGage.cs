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
    private bool isEnemyDestory = false;

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

                slider.value = scParent.GetShotNumber();
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
            isEnemyDestory = true;
        }
    }
}
