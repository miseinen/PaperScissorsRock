using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PaperScissorsRockGame : MonoBehaviour
{
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;
    [SerializeField] private Button rockButton;

    [SerializeField] private GameObject[] choosePrefab;

    [SerializeField] private Image[] playerStars;
    [SerializeField] private Image[] botStars;

    [SerializeField] private AudioManager audioManager;

    [SerializeField] private Text winText;
 
    private int _playerChoice;
    private int _botChoice;
    private int _playerResult;
    private int _botResult;

    private bool _playerTurn=true;
    private bool _isRoundEnd;

    private float _zPosPlayer=-1.04f;
    private float _zPosBot=2f;
    private float _timer;

    private Coroutine _botStateCoroutine;
    private Coroutine _gameResultCoroutine;

    private Action OnPlayerTurnComplete;
    private Action OnBotTurnComplete;
    

    private enum Choice
    {
        Paper=1,
        Scissors,
        Rock,
    }

    private void Start()
    {
        AddListener();

        OnPlayerTurnComplete += SetBotState;
        OnBotTurnComplete += GameResult;

        winText.text = "";
    }

    private void Update()
    {
        
    }

    private void SetPlayerStatePaper()
    {
        if (!_isRoundEnd)
        {
            RemoveListener();
            _playerChoice = (int) Choice.Paper;
            InstantiatePrefab(_playerChoice,_zPosPlayer);
            audioManager.PlayOneShot("Paper");
            OnPlayerTurnComplete?.Invoke();
            _playerTurn = false;
        }
    }

    private void SetPlayerStateScissors()
    {
        if (!_isRoundEnd)
        {
            RemoveListener();
            _playerChoice = (int) Choice.Scissors;
            InstantiatePrefab(_playerChoice,_zPosPlayer);
            audioManager.PlayOneShot("Scissors");
            OnPlayerTurnComplete?.Invoke();
            _playerTurn = false; 
        }
    }

    private void SetPlayerStateRock()
    {
        if (!_isRoundEnd)
        {
            RemoveListener();
            _playerChoice = (int) Choice.Rock;
            InstantiatePrefab(_playerChoice,_zPosPlayer);
            audioManager.PlayOneShot("Rock");
            OnPlayerTurnComplete?.Invoke();
            _playerTurn = false;
        }
    }

    private void SetBotState()
    {
        _botStateCoroutine = StartCoroutine(BotStateCoroutine());
        _botStateCoroutine = null;
    }

    private IEnumerator BotStateCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _botChoice = Random.Range(1, 4);
        InstantiatePrefab(_botChoice,_zPosBot);
        _playerTurn = true;
        _isRoundEnd = true;
        OnBotTurnComplete?.Invoke();
    }

    private void InstantiatePrefab(int index, float zPos)
    {
        var rotation = choosePrefab[index - 1].transform.rotation;
        if (index == 1)
        {
            rotation.y = 90f;
        }
        GameObject obj = Instantiate(choosePrefab[index-1], new Vector3(0f, 0.1f, zPos),
            Quaternion.Euler(rotation.x,rotation.y,rotation.z));
        Destroy(obj,2f);
    }

    private void GameResult()
    {
        _gameResultCoroutine = StartCoroutine(GameResultCoroutine());
        _gameResultCoroutine = null;
    }

    private IEnumerator GameResultCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _isRoundEnd = false;
        AddListener();
        int choicesDifference = _playerChoice - _botChoice;

        if (choicesDifference == 0)
        {
            audioManager.PlayOneShot("Tie");
            yield break;
        }

        if (choicesDifference == 1||choicesDifference==-2)
        {
            _playerResult++;
            audioManager.PlayOneShot("PlayerStar");
            playerStars[_playerResult-1].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if (_playerResult == 5)
            {
                RemoveListener();
                audioManager.PlayOneShot("PlayerWin");
                winText.text = "Excellent!";
                StartCoroutine(ReloadScene());
            }
        }

        if (choicesDifference == -1 || choicesDifference == 2)
        {
            _botResult++;
            audioManager.PlayOneShot("BotStar");
            botStars[_botResult-1].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if (_botResult == 5)
            {
                RemoveListener();
                audioManager.PlayOneShot("BotWin");
                winText.text = "Don't Worry\nTry again";
                StartCoroutine(ReloadScene());
            }
        }
    }

    private void RemoveListener()
    {
        paperButton.onClick.RemoveListener(SetPlayerStatePaper);
        scissorsButton.onClick.RemoveListener(SetPlayerStateScissors);
        rockButton.onClick.RemoveListener(SetPlayerStateRock);
    }

    private void AddListener()
    {
        paperButton.onClick.AddListener(SetPlayerStatePaper);
        scissorsButton.onClick.AddListener(SetPlayerStateScissors);
        rockButton.onClick.AddListener(SetPlayerStateRock);
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
