using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public GameObject Sentence;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject continueButton;
    public GameObject bg;
    public GameObject profileOwl;
    public GameObject talkIcon;
    public GameObject Boundrey;

    public owlManager OM;

    public bool DialogEnded = false;



    void Start()
    {
        StartCoroutine(Type());
        Boundrey.SetActive(true);
    }

    void Update()
    {
        // kan een up arrow aan toegevoegd worden Input.GetKeyDown(KeyCode.UpArrow)
        if (OM.RavenInRange)
        {
            if (textDisplay.text == sentences[index] && textDisplay.text != sentences[0])
            {
                continueButton.SetActive(true);
                bg.SetActive(true);
                profileOwl.SetActive(true);
                Sentence.SetActive(true);

            }

            if (textDisplay.text == sentences[0] && DialogEnded == false)
            {
                talkIcon.SetActive(true);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    talkIcon.SetActive(false);
                    NextSentence();
                    continueButton.SetActive(true);
                    bg.SetActive(true);
                    profileOwl.SetActive(true);
                    Sentence.SetActive(true);
                }
            }
        }

        if(OM.RavenInRange == false)
        {
            continueButton.SetActive(false);
            bg.SetActive(false);
            profileOwl.SetActive(false);
            Sentence.SetActive(false);

        }

        
    }

    IEnumerator Type()
    {
        if (OM.RavenInRange)
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
       
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);

        
       
        if (index < (sentences.Length - 1))
        {
            if (OM.RavenInRange)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());
            }
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            bg.SetActive(false);
            profileOwl.SetActive(false);
            Sentence.SetActive(false);
            DialogEnded = true;
            Boundrey.SetActive(false);
        }
    }
}
