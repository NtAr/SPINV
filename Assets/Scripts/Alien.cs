using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{
    public int speed = 10;

    public Rigidbody2D rigidBody;

    public Sprite startingImage;

    public Sprite altImage;

    private SpriteRenderer spriteRenderer;

    public float secBeforeSpriteChange = 0.5f;

    public GameObject alienBullet;

    public float minFireRateTime = 1.0f;

    public float maxFireRateTime = 6.0f;

    public float baseFireWaitTime = 3.0f;

    public Sprite explodedShipImage;

    private bool isMovingRight = true;


    void Start()
    {


        rigidBody = GetComponent<Rigidbody2D>();

        InvokeRepeating("MoveRight", 0.3f, 0.5f);

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(changeAlienSprite());

        baseFireWaitTime = baseFireWaitTime +
            Random.Range(minFireRateTime, maxFireRateTime);

    }
    void MoveRight()
    {
        transform.position += Vector3.right * Time.deltaTime * speed;

        if(!isMovingRight)
        {
            CancelInvoke("MoveRight");
            InvokeRepeating("MoveLeft", 0.3f, 0.5f);
        }
    }
    void MoveLeft()
    {
        transform.position -= Vector3.right * Time.deltaTime * speed;
        if (isMovingRight)
        {
            CancelInvoke("MoveLeft");
            InvokeRepeating("MoveRight", 0.3f, 0.5f);
        }
    }

    void MoveDown()
    {
        Vector2 position = transform.position;
        position.y -= 5;
        transform.position = position;
    }

    void FixedUpdate()
    {
        if (Time.time > baseFireWaitTime)
        {

            baseFireWaitTime = baseFireWaitTime +
                Random.Range(minFireRateTime, maxFireRateTime);

            Instantiate(alienBullet, transform.position, Quaternion.identity);

        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        

        if (col.gameObject.name == "LeftWall")
        {
            MoveDown();
            speed *= 1;
            isMovingRight = true;
        }
        if (col.gameObject.name == "RightWall")
        {
            MoveDown();  
            isMovingRight = false;
        }

        if (col.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
        var aliens = GameObject.FindGameObjectsWithTag("Alien");


    }

    public IEnumerator changeAlienSprite()
    {
        while (true)
        {
            if (spriteRenderer.sprite == startingImage)
            {
                spriteRenderer.sprite = altImage;
            }
            else
            {
                spriteRenderer.sprite = startingImage;
            }

            yield return new WaitForSeconds(secBeforeSpriteChange);
        }
    }

    


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;

            Destroy(gameObject);
            DestroyObject(col.gameObject, 0.3f);


        }
    }  
}
