using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Levels")]
    public List<GameObject> LevelList;

    [Header("UI")]
    public GameObject LvlCompletedMenu;
    public GameObject LvlFailedMenu;
    public Text LevelText;

    [Header("Sounds")]
    public AudioClip Level_Completed;
    public AudioClip Level_Failed;
    private AudioSource audioSource;
    private bool allowSFX = true;

    PlayerController playerController;
    GameObject Pin;
    bool MoveThePin = false;

    void Awake()
    {
        if (PlayerPrefs.GetInt("level") >= LevelList.Count)
        {
            Instantiate(LevelList[Random.Range(0, LevelList.Count)]);
            LevelText.text = "УРОВЕНЬ " + (PlayerPrefs.GetInt("level") + 1);
        }
        else
        {
            Instantiate(LevelList[PlayerPrefs.GetInt("level")]);
            LevelText.text = "УРОВЕНЬ " + (PlayerPrefs.GetInt("level") + 1);
        }


        playerController = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !playerController.failed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo))
            {
                if (hitinfo.transform.tag == "Pin")
                {
                    Pin = hitinfo.transform.gameObject;
                    Destroy(hitinfo.transform.gameObject, .6f);
                    MoveThePin = true;
                }
            }
        }

        if (MoveThePin && Pin != null && !playerController.failed)
        {
            Pin.transform.position = Vector3.MoveTowards(Pin.transform.position, Pin.transform.GetChild(2).gameObject.transform.position, 40f * Time.deltaTime);
        }

        if (playerController.Completed && !playerController.failed)
        {
            StartCoroutine(waitForLvlCompleted());

            if (PlayerPrefs.GetInt("sfxindex") == 0 && allowSFX)
            {
                audioSource.PlayOneShot(Level_Completed);
                allowSFX = false;
            }
        }

        if (playerController.failed)
        {
            StartCoroutine(waitForLvlFailed());

            if (PlayerPrefs.GetInt("sfxindex") == 0 && allowSFX)
            {
                audioSource.PlayOneShot(Level_Failed);
                allowSFX = false;
            }
        }
    }

    IEnumerator waitForLvlCompleted()
    {
        yield return new WaitForSeconds(1.5f);
        LvlCompletedMenu.SetActive(true);
        playerController.Completed = false;
    }

    IEnumerator waitForLvlFailed()
    {
        yield return new WaitForSeconds(1.5f);
        LvlFailedMenu.SetActive(true);
        playerController.failed = false;
    }

    public void NextLevel()
    {
        int LevelIndex = PlayerPrefs.GetInt("level") + 1;
        PlayerPrefs.SetInt("level", LevelIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void skipLevel()
    {
        
    }

}
