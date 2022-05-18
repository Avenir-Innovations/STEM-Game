using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    public Text problemText, endGameStats, endGamePlaceText;
    public GameObject winScreen, problemScreen;
    public GameObject player;
    public GameObject[] cpus;
    public Button[] answerButtons = new Button[4];
    public int speed = 50;
    private int finished = 0;

    private char[] operators = { '+', '-', '*' };
    private Vector3[] cpuPositions = new Vector3[3];
    private bool[] cpusFinished = new bool[3];
    private int[] cpuSpeeds = new int[3];
    private int num1, num2;
    private char oper;
    private int score, missed;
    private float finishLine = 1000.0f;
    private Vector3 playerPos = new Vector3();
    private bool gameFinished = false;

    private List<string> playerProblems = new List<string>();
    private List<string> playerAnswers = new List<string>();
    private List<bool> playerProblemsCorrect = new List<bool>();

    public void Start() {
        problemScreen.SetActive(true);

        for (int i = 0; i <= 2; i++) {
            cpuSpeeds[i] = speed;
        }

        NextProblem();
        for (int i = 0; i < answerButtons.Length; i++) {
            answerButtons[i].onClick.AddListener(CheckAnswer);
        }
    }

    private void NextProblem() {
        problemText.text = CreateProblem();
        answerButtons[0].gameObject.transform.Find("Text").GetComponent<Text>().text = GenerateAnswer().ToString();
        Debug.Log(answerButtons.Length);

        int[] answers = new int[4];
        int[] possibleAnswers = GeneratePossibleAns();

        answers[0] = GenerateAnswer();
        for (int i = 1; i < answers.Length; i++) {
            answers[i] = possibleAnswers[i - 1];
        }

        System.Random random = new System.Random();
        answers = answers.OrderBy(x => random.Next()).ToArray();

        for (int i = 0; i < answerButtons.Length; i++) {
            answerButtons[i].gameObject.transform.Find("Text").GetComponent<Text>().text = answers[i].ToString();
        }
    }

    public string CreateProblem() {
        num1 = UnityEngine.Random.Range(0, 10);
        num2 = UnityEngine.Random.Range(0, 10);
        oper = operators[UnityEngine.Random.Range(0, 3)];
        string problemString = num1.ToString() + " " + oper + " " + num2.ToString();

        playerProblems.Add(problemString);
        playerAnswers.Add(GenerateAnswer().ToString());

        return problemString;
    }
    
    public int GenerateAnswer() {
        switch (oper) {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
        }

        return 0;
    }

    public int[] GeneratePossibleAns() {
        int[] possibleAns = new int[3];
        for (int i = 0; i < 3; i++) {
            possibleAns[0] = UnityEngine.Random.Range(-9, 81);
            if (i > 0) {
                while (possibleAns[i] == possibleAns[i - 1] || possibleAns[i] == GenerateAnswer() || possibleAns[i] == possibleAns[0]) {
                    Debug.Log("ramdonized");
                    possibleAns[i] = UnityEngine.Random.Range(-9, 81);
                }
            }
        }

        return possibleAns;
    }

    public void CheckAnswer() {
        if (int.Parse(EventSystem.current.currentSelectedGameObject.transform.Find("Text").GetComponent<Text>().text) == GenerateAnswer()) {
            score++;
            speed += 5;
            playerProblemsCorrect.Add(true);
        } else {
            if (speed >= 10) {
                speed -= 5;
            }
            missed++;
            playerProblemsCorrect.Add(false);
        }

        NextProblem();
    }

    private bool IsPlayerFinished(float position) {
        if (position >= finishLine) {
            return true;
        } else {
            return false;
        }
    }

    private void Update() {
        if (!gameFinished) {
            playerPos = player.transform.position;
            playerPos.y += speed * Time.deltaTime;
            player.transform.position = playerPos;

            for (int i = 0; i <= 2; i++) {
                if (UnityEngine.Random.Range(0, 100) == 0) { // 10% chance of speed changing
                    if (UnityEngine.Random.Range(0, 2) == 0) { //50% chance of speeding up or slowing down
                        cpuSpeeds[i] += 5;
                    } else {
                        if (cpuSpeeds[i] > 5) {
                            cpuSpeeds[i] -= 5;
                        }
                    }
                }

                cpuPositions[i] = cpus[i].transform.position;
                cpuPositions[i].y += cpuSpeeds[i] * Time.deltaTime;
                cpus[i].transform.position = cpuPositions[i];

                if (IsPlayerFinished(cpus[i].transform.position.y)) {
                    cpusFinished[i] = true;
                    finished++;
                }
            }

            if (IsPlayerFinished(playerPos.y)) {
                gameFinished = true;

                // make winning screen show up
                problemScreen.SetActive(false);
                winScreen.SetActive(true);

                string endGameString = "";
                for (int i = 0; i < playerProblems.Count() - 1; i++) {
                    if (playerProblemsCorrect[i]) {
                        endGameString += "Correct: ";
                    } else {
                        endGameString += "Incorrect: ";
                    }

                    endGameString += playerProblems[i] + " = ";
                    endGameString += playerAnswers[i] + "\n";
                }

                string endGamePlace = "";
                switch (finished) {
                    case 0:
                        endGamePlace += "First Place!";
                        break;
                    case 1:
                        endGamePlace += "Second Place!";
                        break;
                    case 2:
                        endGamePlace += "Third Place!";
                        break;
                    case 3:
                        endGamePlace += "Fourth Place!";
                        break;
                }

                endGamePlaceText.text = endGamePlace;
                endGameStats.text = endGameString;
            }
        }
    }

}
