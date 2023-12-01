
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    private int score;
    public int lives;
    private int ghostMultiplier = 1;
    private bool start = false;

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text GameOverText;
    [SerializeField] private Text ReadyText;
    [SerializeField] private Text ScoreText;
    [SerializeField] private Image LivesImage1;
    [SerializeField] private Image LivesImage2;
    [SerializeField] private Image LivesImage3;
    [SerializeField] private AudioClip StartSoundEffect;
    [SerializeField] private AudioClip PacmanEatSoundEffect;
    [SerializeField] private AudioClip PacmanDeathSoundEffect;
    [SerializeField] private AudioClip PacmanEatGhostSoundEffect;
    [SerializeField] private AudioClip EatPowerPelletSoundEffect;
    public AudioSource AudioSource;

    private void Awake()
    {
        // Singleton Design Pattern: 
        // Only one instance of GameManager during the game

        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
        AudioSource = GetComponent<AudioSource>();

    }


    private void Update()
    {
        // STOP game until player enter "space" to start
        if(Input.GetKeyDown(KeyCode.Space)) {
            AudioSource.Pause();
            start = true;
        }

        if(start == false)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
        
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    // Initialize game elements
    private void NewGame()
    {
        AudioSource.clip = StartSoundEffect;
        AudioSource.Play();
        GameOverText.enabled = false;
        ReadyText.enabled = true;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    // Start new round
    private void NewRound()
    {
        GameOverText.enabled = false;
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }


        ResetState();
    }

    // Reset movement and position of all game objects
    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        GameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);

        }

        pacman.gameObject.SetActive(false);
        start = false;

    }


    private void SetScore(int point)
    {
        score = point;
        ScoreText.text = $"Score: {score}";
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        if(lives != 3)
        {
            if (LivesImage1.enabled) LivesImage1.enabled = false;
            else if (LivesImage2.enabled) LivesImage2.enabled = false;
            else if (LivesImage3.enabled) LivesImage3.enabled = false;
        }

    }

    // Hanlde event : Ghost be eaten by Pacman
    // ghostMultiplier: the bonus point will be increased when Pacman eat as much as ghosts
    public void GhostEaten(Ghost ghost)
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Pause();
        }
        AudioSource.clip = PacmanEatGhostSoundEffect;
        AudioSource.Play();
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
    }

    // Hanlde event : Pacmna be eaten by Ghost
    public void PacmanEaten()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Pause();
        }
        AudioSource.clip = PacmanDeathSoundEffect;
        AudioSource.Play();
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0)
        {
            //if player lives available, reset state both ghost and pacman

            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            // Othewise, EndGame
            GameOver();
        }
    }

    // Handle Event: Pacman Pellet
    public void PelletEaten(Pellet pellet)
    {

        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = PacmanEatSoundEffect;
            AudioSource.Play();
        }

        if (score == 0)
        {
            ReadyText.enabled = false;
        }

        SetScore(score + pellet.point);
        pellet.gameObject.SetActive(false);

        // If there is no pellet that available, end game
        if(IsAllPelletEaten())
        {
            GameOver();
        }
    }

    private bool IsAllPelletEaten()
    {
        foreach(Transform pellet in pellets)
        {
            if(pellet.gameObject.activeSelf) return false;
        }
        return true;
    }
    
    // Handle Event: Pacman eat PowerPellet
    public void PowerPelletEaten(PowerPellet powerPellet)
    {
        AudioSource.Pause();
        AudioSource.clip = EatPowerPelletSoundEffect;
        AudioSource.Play();
        // trigger all ghostFrightended state
        foreach (Ghost ghost in ghosts)
        {
            // duration time of this state depend on powerPellet
            ghost.frightened.Enable(powerPellet.duration);
        }

        PelletEaten(powerPellet);

        // if continue eat PowerPellet, cancel reset bonus point
        CancelInvoke(nameof(ResetGhostMultiplier));

        // Reset ghost bonus point when firightened state end
        Invoke(nameof(ResetGhostMultiplier), powerPellet.duration);
    }

    
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }


}
