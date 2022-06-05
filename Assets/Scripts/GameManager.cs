using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int Stage;
    public string[] enemyObjs;
    public Transform[] spawnPoints;
    public Transform PlayerPos;

    public AudioSource EnemyDie;
    public AudioSource BossDie;
    public AudioSource Dangerous;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject Player;

    public Animator StageAni;
    public Animator ClearAni;
    public Animator FadeAni;
    public Animator DangerAni;

    public Text PlayerScoreTex;
    public GameObject GameOverPanel;
    public GameObject GameClearPanel;
    public Image[] lifeImage;

    public Text Level;
    public Text Exp;
    public Text BossHp;

    private bool isenemySpawn;
    public bool isScroll = false;

    //배치
    public List<EnemySpawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public ObjectManager objManager;

    public Text GameOverScore;
    public Text GameClearScore;

    void Awake()
    {
        SetResolution();
        spawnList = new List<EnemySpawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        ReadSpawnFile();
    }
    // Start is called before the first frame update
    void Start()
    {
        Player playerLogic = Player.GetComponent<Player>();
        playerLogic.Score = 0;
        StageStart();
    }
    public void StageStart()
    {
        StageAni.SetTrigger("On");
        StageAni.GetComponent<Text>().text = "Stage " + Stage + "\nStart!";
        ClearAni.GetComponent<Text>().text = "Clear ";
        ReadSpawnFile();
        Player playerLogic = Player.GetComponent<Player>();
        playerLogic.PlayerShot = false;
        playerLogic.ScoreUp = false;
        FadeAni.SetTrigger("In");
    }
    public void StageEnd()
    {
        ClearAni.SetTrigger("On");
        FadeAni.SetTrigger("Out");
        Player.transform.position = PlayerPos.position;
        Player playerLogic = Player.GetComponent<Player>();
        Stage++;

        if (Stage > 2)
        {
            playerLogic.PlayerShot = true;
            Invoke("GameClear", 5f);
        } 
        else
        {
            playerLogic.ScoreUp = true;
            playerLogic.PlayerShot = true;
            Invoke("StageStart", 5f);
        }
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;
        
        TextAsset textFile = Resources.Load("Stage" + Stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);
        
        while(stringReader != null)
        {
            string line = stringReader.ReadLine();

            //Debug.Log(line);
            if (line == null)
                break;
            
           
            EnemySpawn spawnData = new EnemySpawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);

            spawnList.Add(spawnData);

           
        }
        //텍스트 파일 닫기
        stringReader.Close();
        nextSpawnDelay = spawnList[0].delay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }
        if (isenemySpawn == false)
        {
            
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
            {

                SpawnEnemy();
                curSpawnDelay = 0;
            }
            Player playerLogic = Player.GetComponent<Player>();
            PlayerScoreTex.text = "" + playerLogic.Score.ToString("N0");
            Level.text = "Lv : " + playerLogic.Player_Lev;
            Exp.text = "Exp : " + playerLogic.currentExp;
            
            
        } 
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objManager.MakeObj(enemyObjs[enemyIndex]); //소환
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        enemyLogic.Player = Player; //생성된후 넘겨주기
        enemyLogic.gameManager = this;
        enemyLogic.objManager = objManager;

        if (enemyPoint == 0 || enemyPoint == 1 || enemyPoint == 2 || enemyPoint == 3 || enemyPoint == 4)
        {
            rigid.velocity = new Vector3(0, enemyLogic.Speed * (-1));
        }
        //리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }
    public void UpdateLife(int Life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < Life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }

       
    }
    public void GameClear()
    {
        GameClearPanel.SetActive(true);
        isenemySpawn = true;
        isScroll = true;
        Player playerLogic = Player.GetComponent<Player>();
        playerLogic.PlayerShot = true;
        GameClearScore.text = playerLogic.Score.ToString("N0");
    }
    
    public void GameOver()
    {

        isenemySpawn = true;
        isScroll = true;
        Player playerLogic = Player.GetComponent<Player>();
        playerLogic.PlayerShot = true;
        GameOverPanel.SetActive(true);
        GameOverScore.text = playerLogic.Score.ToString("N0");
    }
    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 1.5f);
    }
    
    void RespawnPlayerExe()
    {
        Player.transform.position = Vector3.down * 4f;
        Player.SetActive(true);

        
    }
    public void ReStartButton()
    {
        SceneManager.LoadScene("InGameScene");
    }
    public void ExitButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 900; // 사용자 설정 너비
        int setHeight = 1600; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
