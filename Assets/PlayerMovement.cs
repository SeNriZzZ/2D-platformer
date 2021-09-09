using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    float horizontalMove = 0f;

    bool jump = false;

    bool crouch = false;


    public float runSpeed = 40f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("Jumping", false);
    }

    public void OnCrouching(bool Crouching)
    {
        animator.SetBool("Crouching", Crouching);
    }

    private float score = 0;
    private float CoinValue = 1;

    public Text textCoins;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            score += CoinValue;
            textCoins.text = "x " + score.ToString() + "/10";
        }

        if (other.gameObject.CompareTag("Water"))
        {
            animator.SetBool("isDead", true);
            Invoke("LoadLevel", 0.5f);
            score = 0;
        }

        if (other.gameObject.CompareTag("LevelEnd"))
        {
            if (score == 10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                score = 0;
            }
        }
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("RestartMenu");
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}