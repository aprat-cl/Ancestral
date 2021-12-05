using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Velocity, JumpForce;
    public Text debugText;
    Vector3 direction;
    Rigidbody rb;
    bool bAllowJump, bAllowMove, bIsDashing, bStartHealing;
    float TimeForBattle, ActualTFB;
    public GameObject PlayerPanel, MainMenu, GoldText, HealthText, ManaText;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bAllowJump = true;
        bAllowMove = true;
        TimeForBattle = UnityEngine.Random.Range(10f, 20f);
        ActualTFB = 0f;
        playerControls.Ground.Jump.performed += onJumpEvent;
        PlayerData.parentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void onJumpEvent(InputAction.CallbackContext obj)
    {
        if (transform.position.y < 0.5f)
        {
            if (playerControls.Ground.Jump.triggered && bAllowJump)
            {
                bAllowJump = false;
                bAllowMove = false;
                bIsDashing = true;
                rb.AddForce(direction * JumpForce, ForceMode.VelocityChange);
                //rb.detectCollisions = false;

                StartCoroutine(StopJumpAfter(0.2f));
                StartCoroutine(EnableJumpAfter(0.5f));
                StartCoroutine(EnableMoveAfter(0.2f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.LoadSaveData)
        {
            SceneManager.LoadScene(PlayerData.parentScene);
            return;
        }
        float moveX = playerControls.Ground.Move.ReadValue<Vector2>().x;
        float moveY = playerControls.Ground.Move.ReadValue<Vector2>().y;
        if (bAllowMove)
        {            
            PlayerData.MapLocX = gameObject.transform.position.x;
            PlayerData.MapLocY = gameObject.transform.position.z;
            direction = new Vector3(moveX, 0f, moveY).normalized;

            rb.velocity = new Vector3(moveX * Velocity, rb.velocity.y, moveY * Velocity);
            debugText.text = "PosX: " + PlayerData.MapLocX.ToString() + " :: PosY: " + PlayerData.MapLocY.ToString() + " :: MaxTime: " + TimeForBattle.ToString() + " :: CurrentTime: " + ActualTFB.ToString();
            if(rb.velocity.magnitude > 0)
            {
                ActualTFB += Time.deltaTime;
                if(ActualTFB > TimeForBattle)
                {                    //LoadBattle
                    
                    TimeForBattle = UnityEngine.Random.Range(15f, 40f);
                    ActualTFB = 0f;
                    PlayerData.parentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("battle");
                }
            }

        }
        GoldText.GetComponent<Text>().text = Mathf.RoundToInt(PlayerData.Gold).ToString() + "G";
        if (playerControls.Ground.Menu.triggered)
        {
            if (!MainMenu.activeSelf)
                MainMenu.SetActive(true);
            else
                MainMenu.SetActive(false);
        }
        if (bStartHealing)
        {
            float unitMHeal = PlayerData.PlayerMana * 0.02f;
            float unitHHeal = PlayerData.PlayerHealth * 0.02f;
            if (PlayerData.ManaUsage > 0)
                PlayerData.ManaUsage -= Time.deltaTime * unitMHeal;
            if(PlayerData.PlayerDamage > 0)
                PlayerData.PlayerDamage -= Time.deltaTime * unitHHeal;
        }
        UpdatePlayerInfo();

        //Vector3 direction = new Vector3(moveX, 0f, moveY);
        //transform.Translate(direction * Velocity * Time.deltaTime);
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.AddForce(direction * Velocity, ForceMode.Acceleration);
    }

    private void UpdatePlayerInfo()
    {
        GameObject healthBar = HealthText.transform.Find("Health").gameObject;
        GameObject manaBar = ManaText.transform.Find("Mana").gameObject;

        GameObject dmgBar = healthBar.transform.Find("Damage").gameObject;
        GameObject usageBar = manaBar.transform.Find("Usage").gameObject;

        GameObject hText = healthBar.transform.Find("Text").gameObject;
        GameObject mText = manaBar.transform.Find("Text").gameObject;

        float hPercent = PlayerData.PlayerDamage * 100 / PlayerData.PlayerHealth;
        float mPercent = PlayerData.ManaUsage * 100 / PlayerData.PlayerMana;

        float hBarTotal = healthBar.GetComponent<RectTransform>().rect.width;
        float mBarTotal = manaBar.GetComponent<RectTransform>().rect.width;

        dmgBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hBarTotal * hPercent / 100);
        usageBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mBarTotal * mPercent / 100);

        hText.GetComponent<Text>().text = Mathf.RoundToInt(PlayerData.PlayerHealth - PlayerData.PlayerDamage).ToString() + " / " + Mathf.RoundToInt(PlayerData.PlayerHealth).ToString();
        mText.GetComponent<Text>().text = Mathf.RoundToInt(PlayerData.PlayerMana - PlayerData.ManaUsage).ToString() + " / " + Mathf.RoundToInt(PlayerData.PlayerMana).ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "door" && bIsDashing)
        {
            Debug.Log("Trigger a door break!");
            Destroy(other.gameObject);
            PlayerData.ManaUsage += 7.5f;
        }
        else if (other.tag == "teleport")
        {
            bStartHealing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if(other.tag == "teleport")
        {
            bStartHealing = false;
        }
    }
    private void FixedUpdate()
    {
        
    }    

    IEnumerator EnableJumpAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        bAllowJump = true;
    }
    IEnumerator EnableMoveAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        bAllowMove = true;
    }
    IEnumerator StopJumpAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb.velocity = Vector3.zero;
        bIsDashing = false;
        //rb.detectCollisions = true;
    }
}
