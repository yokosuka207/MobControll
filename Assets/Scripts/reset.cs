using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reset : MonoBehaviour
{
    public void Retry()//ここに書いた文字「Retry」が選択画面で表示される
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
