using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createStage : MonoBehaviour
{
    [SerializeField] GameObject[] createObj;
    [SerializeField] GameObject floorObj;
    playerMove scPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Player");
        scPlayer = obj.GetComponent<playerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scPlayer.isNextMove)
        {
            StartCoroutine(waitTime());

            if (floorObj == null)
            {
                foreach (var obj in createObj)
                {
                    obj.SetActive(true);
                    enabled = false;
                }
            }
        }
    }

    private IEnumerator waitTime()
    {
        yield return new WaitForSeconds(1);
    }
}
