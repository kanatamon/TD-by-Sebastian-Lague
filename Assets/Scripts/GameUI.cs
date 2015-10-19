using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;

    bool isLoading;

	// Use this for initialization
	void Start () {
        isLoading = false;
        FindObjectOfType<Player>().OnDeath += OnGameOver;
        StartCoroutine(Fade(Color.white, Color.clear, 1));
	}
	
	void OnGameOver(){
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time){
        float speed = 1 / time;
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI Input
    public void StartNewGame(){
        if (!isLoading)
        {
            //Application.LoadLevel("Game");
            StartCoroutine(LoadNewGame());
        }
    }

    IEnumerator LoadNewGame(){
        isLoading = true;
        gameOverUI.SetActive(false);

        float time = 1;
        StartCoroutine(Fade(Color.black, Color.white, time));

        yield return new WaitForSeconds(time);

        Application.LoadLevel("Game");
    }


}
