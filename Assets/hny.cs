using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hny : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hnym;
    [SerializeField] Text loadText;
    void Awake()
    {
        if (PlayerPrefs.GetInt("hny2020",0)==0)
        {
            audioSource.PlayOneShot(hnym);
            StartCoroutine("HnyM");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        
    }

    IEnumerator HnyM()
    {
        PlayerPrefs.SetInt("hny2020", 1000);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("StarterMenu");
    }

        void Start()
    {
        
    }

    public void GoToMenu() {
        SceneManager.LoadScene("StarterMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
