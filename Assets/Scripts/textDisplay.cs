using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int twice;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "�~" + twice.ToString();
    }

    public int GetTwice()
    {
        return twice;
    }

}
