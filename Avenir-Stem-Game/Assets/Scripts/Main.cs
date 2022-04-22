using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour {

    public Text problemText;
    public GameObject player;
    public GameObject[] cpus;
    public Button[] answerButtons = new Button[4];
    public int speed = 50;

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

    public void Start() {
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
        for (int i = 1; i < answerButtons.Length; i++) {
            answerButtons[i].gameObject.transform.Find("Text").GetComponent<Text>().text = GeneratePossibleAns()[i - 1].ToString();
            Debug.Log(i);
        }
    }

    public string CreateProblem() {
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        oper = operators[Random.Range(0, 3)];
        return num1.ToString() + oper + num2.ToString();
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
            possibleAns[0] = Random.Range(-9, 81);

            if (i > 0) {
                while (possibleAns[i] == possibleAns[i - 1]) {
                    possibleAns[i] = Random.Range(-9, 81);
                }
            }
        }

        possibleAns[Random.Range(0, 2)] = GenerateAnswer();

        return possibleAns;
    }

    public void CheckAnswer() {
        if (int.Parse(EventSystem.current.currentSelectedGameObject.transform.Find("Text").GetComponent<Text>().text) == GenerateAnswer()) {
            score++;
            speed += 5;
        } else {
            if (speed >= 10) {
                speed -= 5;
            }
            missed++;
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
        /*if (Input.GetKeyDown(KeyCode.Return)) {
            CheckAnswer();
        }*/

        if (!gameFinished) {
            playerPos = player.transform.position;
            playerPos.y += speed * Time.deltaTime;
            player.transform.position = playerPos;

            int finished = 0;
            for (int i = 0; i <= 2; i++) {
                // cpus[i].transform.localPosition = new Vector3(cpus[i].transform.localPosition.x, cpus[i].transform.localPosition.y + cpuSpeeds[i] * Time.deltaTime, cpus[i].transform.localPosition.z);

                cpuPositions[i] = cpus[i].transform.position;
                cpuPositions[i].y += cpuSpeeds[i] * Time.deltaTime;
                cpus[i].transform.position = cpuPositions[i];

                if (IsPlayerFinished(cpus[i].transform.position.y)) {
                    cpusFinished[i] = true;
                    finished++;
                }
            }

            if (finished >= 3 && IsPlayerFinished(playerPos.y)) {
                gameFinished = true;
                // make winning screen show up
            }
        }
    }

}
