using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static RoomInstance;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Velocity, JumpForce;
    public Text debugText;
    Vector3 direction;
    int ItemPage = 0;
    Rigidbody rb;
    bool bAllowJump, bAllowMove, bIsDashing, bStartHealing, bAllowOpenChest;//, bMenuOpen;
    float TimeForBattle, ActualTFB;
    public GameObject MainMenu, GoldText, HealthText, ManaText, Inventory, BagPanel, EquipPanel, StatsLeftPnl, StatsRightPnl, ChestPanel;
    public static List<GameObject> menuButtons;
    public static int SelectedButton = 0;
    public RoomType roomType;
    

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Ground.Jump.performed += onJumpEvent;
        playerControls.Ground.Action.performed += onActionEvent;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Ground.Jump.performed -= onJumpEvent;
        playerControls.Ground.Action.performed -= onActionEvent;
    }

    void Start()
    {
        menuButtons = new List<GameObject>();
        menuButtons.Add(MainMenu.transform.Find("Panel").transform.Find("SaveBtn").gameObject);
        menuButtons.Add(MainMenu.transform.Find("Panel").transform.Find("LoadBtn").gameObject);
        menuButtons.Add(MainMenu.transform.Find("Panel").transform.Find("ExitBtn").gameObject);

        menuButtons[0].GetComponent<Button>().Select();

        rb = GetComponent<Rigidbody>();
        bAllowJump = true;
        bAllowMove = true;
        TimeForBattle = UnityEngine.Random.Range(10f, 20f);
        ActualTFB = 0f;
        
        PlayerData.parentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void onJumpEvent(InputAction.CallbackContext obj)
    {
        if (transform.position.y < 0.5f && bAllowMove && !Inventory.activeSelf && !MainMenu.activeSelf)
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
    private void onActionEvent(InputAction.CallbackContext obj)
    {
        if (transform.position.y < 0.5f && bAllowMove && !Inventory.activeSelf && !MainMenu.activeSelf)
        {
            if(bAllowOpenChest)
            {
                if(PlayerData.CurrentRoom.chest.bHasTreasure && !PlayerData.CurrentRoom.chest.bIsOpen)
                {
                    var item = PlayerData.ItemCollection[PlayerData.CurrentRoom.chest.ItemCode];
                    if (PlayerData.Bag.ContainsKey(PlayerData.CurrentRoom.chest.ItemCode))
                    {
                        PlayerData.Bag[PlayerData.CurrentRoom.chest.ItemCode]+= Mathf.RoundToInt(PlayerData.CurrentRoom.chest.Qty);
                    }
                    else
                    {
                        PlayerData.Bag.Add(PlayerData.CurrentRoom.chest.ItemCode,Mathf.RoundToInt(PlayerData.CurrentRoom.chest.Qty));
                    }
                    PlayerData.CurrentRoom.chest.bIsOpen = true;

                    GameObject itemContainer = ChestPanel.transform.Find("Content").transform.Find("ItemContainer").gameObject;
                    itemContainer.transform.Find("Text").GetComponent<Text>().text = item.Name;
                    itemContainer.transform.Find("CantContainer").transform.Find("Text").GetComponent<Text>().text = Mathf.RoundToInt(PlayerData.CurrentRoom.chest.Qty).ToString();
                    itemContainer.transform.Find("Icon").GetComponent<RawImage>().texture = TerrainGenerator.LoadPNG(string.Format(@".\Assets\Sprites\items\{0}.png", item.IconName));

                    ChestPanel.SetActive(true);
                    StartCoroutine(CloseChestAfter(3f));
                }                
            }
        }
    }

    private IEnumerator CloseChestAfter(float time)
    {
        yield return new WaitForSeconds(time);
        ChestPanel.SetActive(false);
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
        if (bAllowMove && !Inventory.activeSelf && !MainMenu.activeSelf)
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
        //else if (bMenuOpen)
        //{
        //    //GameObject button = menuButtons[SelectedButton];
        //    //if (bMenuCanMove)
        //    //{
        //    //    if (playerControls.Ground.Action.triggered)
        //    //    {
        //    //        bMenuCanMove = false;
        //    //        var eventSystem = EventSystem.current;
        //    //        ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        //    //        //StartCoroutine(EnableMenuMove(1f));
        //    //    }
        //    //    else if (playerControls.Ground.Move.ReadValue<Vector2>().y > 0.5f)
        //    //    {
        //    //        bMenuCanMove = false;
        //    //        SelectedButton--;
        //    //        if (SelectedButton < 0) SelectedButton = menuButtons.Count - 1;                   

        //    //        //StartCoroutine(EnableMenuMove(1f));
        //    //    }
        //    //    else if (playerControls.Ground.Move.ReadValue<Vector2>().y < -0.5f)
        //    //    {
        //    //        bMenuCanMove = false;
        //    //        SelectedButton++;
        //    //        if (SelectedButton >= menuButtons.Count) SelectedButton = 0;                   

        //    //        //StartCoroutine(EnableMenuMove(1f));
        //    //    }
        //    //    else
        //    //    {
        //    //        bMenuCanMove = true;
        //    //    }
        //    //    button = menuButtons[SelectedButton];
        //    //    button.GetComponent<Button>().Select();
        //    //}

        //}
        GoldText.GetComponent<Text>().text = Mathf.RoundToInt(PlayerData.Gold).ToString() + "G";
        if (playerControls.Ground.Menu.triggered)
        {
            if (!MainMenu.activeSelf)
            {
                menuButtons[0].GetComponent<Button>().Select();
                //bMenuOpen = true;
                bAllowMove = false;
                bAllowJump = false;
                rb.velocity = Vector3.zero;
                MainMenu.SetActive(true);
            }
            else
            {
                //bMenuOpen = false;
                bAllowMove = true;
                bAllowJump = true;
                MainMenu.SetActive(false);
            }
        }
        if (playerControls.Ground.Inventory.triggered)
        {
            if (!Inventory.activeSelf)
            {                
                PrepareInventory();
                PrepareEquip();
                //bMenuOpen = true;                
                bAllowMove = false;
                bAllowJump = false;
                rb.velocity = Vector3.zero;
                Inventory.SetActive(true);
            }
            else
            {
                //bMenuOpen = false;
                UnloadInventory();
                bAllowMove = true;
                bAllowJump = true;
                Inventory.SetActive(false);
            }
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

    private void UnloadInventory()
    {
        for(int i = 1; i <= 10; i++)
        {
            GameObject itemPnl = BagPanel.transform.Find(string.Format("ItemContainer{0}", i)).gameObject;
            itemPnl.SetActive(false);
        }
    }

    private void PrepareEquip()
    {
        //Equipment Section
        GameObject weaponPnl = EquipPanel.transform.Find("WeaponContainer").gameObject;
        var wItem = PlayerData.ItemCollection[PlayerData.weapon.Code];

        weaponPnl.transform.Find("Text").gameObject.GetComponent<Text>().text = PlayerData.weapon.Name;
        weaponPnl.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = TerrainGenerator.LoadPNG(String.Format(@".\Assets\Sprites\weapons\{0}.png", wItem.IconName));

        GameObject armourPnl = EquipPanel.transform.Find("ArmourContainer").gameObject;
        var aItem = PlayerData.ItemCollection[PlayerData.armour.Code];

        armourPnl.transform.Find("Text").gameObject.GetComponent<Text>().text = PlayerData.armour.Name;
        armourPnl.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = TerrainGenerator.LoadPNG(String.Format(@".\Assets\Sprites\armour\{0}.png", aItem.IconName));

        // Stats Section

        StatsLeftPnl.transform.Find("LvlStatTxt").gameObject.GetComponent<Text>().text = String.Format("{0} : Level", PlayerData.Level.ToString());
        StatsLeftPnl.transform.Find("StrStatTxt").gameObject.GetComponent<Text>().text = String.Format("{0}({1}) : Strenght", PlayerData.Attackpow.ToString(), PlayerData.weapon.type == WeaponType.Melee || PlayerData.weapon.type == WeaponType.Ranged? (PlayerData.Attackpow + PlayerData.weapon.Power1).ToString(): PlayerData.Attackpow.ToString());
        StatsLeftPnl.transform.Find("DefStatTxt").gameObject.GetComponent<Text>().text = String.Format("{0}({1}) : Defence", PlayerData.Defence.ToString(), (PlayerData.Defence + PlayerData.armour.Def).ToString());
        StatsLeftPnl.transform.Find("SpdStatTxt").gameObject.GetComponent<Text>().text = String.Format("{0}({1}) : Speed", PlayerData.AttSpeed.ToString(), (PlayerData.AttSpeed + PlayerData.armour.SpeedBonus).ToString());

        //StatsRightPnl.transform.Find("LvlStatTxt").gameObject.GetComponent<Text>().text = String.Format("{0} : Level", PlayerData.Level.ToString());
        StatsRightPnl.transform.Find("MagStatTxt").gameObject.GetComponent<Text>().text = String.Format("Magic : {0}({1})", PlayerData.MagPow.ToString(), PlayerData.weapon.type == WeaponType.MagicMele || PlayerData.weapon.type == WeaponType.MagicRanged ? (PlayerData.MagPow + PlayerData.weapon.Power1 + PlayerData.armour.MBonus).ToString() : PlayerData.MagPow.ToString());
        StatsRightPnl.transform.Find("MDefStatTxt").gameObject.GetComponent<Text>().text = String.Format("Mag Def. : {0}({1})", PlayerData.MagicDef.ToString(), (PlayerData.MagicDef + PlayerData.armour.MagDef).ToString());
        StatsRightPnl.transform.Find("AccStatTxt").gameObject.GetComponent<Text>().text = String.Format("Acuracy : {0}({1})", PlayerData.AttAcc.ToString(), (PlayerData.AttAcc + PlayerData.weapon.AccBonus).ToString());

    }

    private void PrepareInventory()
    {
        List<ItemData> lstItems = new List<ItemData>();
        List<int> lstCant = new List<int>();
        foreach(var item in PlayerData.Bag)
        {
            lstItems.Add(PlayerData.ItemCollection[item.Key]);
            lstCant.Add(item.Value);
        }
        int pos = 1;
        for(int i = 0; i < 10; i++)
        {
            int idx = i + (ItemPage * 10);
            if (idx >= lstItems.Count) break;
            else
            {                
                GameObject itemPnl = BagPanel.transform.Find(string.Format("ItemContainer{0}", pos)).gameObject;
                itemPnl.transform.Find("Text").gameObject.GetComponent<Text>().text = lstItems[idx].Name;
                itemPnl.transform.Find("CantContainer").transform.Find("Text").gameObject.GetComponent<Text>().text = lstCant[idx].ToString();

                itemPnl.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = TerrainGenerator.LoadPNG(String.Format(@".\Assets\Sprites\items\{0}.png", lstItems[i].IconName));
                itemPnl.SetActive(true);
                pos++;
            }
        }        
    }

    //private IEnumerator EnableMenuMove(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    bMenuCanMove = true;
    //}

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
        else if(other.tag == "chest")
        {
            bAllowOpenChest = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if(other.tag == "teleport")
        {
            bStartHealing = false;
        }
        else if(other.tag == "chest")
        {
            bAllowOpenChest = false;
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
