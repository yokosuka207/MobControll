using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int twice;
    [SerializeField] private float waitTime = 2.0f;
    private enemyHealth collidableObject;
    private bool isFall = false;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Å~" + twice.ToString();

        // BaseDestroyéÊìæ
        collidableObject = FindObjectOfType<enemyHealth>();
        if (collidableObject != null)
        {
            collidableObject.OnDestroy += HandleObjectDestroyed;
        }
    }

    private void Update()
    {
        if(isFall)
        {
            gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f) * 3 * Time.deltaTime;

            if (gameObject.transform.position.y < -0.6f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void HandleObjectDestroyed()
    {
        if (gameObject != null)
        {
            StartCoroutine(destoryCoroutine());
        }
    }

    private IEnumerator destoryCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        isFall = true;
    }

    public int GetTwice()
    {
        return twice;
    }
}
