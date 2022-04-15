using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public InputField input;
    public GameObject player;
    public GameObject[] cpus;
    public int speed = 50;

    private char[] operators = { '+', '-', '*' };
    private Vector3[] cpuPositions = new Vector3[3];
    private bool[] cpusFinished = new bool[3];
    private int[] cpuSpeeds = new int[3];
    private int num1, num2;
    private char oper;
    private int score, missed;
    private float finishLine = 0.0f;
    private Vector3 playerPos = new Vector3();
    private bool gameFinished = false;

    public void Start() {
        for (int i = 0; i < 2; i++) {
            cpuSpeeds[i] = speed;
        }

        foreach (GameObject g in cpus) {
            Debug.Log(g.name);
        }
    }

    public string CreateProblem() {
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        oper = operators[Random.Range(0, 2)];
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
            possibleAns[i] = Random.Range(-9, 81);
        }

        possibleAns[Random.Range(0, 2)] = GenerateAnswer();

        return possibleAns;
    }

    public void CheckAnswer() {
        string answer = input.text;
        if (int.Parse(answer) == GenerateAnswer()) {
            score++;
            speed += 5;
        } else {
            if (speed >= 10) {
                speed -= 5;
            }
            missed++;
        }
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

            int finished = 0;
            for (int i = 0; i < 2; i++) {
                cpus[i].transform.localPosition = new Vector3(cpus[i].transform.localPosition.x, cpus[i].transform.localPosition.y + cpuSpeeds[i] * Time.deltaTime, cpus[i].transform.localPosition.z);

                /*cpuPositions[i] = cpus[i].transform.position;
                cpuPositions[i].y += cpuSpeeds[i] * Time.deltaTime;
                cpus[i].transform.position = cpuPositions[i];*/

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
