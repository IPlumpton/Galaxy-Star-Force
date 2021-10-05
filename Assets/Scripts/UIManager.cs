using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText = null;
    [SerializeField]
    private Text _gameOverText = null;
    [SerializeField]
    private Text _restartText = null;
    [SerializeField]
    private GameManager _gameManager = null;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImage;

    private int _score = 0;
    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.enabled = false;
        _scoreText.text = "Score: " + _score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreText(int updatedScore)
    {
        _score = updatedScore;
        _scoreText.text = "Score: " + _score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    public void OnPlayerDeath()
    {
        StartCoroutine(GameOverTextFlicker());
        _gameManager.GameOver();
        _restartText.enabled = true;
    }

    IEnumerator GameOverTextFlicker()
    {
        while(true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(1.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
