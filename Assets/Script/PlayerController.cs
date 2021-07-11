using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private float _firstLine;
    [SerializeField] private float _secondline;
    [SerializeField] private float _thirdLine;

    [SerializeField] private float _moveThreshold;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveSpeed;
    public string currentTag;



    public GameManager gameManager;
    

    [SerializeField] private Text textScore;
    private int score=0;

    private Rigidbody rb;

    private float _lastMoveTime;

    private Lane _lane = Lane.Second;
    enum Lane
    {
        First,
        Second,
        Third

    }

    Vector3 moveTo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        //character movements
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            float movePow = touch.deltaPosition.normalized.x;

            if (Mathf.Abs(movePow) > _moveThreshold && Time.time - _lastMoveTime > 0.5f)
            {
                _lastMoveTime = Time.time;
                if (movePow < 0)
                {

                    switch (_lane)
                    {
                        case Lane.First:
                            break;
                        case Lane.Second:
                            moveTo = new Vector3(_firstLine, 0, transform.position.z);
                            _lane = Lane.First;
                            break;
                        case Lane.Third:
                            moveTo = new Vector3(_secondline, 0, transform.position.z);
                            _lane = Lane.Second;
                            break;
                    }
                }
                if (movePow > 0)
                {
                    switch (_lane)
                    {
                        case Lane.First:

                            moveTo = new Vector3(_secondline, 0, transform.position.z);
                            _lane = Lane.Second;
                            break;
                        case Lane.Second:
                            moveTo = new Vector3(_thirdLine, 0, transform.position.z);
                            _lane = Lane.Third;
                            break;
                        case Lane.Third:
                            break;
                    }
                }

            }
        }

        
        Move(moveTo);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * (Time.deltaTime * _moveSpeed);
    }

    private void Move(Vector3 moveTo)
    {
        moveTo = new Vector3(moveTo.x, 0, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * _speed);
    }


    private void OnCollisionEnter (Collision col)//When the player touches an object of the same color, it destroys it.
    {
         if (col.gameObject.tag == currentTag)
         {
             Destroy(col.gameObject);

             score++;
             textScore.text = "" + score;
         }

        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restarts the scene when game over

        }
    }

    private void OnTriggerEnter(Collider other) //When you reach the finish line, the menu opens.
    {
        if (other.gameObject.tag == "Finish")
        {
            gameManager.finishMenu.SetActive(true);
           // Debug.Log("gecti");
        }
    }

   


}

