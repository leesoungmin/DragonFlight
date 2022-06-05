using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyname;
    public int enemyScore;
    public float Speed;
    public int Hp;
    public Sprite[] sprites;
    public float maxShotDelay;
    public float curShotDelay;
    public int exp;


    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    bool BossBulletShot;

    public AudioSource EnemyDie;

    SpriteRenderer SpriteRenderer;

    public GameObject enemyBulletObj1;
    public GameObject enemyBulletObj2;
    public GameObject bossBulletObj;

    public GameObject Player;
    public GameManager gameManager;
    public ObjectManager objManager;
    // Start is called before the first frame update
    void Start()
    {
       
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = sprites[0];
    }

    void OnEnable()
    {
        switch (enemyname)
        {
            case "L":
                Hp = 80;
                break;
            case "M":
                Hp = 50;
                break;
            case "S":
                Hp = 10;
                break;
            case "B":
                Hp = 10000;
                Invoke("Stop", 2);
                break;
        }
        
    }

    void OnHit(int dmg)
    {
        Hp -= dmg;
        SpriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        
        if (Hp <= 0)
        {
            Player playerLogic = Player.GetComponent<Player>();
            playerLogic.Score += enemyScore;
            playerLogic.currentExp += exp;
            CancelInvoke();
            gameObject.SetActive(false);
            if(enemyname == "S" || enemyname == "M" || enemyname == "L")
            {
                gameManager.EnemyDie.Play();
            }
            

            if (enemyname == "B")
            {
                gameManager.StageEnd();
                gameManager.BossDie.Play();
            }
        }
        
    }
    void Update()
    {
        if (enemyname == "B")
            return;

        Fire();
        Reload();
        
    }
    void ReturnSprite()
    {
        SpriteRenderer.sprite = sprites[0];
    }
    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        
        if(gameManager.Stage == 1)
        {
            gameManager.Dangerous.Play();
            gameManager.DangerAni.SetTrigger("On");
            Invoke("Pattern", 4);
        }
        if(gameManager.Stage == 2)
        {
            gameManager.Dangerous.Play();
            gameManager.DangerAni.SetTrigger("On");
            Invoke("Pattern2", 4);
        }
    }
    void Pattern2()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireAround2();
                break;
            case 1:
                FireFoward2();

                break;
            case 2:
                FireShot2();

                break;
            case 3:
                FireArc2();

                break;
        }

    }
    void Pattern()
    {
            patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
            curPatternCount = 0;

            switch (patternIndex)
            {
                case 0:
                    FireAround();
                    break;
                case 1:
                    FireFoward();

                    break;
                case 2:
                    FireShot();

                    break;
                case 3:
                    FireArc();

                    break;
            }
        
        
    }
    void FireFoward2()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        GameObject bullet1 = objManager.MakeObj("BossBullet");
        bullet1.transform.position = transform.position + Vector3.left * 2.5f;
        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        rigid1.AddForce(Vector2.down * 7, ForceMode2D.Impulse);

        GameObject bullet2 = objManager.MakeObj("BossBullet");
        bullet2.transform.position = transform.position + Vector3.left * 1f;
        Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
        rigid2.AddForce(Vector2.down * 7, ForceMode2D.Impulse);

        GameObject bullet3 = objManager.MakeObj("BossBullet");
        bullet3.transform.position = transform.position;
        Rigidbody2D rigid3 = bullet3.GetComponent<Rigidbody2D>();
        rigid3.AddForce(Vector2.down * 7, ForceMode2D.Impulse);

        GameObject bullet4 = objManager.MakeObj("BossBullet");
        bullet4.transform.position = transform.position + Vector3.right * 1f;
        Rigidbody2D rigid4 = bullet4.GetComponent<Rigidbody2D>();
        rigid4.AddForce(Vector2.down * 7, ForceMode2D.Impulse);

        GameObject bullet5 = objManager.MakeObj("BossBullet");
        bullet5.transform.position = transform.position + Vector3.right * 2.5f;
        Rigidbody2D rigid5 = bullet5.GetComponent<Rigidbody2D>();
        rigid5.AddForce(Vector2.down * 7, ForceMode2D.Impulse);


        Debug.Log("앞으로 5발 발사");
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward2", 3);
        }
        else
        {
            Invoke("Pattern2", 2);
        }
    }
    void FireFoward()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        GameObject bullet1 = objManager.MakeObj("BossBullet");
        bullet1.transform.position = transform.position + Vector3.left * 1f;
        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        rigid1.AddForce(Vector2.down * 5, ForceMode2D.Impulse);

        GameObject bullet2 = objManager.MakeObj("BossBullet");
        bullet2.transform.position = transform.position + Vector3.left * 0.5f;
        Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
        rigid2.AddForce(Vector2.down * 5, ForceMode2D.Impulse);

        GameObject bullet3 = objManager.MakeObj("BossBullet");
        bullet3.transform.position = transform.position;    
        Rigidbody2D rigid3 = bullet3.GetComponent<Rigidbody2D>();
        rigid3.AddForce(Vector2.down * 5, ForceMode2D.Impulse);

        GameObject bullet4 = objManager.MakeObj("BossBullet");
        bullet4.transform.position = transform.position + Vector3.right * 0.5f;
        Rigidbody2D rigid4 = bullet4.GetComponent<Rigidbody2D>();
        rigid4.AddForce(Vector2.down * 5, ForceMode2D.Impulse);

        GameObject bullet5 = objManager.MakeObj("BossBullet");
        bullet5.transform.position = transform.position + Vector3.right * 1f;
        Rigidbody2D rigid5 = bullet5.GetComponent<Rigidbody2D>();
        rigid5.AddForce(Vector2.down * 5, ForceMode2D.Impulse);


        Debug.Log("앞으로 5발 발사");
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 2);
        }
        else
        {
            Invoke("Pattern", 2);
        }
    }
    void FireShot()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        for (int index = 0; index  < 5; index++)
        {
            GameObject shotbullet = objManager.MakeObj("BossBullet");
            shotbullet.transform.position = transform.position;

            Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = Player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f ,2f));
            dirVec += ranVec;
            shotrigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }
        Debug.Log("플레이어 방향으로 샷권 발사");

        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot", 1.5f);
        }
        else
        {
            Invoke("Pattern", 2);
        }
    }
    void FireShot2()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        for (int index = 0; index < 5; index++)
        {
            GameObject shotbullet = objManager.MakeObj("BossBullet");
            shotbullet.transform.position = transform.position;

            Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = Player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-2f, 2f), Random.Range(0f, 2f));
            dirVec += ranVec;
            shotrigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);
        }
        Debug.Log("플레이어 방향으로 샷권 발사");

        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot2", 1);
        }
        else
        {
            Invoke("Pattern2", 2);
        }
    }
    void FireArc()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        GameObject shotbullet = objManager.MakeObj("BossBullet");
            shotbullet.transform.position = transform.position;
            shotbullet.transform.rotation = Quaternion.identity;

            Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 8 * curPatternCount / maxPatternCount[patternIndex]), -1);
            shotrigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        
        Debug.Log("부채모양으로 발사");
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.2f);
        }
        else
        {
            Invoke("Pattern", 2);
        }
    }
    void FireArc2()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (Hp <= 0) return;
        if (playerLogic.Life <= 0) return;

        GameObject shotbullet = objManager.MakeObj("BossBullet");
        shotbullet.transform.position = transform.position;
        shotbullet.transform.rotation = Quaternion.identity;

        Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 8 * curPatternCount / maxPatternCount[patternIndex]), -1);
        shotrigid.AddForce(dirVec.normalized * 6, ForceMode2D.Impulse);

        Debug.Log("부채모양으로 발사");
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc2", 0.1f);
        }
        else
        {
            Invoke("Pattern2", 2);
        }
    }
    void FireAround()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (playerLogic.Life <= 0) return;
        if (Hp <= 0) return;

        int round = 50;
        int roundA = 40;
        int roundB = curPatternCount % 2 == 0 ? round : roundA;
        for (int index = 0; index < roundB; index++)
        {
            GameObject shotbullet = objManager.MakeObj("BossBullet");
            shotbullet.transform.position = transform.position;
            shotbullet.transform.rotation = Quaternion.identity;

            Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundB), Mathf.Sin(Mathf.PI * 2 * index / roundB));
            shotrigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
        }
        Debug.Log("원 형태로 전체 공격");
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround", 1f);
        }
        else
        {
            Invoke("Pattern", 4);
        }
    }
    void FireAround2()
    {
        Player playerLogic = Player.GetComponent<Player>();
        if (playerLogic.Life <= 0) return;
        if (Hp <= 0) return;

        int round = 50;
        int roundA = 40;
        int roundB = curPatternCount % 2 == 0 ? round : roundA;
        for (int index = 0; index < roundB; index++)
        {
            GameObject shotbullet = objManager.MakeObj("BossBullet");
            shotbullet.transform.position = transform.position;
            shotbullet.transform.rotation = Quaternion.identity;

            Rigidbody2D shotrigid = shotbullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundB), Mathf.Sin(Mathf.PI * 2 * index / roundB));
            shotrigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
        }
        Debug.Log("원 형태로 전체 공격");
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround2", 1f);
        }
        else
        {
            Invoke("Pattern2", 4);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BulletBorder" && enemyname != "B")
        {
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
       

            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);


            collision.gameObject.SetActive(false);


        }
    }
    private void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;
        
        if(enemyname == "M")
        {
            GameObject bullet = objManager.MakeObj("EnemyBullet1");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            //Vector3 dirVec = Player.transform.position - transform.position;

            rigid.AddForce(Vector2.down * 10 , ForceMode2D.Impulse);
        }
        else if(enemyname == "L")
        {
            GameObject bullet = objManager.MakeObj("EnemyBullet2");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = Player.transform.position - transform.position;

            rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }
    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
