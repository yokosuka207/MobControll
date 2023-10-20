using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class explosionEnemy : MonoBehaviour
{
    public event Action OnDestoryed;
    public float eruptionForce = 10.0f; // ���΂̗�
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f); // �ŏ��̃X�P�[��
    public Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f); // �ő�̃X�P�[��
    public float shrinkDuration = 1.0f; // �k���ɂ����鎞��
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent <Rigidbody>();

        // �����_���ȃT�C�Y��ݒ�
        Vector3 randomScale = new Vector3(UnityEngine.Random.Range(minScale.x, maxScale.x), UnityEngine.Random.Range(minScale.y, maxScale.y), UnityEngine.Random.Range(minScale.z, maxScale.z));
        transform.localScale = randomScale;

        // �����_���ȕ����ɗ͂�������
        Vector3 eruptionDirection = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(0.5f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
        eruptionDirection.Normalize(); // �x�N�g���̐��K��
        rb.AddForce(eruptionDirection * eruptionForce, ForceMode.Impulse);

        // ��莞�Ԍ�ɏk���Ə��ł��J�n
        Invoke("ShrinkAndDestroy", shrinkDuration);
    }

    void ShrinkAndDestroy()
    {
        // �I�u�W�F�N�g���k��
        StartCoroutine(ShrinkObject());

        // ��莞�Ԍ�ɃI�u�W�F�N�g��j��
        Destroy(gameObject, shrinkDuration);
    }

    IEnumerator ShrinkObject()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0;

        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    void OnDestory()
    {
        OnDestoryed?.Invoke();
    }
}

