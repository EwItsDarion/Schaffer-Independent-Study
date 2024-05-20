using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Goldmetal.UndeadSurvivor
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [Header("# Game Control")]
        public bool isLive;
        public float gameTime;
        public float endGameTime;
        public float carryon;
        public float totalPauseTime;
        public float maxGameTime = 2 * 10f;
        [Header("# Player Info")]
        public int playerId;
        public int visuals;
        public int variety;
        public float health;
        public float maxHealth = 100;
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 3, 5, 10, 25, 55, 100, 170, 275, 400, 600 };
        [Header("# Game Object")]
        public PoolManager pool;
        public Player player;
        public LevelUp uiLevelUp;
        public Result uiResult;
        public GameObject enemyCleaner;

        
        public List<GameObject> weaponList;
        public List<GameObject> enemyList;
        public RuleTile[] tilingRules;
        public Tilemap tilemap;

        void Awake()
        {
            instance = this;
            Application.targetFrameRate = 60;
            SwapTilingRule(visuals);

            endGameTime = 0;

            if (PlayerPrefs.HasKey("totalPauseTime"))
            {
                totalPauseTime = PlayerPrefs.GetFloat("totalPauseTime");
            }

            else
            {
                totalPauseTime = 0;
            }

            if (PlayerPrefs.HasKey("endGameTime"))
            {
                endGameTime = PlayerPrefs.GetFloat("endGameTime");
            }
            else
            {
                endGameTime = 0;
            }
        }

        public void GameStart(int id)
        {
            playerId = id;
            health = maxHealth;

            player.gameObject.SetActive(true);
            uiLevelUp.Select(playerId % 2);
            Resume();

            AudioManager.instance.PlayBgm(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        IEnumerator GameOverRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        }

        public void GameDone()
        {
            StartCoroutine(GameDoneRoutine());
        }

        IEnumerator GameDoneRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Done();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }

        public void GameVictroy()
        {
            StartCoroutine(GameVictroyRoutine());
        }

        IEnumerator GameVictroyRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }

        public void GameRetry()
        {
            SceneManager.LoadScene(0);
            print(endGameTime);
        }

        public void GameQuit()
        {
            Application.Quit();
        }

        void Update()
        {
            if (!isLive)

                return;

            gameTime += Time.deltaTime;


            if (gameTime > maxGameTime) {
                gameTime = maxGameTime;
                GameVictroy();
            }
        }
        void FixedUpdate()
        {
            if (!isLive)
            {
                totalPauseTime += Time.deltaTime;
                PlayerPrefs.SetFloat("totalPauseTime", totalPauseTime);
                return;
            }

            endGameTime += Time.deltaTime;
            PlayerPrefs.SetFloat("endGameTime", endGameTime);

            if(endGameTime >= 1800){
                isLive = false;
                Time.timeScale = 0;
                
                GameDone();
                uiResult.gameObject.SetActive(true);
            }
        }
        public void GetExp()
        {
            if (!isLive)
                return;

            exp++;

            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) {
                level++;
                exp = 0;
                uiLevelUp.Show();
            }
        }

        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;

        }

        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;

        }

        public void SwapTilingRule(int index)
        {
        if (index >= 0 && index < tilingRules.Length)
        {
            tilemap.SetTile(new Vector3Int(0, 0, 0), tilingRules[index]);
        }
        else
        {
            Debug.LogError("Index out of range");
        }
        }

    }
}
