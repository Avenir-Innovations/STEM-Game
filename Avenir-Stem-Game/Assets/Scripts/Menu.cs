using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void MoveToGame() {
        SceneManager.LoadScene("Math");
    }

}
