using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.win)
        {
            LoadResults();
            GameManager.win = false;
        }
    }

    public void LoadResults()
    {
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(levelIndex);
    }
}
