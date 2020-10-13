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
    int lastLvl = 0;

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

    public void ScoreUpdate(int score, string time, int level)
    {
        Char[] scoreCharTab = score.ToString().ToCharArray();
        int i = scoreCharTab.Length-1;

        foreach(Text scoreDecimal in scoreTexts)
        {
            scoreDecimal.GetComponent<Animator>().SetBool("NewDifficulty", false);

            if (i>=0)
            {
                if (scoreCharTab[i].ToString() != scoreDecimal.text)
                {
                    scoreDecimal.text = scoreCharTab[i].ToString();
                    scoreDecimal.GetComponent<Animator>().SetBool("ScoreChange", true);
                }
                else
                {
                    scoreDecimal.GetComponent<Animator>().SetBool("ScoreChange", false);
                }
            }
            if (level != lastLvl)
            {
                scoreDecimal.GetComponent<Animator>().SetBool("ScoreChange", false);
                scoreDecimal.GetComponent<Animator>().SetBool("NewDifficulty", true);
                lastLvl = level;
            }

            i--;
        }

        timeText.text = time;
    }
}
