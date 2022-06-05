using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Speed;
    public float maxShotDelay;
    public float curShotDelay;
    public float Power = 1;
    public int[] needExp;
    public int currentExp;
    public int Player_Lev;
    public bool ScoreUp;
    
    public int Life;
    public float Score;
    public bool PlayerShot = false;

    public AudioSource Attack;
    
    public bool isRespawnTime;

    public GameObject bulletObj1;
    public GameObject bulletObj2;
    public SpriteRenderer sprite;

    public GameManager gameManager;
    public ObjectManager objectManager;
    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        
        Player_Lev = 1;
    }
    void Awake()
    {
        SpriteRenderer spriterenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Bug();

        PlayerMove();
        if(PlayerShot == false)
        {
            Fire();

        }
        ScreenChk();
        Reload();
        LevelUp();

        if (currentExp == 1200)
            return;

        if(ScoreUp == false)
        {
            Score += Time.deltaTime;
        }
    }

    void LevelUp()
    {
        if (Player_Lev == 5)
            return;
        if(currentExp >= needExp[Player_Lev])
        {
            Player_Lev++;
            Power++;
            
        }
    }
    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    private void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        Attack.Play();

        switch (Power)
        {
            case 1:
                //한발
                GameObject bullet = objectManager.MakeObj("PlayerBullet");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                //두발 
                GameObject bulletR = objectManager.MakeObj("PlayerBullet");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletL = objectManager.MakeObj("PlayerBullet");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = objectManager.MakeObj("PlayerBullet");
                bulletRR.transform.position = transform.position + Vector3.right * 0.3f;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletCC = objectManager.MakeObj("PlayerBullet");
                bulletCC.transform.position = transform.position;
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletLL = objectManager.MakeObj("PlayerBullet");
                bulletLL.transform.position = transform.position + Vector3.left * 0.3f;
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 4:
                GameObject bulletCCC = objectManager.MakeObj("PlayerBullet2");
                bulletCCC.transform.position = transform.position;
                Rigidbody2D rigidCCC = bulletCCC.GetComponent<Rigidbody2D>();
                rigidCCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 5:
                GameObject bulletRRRR = objectManager.MakeObj("PlayerBullet");
                bulletRRRR.transform.position = transform.position + Vector3.right * 0.5f;
                Rigidbody2D rigidRRRR = bulletRRRR.GetComponent<Rigidbody2D>();
                rigidRRRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletCCCC = objectManager.MakeObj("PlayerBullet2");
                bulletCCCC.transform.position = transform.position;
                Rigidbody2D rigidCCCC = bulletCCCC.GetComponent<Rigidbody2D>();
                rigidCCCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                GameObject bulletLLLL = objectManager.MakeObj("PlayerBullet");
                bulletLLLL.transform.position = transform.position + Vector3.left * 0.5f;
                Rigidbody2D rigidLLLL = bulletLLLL.GetComponent<Rigidbody2D>();
                rigidLLLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
                
        }
         
        
        curShotDelay = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime) //무적
                return;

            Life--;
            gameManager.UpdateLife(Life);
            gameManager.RespawnPlayer();
            
           if(Life == 0)
            {
                gameManager.GameOver();
            }
        
            
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "EnemyBullet")
        {

            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * Speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }
    IEnumerator scoreTimer()
    {
        while(true)
        {
            Score++;

            yield return new WaitForSeconds(1f);
        }
        
    }
    void Bug()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isRespawnTime = true;
            if (isRespawnTime)
            {
                sprite.color = new Color(1, 1, 1, 0.5f);
            }
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            isRespawnTime = false;
            if (!isRespawnTime)
            {
                sprite.color = new Color(1, 1, 1, 1);
            }
        }
    }
    void OnEnable()
    {
        Unbeatable();

        Invoke("Unbeatable", 3f);
    }
    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if(isRespawnTime)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
    

    private void ScreenChk()
    {
        Vector3 worlpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worlpos.x < 0.05f) worlpos.x = 0.05f;
        if (worlpos.y < 0.05f) worlpos.y = 0.05f;
        if (worlpos.x > 0.95f) worlpos.x = 0.95f;
        if (worlpos.y > 0.95f) worlpos.y = 0.95f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worlpos);
    }
}
