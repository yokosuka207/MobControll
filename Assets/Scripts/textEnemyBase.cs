using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textEnemyBase : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] public int number;

    void Update()
    {
        text.text = "" + number.ToString();
    }
}
