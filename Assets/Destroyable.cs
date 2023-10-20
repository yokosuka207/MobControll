using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public event System.Action OnDestroyed;
    public GameObject objectToDeactivate;

    private void Start()
    {
        objectToDeactivate.SetActive(false);
        StartCoroutine(DelayedDestroy());
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.right * 3 * Time.deltaTime;
    }

    // Õ“Ë‚ÉŒÄ‚Î‚ê‚éŠÖ”
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //objectToDeactivate.SetActive(false);
            //StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2.0f); // ’x‰„‚³‚¹‚½‚¢•b”‚ğİ’è
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
