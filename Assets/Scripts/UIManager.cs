using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public Canvas canvas;

    GameObject deathPanel;
    GameObject mainPanel;
    Text[] scoreTexts;
    Text finalText;
    Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        deathPanel = canvas.transform.Find("DeathPanel").gameObject;
        mainPanel = canvas.transform.Find("MainPanel").gameObject;

        scoreTexts = new Text[] {
            mainPanel.transform.Find("Score/Score1").GetComponent<Text>(),
            mainPanel.transform.Find("Score/Score2").GetComponent<Text>(),
            mainPanel.transform.Find("Score/Score3").GetComponent<Text>()
        };
        scoreTexts[0].text = 0.ToString();
        scoreTexts[1].text = "";
        scoreTexts[2].text = "";

        finalText = deathPanel.transform.Find("Subtitle").GetComponent<Text>();
        timeText = mainPanel.transform.Find("Time").GetComponent<Text>();
    }

    public void DisplayDeathScreen(int score)
    {
        finalText.text = "TIME :" + timeText.text + "          SCORE :" + score;
        mainPanel.SetActive(false);
        deathPanel.SetActive(true);
    }

    public void ScoreUpdate(Char[] scores, string time)
    {
        int i = scores.Length-1;

        foreach(Text scoreDecimal in scoreTexts)
        {
            if(i>=0)
            {
                if (scores[i].ToString() != scoreDecimal.text)
                {
                    scoreDecimal.text = scores[i].ToString();
                    scoreDecimal.GetComponent<Animator>().SetBool("ScoreChange", true);
                }
                else
                {
                    scoreDecimal.GetComponent<Animator>().SetBool("ScoreChange", false);
                }
            }

            i--;
        }

        timeText.text = time;
    }
}
