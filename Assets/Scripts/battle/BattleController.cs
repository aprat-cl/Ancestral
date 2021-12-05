using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EnemyController;
using static RoomInstance;

public class BattleController : MonoBehaviour
{
    public Transform CameraTarget;
    ArrayList enemies = new ArrayList(), enemyPanels = new ArrayList(); 
    List<bool> enemyKills = new List<bool>();
    List<float> EnemyTimeAtack = new List<float>();
    Hashtable enemyNames = new Hashtable();
    int EnemySelected = 0;
    bool bAllowMove = true, bAllowAttack = true, bBattleEnded = false;
    public GameObject PlayerPanel, ItemsPanel, GoldText;
    float CurrTimePlayerAtt = 0;
    // Start is called before the first frame update
    private PlayerControls playerControls;
    private float goldEarn;
    List<ItemCodes> itemEarn;

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
        bBattleEnded = false;
        transform.Find("BattleCover").GetComponent<Canvas>().gameObject.SetActive(false);
        //PlayerData.CurrentRoom = RoomInstance.CreateRoom(0x0, Connector.None);
        //PlayerData.CurrentRoom.type = RoomType.Initial;
        //PlayerData.PlayerModelName = "Player_default";        

        StartCoroutine(PopulateEnemies());
        PlacePlayer();

    }

    private void PlacePlayer()
    {
        var player = GetPlayerModel(PlayerData.PlayerModelName);
        player.transform.position = new Vector3(0f, 0.6f, -8f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CameraTarget);
        int PrevSelected = EnemySelected;
        bool bChanged = false;
        
        if (!bBattleEnded)
        {
            if (playerControls.Battle.Select.ReadValue<Vector2>().y > 0.5 && bAllowMove)
            {
                bAllowMove = false;
                bChanged = true;
                EnemySelected--;
                if (EnemySelected < 0)
                {
                    EnemySelected = enemies.Count + EnemySelected;
                }
                while (enemyKills[EnemySelected])
                {
                    EnemySelected--;
                    if (EnemySelected < 0)
                    {
                        EnemySelected = enemies.Count + EnemySelected;
                    }
                }
                StartCoroutine(AllowMoveAfter(0.33f));
            }
            else if (playerControls.Battle.Select.ReadValue<Vector2>().y < -0.5 && bAllowMove)
            {
                bAllowMove = false;
                bChanged = true;
                EnemySelected++;

                if (EnemySelected > enemies.Count - 1)
                {
                    EnemySelected = EnemySelected - enemies.Count;
                }
                while (enemyKills[EnemySelected])
                {
                    EnemySelected++;
                    if (EnemySelected > enemies.Count - 1)
                    {
                        EnemySelected = EnemySelected - enemies.Count;
                    }
                }
                StartCoroutine(AllowMoveAfter(0.33f));
            }
            if (bChanged)
            {
                var EnemyPanelPrev = getEnemyPanel(PrevSelected).GetComponent<Image>();
                var EnemyPanelAct = getEnemyPanel(EnemySelected).GetComponent<Image>();

                EnemyPanelPrev.color = new Color(1f, 1f, 1f, 0.39f);
                EnemyPanelAct.color = new Color(0f, 1f, 0.095f, 0.51f);
                
            }

            //PLAYER ATACK REGION
            GameObject turnBar = PlayerPanel.transform.Find("Turn").gameObject;
            GameObject timeBar = turnBar.transform.Find("Time").gameObject;
            GameObject manaBar = PlayerPanel.transform.Find("BaseMana").gameObject;
            //GameObject manaUsage = manaBar.transform.Find("Damage").gameObject;
            float AttUsage = 0;
            if (bAllowAttack)
            {
                timeBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
                CurrTimePlayerAtt = 0;
                if (playerControls.Battle.AttackLight.triggered)
                {
                    if (PlayerData.PlayerMana - PlayerData.ManaUsage > PlayerData.weapon.Pow1Usage)
                    {
                        //Mele
                        if (UpdateEnemyDamage(PlayerData.weapon.Power1 * PlayerData.Attackpow))
                        {
                            bAllowAttack = false;
                            AttUsage = PlayerData.weapon.Pow1Usage;
                        }
                        StartCoroutine(AllowAttackAfter(10 - PlayerData.AttSpeed));
                        StartCoroutine(CheckBattleEnded());
                    }
                    else
                    {
                        StartCoroutine(AnimateLackMana(2f, manaBar));
                    }
                }
                else if (playerControls.Battle.AttackHard.triggered)
                {
                    //Ranged or Hard
                    if (PlayerData.PlayerMana - PlayerData.ManaUsage > PlayerData.weapon.Pow2Usage)
                    {
                        if (UpdateEnemyDamage(PlayerData.weapon.Power2 * PlayerData.Attackpow))
                        {
                            bAllowAttack = false;
                            AttUsage = PlayerData.weapon.Pow2Usage;
                        }
                        StartCoroutine(AllowAttackAfter(10 - PlayerData.AttSpeed));
                        StartCoroutine(CheckBattleEnded());
                    }
                    else
                    {
                        StartCoroutine(AnimateLackMana(2f, manaBar));
                    }
                }
                else if (playerControls.Battle.AttackSpecial.triggered)
                {
                    if (PlayerData.PlayerMana - PlayerData.ManaUsage > PlayerData.weapon.SpecialManaUsage)
                    {
                        //Special
                        if (UpdateEnemyDamage(PlayerData.weapon.PowerSpecial * PlayerData.Attackpow))
                        {
                            bAllowAttack = false;
                            AttUsage = PlayerData.weapon.SpecialManaUsage;
                        }
                        StartCoroutine(AllowAttackAfter(10 - PlayerData.AttSpeed));
                        StartCoroutine(CheckBattleEnded());
                    }
                    else
                    {
                        StartCoroutine(AnimateLackMana(2f, manaBar));
                    }
                }
                if (!bAllowAttack)
                {
                    PlayerData.ManaUsage += AttUsage;
                    //float percent = PlayerData.ManaUsage * 100 / PlayerData.PlayerMana;
                    //float totalManaBar = manaBar.GetComponent<RectTransform>().rect.width;
                    //float value = totalManaBar * percent / 100;
                    //manaUsage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                }
            }
            else
            {
                float totalTime = turnBar.GetComponent<RectTransform>().rect.width;
                float timeValue = 10 - PlayerData.AttSpeed;
                CurrTimePlayerAtt += Time.deltaTime;

                var percent = CurrTimePlayerAtt * 100 / timeValue;

                var ActValue = totalTime * percent / 100;

                timeBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalTime - ActValue);
            }
            //ENEMY ATTACK REGION
            for(int i = 0; i < enemies.Count; i++)
            {
                if (!enemyKills[i])
                {
                    var ec = ((GameObject)enemies[i]).GetComponent<EnemyController>();
                    EnemyTimeAtack[i] += Time.deltaTime;
                    float EnemyTime = (10 - ec.AttSpeed) < 1 ? 1 : (10 - ec.AttSpeed) * Random.Range(0.8f, 1.1f);
                    if(EnemyTimeAtack[i] > EnemyTime)
                    {
                        EnemyTimeAtack[i] = 0;
                        //TimeToAttack
                        float att_type = Random.Range(0f, 1.001f);
                        float att_chance, att_dmg;
                        float chance = Random.Range(0f, 1.001f);
                        AttackType attackType;
                        if(att_type < 0.05)
                        {
                            att_chance = ec.AccAttS;
                            att_dmg = ec.Special;
                            attackType = ec.AttackTypeS;
                        }
                        else if (att_type < 0.35)
                        {
                            att_chance = ec.AccAtt2;
                            att_dmg = ec.AttackPow2;
                            attackType = ec.AttackType2;
                        }
                        else
                        {
                            att_chance = ec.AccAtt1;
                            att_dmg = ec.AttackPow1;
                            attackType = ec.AttackType1;
                        }
                        if(chance < att_chance)
                        {//HIT!
                            float PlayerDef = attackType == AttackType.Mele || attackType == AttackType.Ranged ? PlayerData.Defense : PlayerData.MagicDefense;
                            PlayerData.PlayerDamage += att_dmg - ((PlayerDef / 10) * att_dmg);
                        }
                    }
                }
            }
            StartCoroutine(UpdatePlayerInfo());
        }
            
    }

    private IEnumerator AnimateLackMana(float time, GameObject panel)
    {
        bool red = true;
        for(float i = 0; i < time * 2f; i += 0.500f)
        {
            panel.GetComponent<Image>().color = red ? Color.red : new Color(0, 0.6320754f, 0.1693114f, 1);
            red = !red;
            yield return new WaitForSeconds(0.500f);
        }
    }

    private IEnumerator UpdatePlayerInfo()
    {
        GameObject healthBar = PlayerPanel.transform.Find("BaseLife").gameObject;
        GameObject manaBar = PlayerPanel.transform.Find("BaseMana").gameObject;

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

        yield return new WaitForSeconds(0.01f);
    }

    private bool UpdateEnemyDamage(float Damage)
    {
        if (!(bool)enemyKills[EnemySelected])
        {
            GameObject panel = (GameObject)enemyPanels[EnemySelected];
            GameObject dmgBar = panel.transform.Find("Damage").gameObject;
            GameObject lifeBar = panel.transform.Find("BaseLife").gameObject;
            //float currDmg = dmgBar.GetComponent<RectTransform>().rect.width;
            float currLife = lifeBar.GetComponent<RectTransform>().rect.width;
            GameObject enemy = (GameObject)enemies[EnemySelected];
            var ec = enemy.GetComponent<EnemyController>();

            ec.Damage += Damage;
            if (ec.Damage >= ec.Health)
            {
                ec.Damage = ec.Health;
                PlayerData.Gold += ec.Experience;
                goldEarn += ec.Experience;
                if (ec.ItemHeld != ItemCodes.None)
                {
                    itemEarn.Add(ec.ItemHeld);
                    if (PlayerData.Bag.ContainsKey(ec.ItemHeld))
                    {
                        int cant = (int)PlayerData.Bag[ec.ItemHeld];
                        PlayerData.Bag[ec.ItemHeld] = cant + 1;
                    }
                    else
                    {
                        PlayerData.Bag.Add(ec.ItemHeld, 1);
                    }
                }
                //Death
                panel.SetActive(false);
                Destroy(enemy);
                enemyKills[EnemySelected] = true;
            }

            float percent = ec.Damage * 100 / ec.Health;

            dmgBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currLife * percent / 100);
            return true;
        }
        else return false;
    }
    private GameObject getEnemyPanel(int idx)
    {
        GameObject Parent = transform.Find("BattleDisp").transform.Find("Background").gameObject;
        GameObject panel = Parent.transform.Find("Enemy1").gameObject;
        switch (idx)
        {
            case 0:
                panel = Parent.transform.Find("Enemy1").gameObject;
                break;
            case 1:
                panel = Parent.transform.Find("Enemy2").gameObject;
                break;
            case 2:
                panel = Parent.transform.Find("Enemy3").gameObject;
                break;
            case 3:
                panel = Parent.transform.Find("Enemy4").gameObject;
                break;
            case 4:
                panel = Parent.transform.Find("Enemy5").gameObject;
                break;
            case 5:
                panel = Parent.transform.Find("Enemy6").gameObject;
                break;
            default:
                panel = Parent.transform.Find("Enemy1").gameObject;
                break;
        }
        return panel;
    }
    IEnumerator CheckBattleEnded()
    {
        yield return new WaitForSeconds(0.1f);
        bool AllEnemyDead = true;
        for(int i = 0; i < enemyKills.Count; i++)
        {
            AllEnemyDead &= (bool)enemyKills[i];
            //bMoveToNextEnemy = true;
        }
        if (AllEnemyDead)
        {
            bBattleEnded = true;
            int cont = 1;
            foreach (ItemCodes item in itemEarn)
            {
                GameObject panel = ItemsPanel.transform.Find("ItemPanel" + cont).gameObject;
                GameObject icon = panel.transform.Find("Icon").gameObject;
                GameObject text = panel.transform.Find("Text").gameObject;

                icon.GetComponent<RawImage>().texture = GetItemIcon(item);
                text.GetComponent<Text>().text = GetItemName(item);
            }
            transform.Find("BattleCover").GetComponent<Canvas>().gameObject.transform.Find("Text").GetComponent<Text>().text = "Battle Resolved!";
            transform.Find("BattleCover").GetComponent<Canvas>().gameObject.SetActive(true);
            StartCoroutine(CloseAfter(5f));
        }

    }

    private string GetItemName(ItemCodes item)
    {
        throw new System.NotImplementedException();
    }

    private Texture GetItemIcon(ItemCodes item)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator AllowAttackAfter(float time)
    {
        yield return new WaitForSeconds(time);
        bAllowAttack = true;
    }
    IEnumerator AllowMoveAfter(float time)
    {
        yield return new WaitForSeconds(time);
        bAllowMove = true;
    }
    IEnumerator PopulateEnemies()
    {
        int NumEnemies = Random.Range(1, 7);
        enemyKills = new List<bool>();
        EnemyTimeAtack = new List<float>();
        for (int i = 0; i < NumEnemies; i++)
        {
            enemyKills.Add(false);
            EnemyTimeAtack.Add(0f);
        }
        if (NumEnemies == 1)
        {
            for (int i = 0; i < NumEnemies; i++)
            {
                GameObject enemyModel = getEnemy(EnemySize.Boss, i);                
                enemies.Add(enemyModel);
                enemyPanels.Add(getEnemyPanel(i));
                ((GameObject)enemyPanels[i]).SetActive(true);
                var ec = enemyModel.GetComponent<EnemyController>();
                getEnemyPanel(i).transform.Find("Icon").GetComponent<RawImage>().texture = ec.Icon;
                getEnemyPanel(i).transform.Find("Name").GetComponent<Text>().text = ec.MonsterName;
            }
        }
        else if(NumEnemies < 3)
        {
            for (int i = 0; i < NumEnemies; i++)
            {
                GameObject enemyModel = getEnemy(EnemySize.Medium, i);
                enemies.Add(enemyModel);
                enemyPanels.Add(getEnemyPanel(i));
                ((GameObject)enemyPanels[i]).SetActive(true);
                var ec = enemyModel.GetComponent<EnemyController>();
                getEnemyPanel(i).transform.Find("Icon").GetComponent<RawImage>().texture = ec.Icon;
                getEnemyPanel(i).transform.Find("Name").GetComponent<Text>().text = ec.MonsterName;
            }
        }
        else if (NumEnemies < 5)
        {
            for (int i = 0; i < NumEnemies; i++)
            {
                GameObject enemyModel = getEnemy(EnemySize.Small, i);
                enemies.Add(enemyModel);
                enemyPanels.Add(getEnemyPanel(i));
                ((GameObject)enemyPanels[i]).SetActive(true);
                var ec = enemyModel.GetComponent<EnemyController>();
                getEnemyPanel(i).transform.Find("Icon").GetComponent<RawImage>().texture = ec.Icon;
                getEnemyPanel(i).transform.Find("Name").GetComponent<Text>().text = ec.MonsterName;
            }
        }
        else
        {
            for (int i = 0; i < NumEnemies; i++)
            {
                GameObject enemyModel = getEnemy(EnemySize.Tiny, i);
                enemies.Add(enemyModel);
                enemyPanels.Add(getEnemyPanel(i));
                ((GameObject)enemyPanels[i]).SetActive(true);
                var ec = enemyModel.GetComponent<EnemyController>();
                getEnemyPanel(i).transform.Find("Icon").GetComponent<RawImage>().texture = ec.Icon;
                getEnemyPanel(i).transform.Find("Name").GetComponent<Text>().text = ec.MonsterName;
            }
        }
        if (NumEnemies == 1)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(0, 0, 0);
        }
        else if (NumEnemies == 2)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(4, 0, -2);
            ((GameObject)enemies[1]).transform.position = new Vector3(-4, 0, -2);
        }
        else if (NumEnemies == 3)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(0, 0, 0);
            ((GameObject)enemies[1]).transform.position = new Vector3(4, 0, -2);
            ((GameObject)enemies[2]).transform.position = new Vector3(-4, 0, -2);
        }
        else if (NumEnemies == 4)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(4, 0, -2);
            ((GameObject)enemies[1]).transform.position = new Vector3(-4, 0, -2);
            ((GameObject)enemies[2]).transform.position = new Vector3(4, 0, 1.5f);
            ((GameObject)enemies[3]).transform.position = new Vector3(-4, 0, 1.5f);
        }
        else if (NumEnemies == 5)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(4, 0, -2);
            ((GameObject)enemies[1]).transform.position = new Vector3(-4, 0, -2);
            ((GameObject)enemies[2]).transform.position = new Vector3(4, 0, 1.5f);
            ((GameObject)enemies[3]).transform.position = new Vector3(-4, 0, 1.5f);
            ((GameObject)enemies[4]).transform.position = new Vector3(0, 0, 4f);

        }
        else if (NumEnemies == 6)
        {
            ((GameObject)enemies[0]).transform.position = new Vector3(4, 0, -2);
            ((GameObject)enemies[1]).transform.position = new Vector3(-4, 0, -2);
            ((GameObject)enemies[2]).transform.position = new Vector3(4, 0, 1.5f);
            ((GameObject)enemies[3]).transform.position = new Vector3(-4, 0, 1.5f);
            ((GameObject)enemies[4]).transform.position = new Vector3(0, 0, 4f);
            ((GameObject)enemies[5]).transform.position = new Vector3(0, 0, 0f);
        }
        yield return new WaitForSeconds(0.1f);
    }
    GameObject getEnemy(EnemySize MaxSize, int Idx)
    {

        EnemySize size = MaxSize == EnemySize.Tiny? EnemySize.Tiny: (EnemySize)Random.Range(0, ((int)MaxSize) + 1);
        GameObject enemyModel;
        EnemyController ec;
        switch (size)
        {
            case EnemySize.Tiny:
                enemyModel = GetEnemyModel("EnemyTiny", PlayerData.CurrentRoom.type);
                break;
            case EnemySize.Small:
                enemyModel = GetEnemyModel("EnemySmall", PlayerData.CurrentRoom.type);
                break;
            case EnemySize.Medium:
                enemyModel = GetEnemyModel("EnemyMedium", PlayerData.CurrentRoom.type);
                break;
            case EnemySize.Large:
                enemyModel = GetEnemyModel("EnemyLarge", PlayerData.CurrentRoom.type);
                break;
            case EnemySize.Boss:
                enemyModel = GetEnemyModel("EnemyBoss", PlayerData.CurrentRoom.type);
                break;
            default:
                enemyModel = GetEnemyModel("EnemySmall", PlayerData.CurrentRoom.type);
                break;
        }
        ec = enemyModel.GetComponent<EnemyController>();
        ec.Health = GetHealthByLevel(PlayerData.Level, PlayerData.CurrentRoom.type, size);        
        ec.Experience = GetExperience(PlayerData.Level, PlayerData.CurrentRoom.type, size);
        ec.AttSpeed = GetSpeedByLevel(PlayerData.Level, PlayerData.CurrentRoom.type, size);
        ec.ItemHeld = GetItemByLevel(PlayerData.Level, PlayerData.CurrentRoom.type, size);
        ec.AttackPow1 = GetPow1Bylevel(PlayerData.Level, PlayerData.CurrentRoom.type, size, ec.AttSpeed, out AttackType type1, out float Acc1);
        ec.AttackPow2 = GetPow2Bylevel(PlayerData.Level, PlayerData.CurrentRoom.type, size, ec.AttSpeed, out AttackType type2, out float Acc2);
        ec.Special = GetSpecialBylevel(PlayerData.Level, PlayerData.CurrentRoom.type, size, ec.AttSpeed, out AttackType typeS, out float AccS);
        ec.Icon = GetIconByLevel(PlayerData.CurrentRoom.type, size);

        string MonsterName = GetEnemyNameByLevel(PlayerData.Level, PlayerData.CurrentRoom.type, size);
        
        if(enemyNames.ContainsKey(MonsterName))
        {
            int cant = (int)enemyNames[MonsterName];
            cant++;
            enemyNames[MonsterName] = cant;
            MonsterName += " " + cant.ToString();
        }
        else
        {
            enemyNames[MonsterName] = 1;
            MonsterName += " 1";
        }
        ec.MonsterName = MonsterName;

        ec.AttackType1 = type1;
        ec.AttackType2 = type2;
        ec.AttackTypeS = typeS;
        ec.AccAtt1 = Acc1;
        ec.AccAtt2 = Acc2;
        ec.AccAttS = AccS;
        ec.Damage = 0;
        return enemyModel;
    }

    private ItemCodes GetItemByLevel(float level, RoomType type, EnemySize size)
    {
        ItemCodes SelItem = ItemCodes.None;
        List<ItemCodes> itemCodes = new List<ItemCodes>();
        
        switch (type)
        {
            case RoomType.Initial:
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.Stone);
                itemCodes.Add(ItemCodes.Bone);
                itemCodes.Add(ItemCodes.TinChip);
                break;
            case RoomType.None:
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.None);
                break;
            case RoomType.Cave:
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.Stone);
                itemCodes.Add(ItemCodes.Flint);
                itemCodes.Add(ItemCodes.TinChip);
                break;
            case RoomType.Praire:
                itemCodes.Add(ItemCodes.None);
                itemCodes.Add(ItemCodes.Stone);
                itemCodes.Add(ItemCodes.Bone);
                itemCodes.Add(ItemCodes.WoodChip);                
                break;
            case RoomType.Mountains:
                itemCodes.Add(ItemCodes.Bone);
                itemCodes.Add(ItemCodes.Flint);
                itemCodes.Add(ItemCodes.IronChip);
                itemCodes.Add(ItemCodes.CopperChip);
                break;
            case RoomType.Sky:
                itemCodes.Add(ItemCodes.Bone);
                itemCodes.Add(ItemCodes.CopperChip);
                itemCodes.Add(ItemCodes.SteelChip);
                itemCodes.Add(ItemCodes.EmeraldChip);
                break;
            case RoomType.Beach:
                itemCodes.Add(ItemCodes.Flint);
                itemCodes.Add(ItemCodes.Shell);
                itemCodes.Add(ItemCodes.SteelChip);
                itemCodes.Add(ItemCodes.SilverChip);
                break;
            case RoomType.Hellish:
                itemCodes.Add(ItemCodes.Bone);
                itemCodes.Add(ItemCodes.Shell);                
                itemCodes.Add(ItemCodes.SilverChip);
                itemCodes.Add(ItemCodes.HellChip);
                break;
            case RoomType.Lava:
                itemCodes.Add(ItemCodes.Stone);
                itemCodes.Add(ItemCodes.Flint);
                itemCodes.Add(ItemCodes.HellChip);
                itemCodes.Add(ItemCodes.GoldChip);
                break;
            case RoomType.Underwater:
                itemCodes.Add(ItemCodes.WoodChip);
                itemCodes.Add(ItemCodes.Shell);
                itemCodes.Add(ItemCodes.LapisChip);
                itemCodes.Add(ItemCodes.SaphireChip);
                break;
            case RoomType.Void:
                itemCodes.Add(ItemCodes.SilverChip);
                itemCodes.Add(ItemCodes.GoldChip);
                itemCodes.Add(ItemCodes.OricalcumChip);
                itemCodes.Add(ItemCodes.MithrilChip);
                break;
            case RoomType.Light:
                itemCodes.Add(ItemCodes.MithrilChip);
                itemCodes.Add(ItemCodes.SkyChip);
                itemCodes.Add(ItemCodes.DarkChip);
                itemCodes.Add(ItemCodes.OmniChip);
                break;
        }
        switch (size)
        {
            case EnemySize.Tiny:
                {
                    float chance = Random.Range(0f, 1f);
                    float quality = Random.Range(0f, 1f);
                    ItemQuality item_quality = ItemQuality.Normal;
                    if (quality < 0.01) item_quality = ItemQuality.Legendary;
                    else if (quality < 0.05) item_quality = ItemQuality.Special;
                    else if (quality < 0.1) item_quality = ItemQuality.Rare;
                    else if (quality < 0.25) item_quality = ItemQuality.Uncommon;
                    else item_quality = ItemQuality.Normal;
                    if (chance < 0.01)
                    {
                        SelItem = GetByQuality(itemCodes[3], item_quality);
                    }
                    else if (chance < 0.03)
                    {
                        SelItem = GetByQuality(itemCodes[2], item_quality);
                    }
                    else if (chance < 0.08)
                    {
                        SelItem = GetByQuality(itemCodes[1], item_quality);
                    }
                    else if (chance < 0.16)
                    {
                        SelItem = GetByQuality(itemCodes[0], item_quality);
                    }
                    else if (chance < 0.35)
                    {
                        SelItem = GetByQuality(ItemCodes.Junk, item_quality);
                    }
                    else SelItem = ItemCodes.None;
                }
                break;
            case EnemySize.Small:
                {
                    float chance = Random.Range(0f, 1f);
                    float quality = Random.Range(0f, 1f);
                    ItemQuality item_quality = ItemQuality.Normal;
                    if (quality < 0.01) item_quality = ItemQuality.Legendary;
                    else if (quality < 0.05) item_quality = ItemQuality.Special;
                    else if (quality < 0.1) item_quality = ItemQuality.Rare;
                    else if (quality < 0.25) item_quality = ItemQuality.Uncommon;
                    else item_quality = ItemQuality.Normal;
                    if (chance < 0.02)
                    {
                        SelItem = GetByQuality(itemCodes[3], item_quality);
                    }
                    else if (chance < 0.06)
                    {
                        SelItem = GetByQuality(itemCodes[2], item_quality);
                    }
                    else if (chance < 0.125)
                    {
                        SelItem = GetByQuality(itemCodes[1], item_quality);
                    }
                    else if (chance < 0.25)
                    {
                        SelItem = GetByQuality(itemCodes[0], item_quality);
                    }
                    else if (chance < 0.40)
                    {
                        SelItem = GetByQuality(ItemCodes.Junk, item_quality);
                    }
                    else SelItem = ItemCodes.None;
                }
                break;
            case EnemySize.Medium:
                {
                    float chance = Random.Range(0f, 1f);
                    float quality = Random.Range(0f, 1f);
                    ItemQuality item_quality = ItemQuality.Normal;
                    if (quality < 0.01) item_quality = ItemQuality.Legendary;
                    else if (quality < 0.05) item_quality = ItemQuality.Special;
                    else if (quality < 0.1) item_quality = ItemQuality.Rare;
                    else if (quality < 0.25) item_quality = ItemQuality.Uncommon;
                    else item_quality = ItemQuality.Normal;
                    if (chance < 0.03)
                    {
                        SelItem = GetByQuality(itemCodes[3], item_quality);
                    }
                    else if (chance < 0.08)
                    {
                        SelItem = GetByQuality(itemCodes[2], item_quality);
                    }
                    else if (chance < 0.16)
                    {
                        SelItem = GetByQuality(itemCodes[1], item_quality);
                    }
                    else if (chance < 0.30)
                    {
                        SelItem = GetByQuality(itemCodes[0], item_quality);
                    }
                    else if (chance < 0.55)
                    {
                        SelItem = GetByQuality(ItemCodes.Junk, item_quality);
                    }
                    else SelItem = ItemCodes.None;
                }
                break;
            case EnemySize.Large:
                {
                    float chance = Random.Range(0f, 1f);
                    float quality = Random.Range(0f, 1f);
                    ItemQuality item_quality = ItemQuality.Normal;
                    if (quality < 0.01) item_quality = ItemQuality.Legendary;
                    else if (quality < 0.05) item_quality = ItemQuality.Special;
                    else if (quality < 0.1) item_quality = ItemQuality.Rare;
                    else if (quality < 0.25) item_quality = ItemQuality.Uncommon;
                    else item_quality = ItemQuality.Normal;
                    if (chance < 0.045)
                    {
                        SelItem = GetByQuality(itemCodes[3], item_quality);
                    }
                    else if (chance < 0.10)
                    {
                        SelItem = GetByQuality(itemCodes[2], item_quality);
                    }
                    else if (chance < 0.20)
                    {
                        SelItem = GetByQuality(itemCodes[1], item_quality);
                    }
                    else if (chance < 0.40)
                    {
                        SelItem = GetByQuality(itemCodes[0], item_quality);
                    }
                    else if (chance < 0.60)
                    {
                        SelItem = GetByQuality(ItemCodes.Junk, item_quality);
                    }
                    else SelItem = ItemCodes.None;
                }
                break;
            case EnemySize.Boss:
                {
                    float chance = Random.Range(0f, 1f);
                    float quality = Random.Range(0f, 1f);
                    ItemQuality item_quality = ItemQuality.Normal;
                    if (quality < 0.01) item_quality = ItemQuality.Legendary;
                    else if (quality < 0.05) item_quality = ItemQuality.Special;
                    else if (quality < 0.1) item_quality = ItemQuality.Rare;
                    else if (quality < 0.25) item_quality = ItemQuality.Uncommon;
                    else item_quality = ItemQuality.Normal;
                    if (chance < 0.06)
                    {
                        SelItem = GetByQuality(itemCodes[3], item_quality);
                    }
                    else if (chance < 0.125)
                    {
                        SelItem = GetByQuality(itemCodes[2], item_quality);
                    }
                    else if (chance < 0.25)
                    {
                        SelItem = GetByQuality(itemCodes[1], item_quality);
                    }
                    else if (chance < 0.50)
                    {
                        SelItem = GetByQuality(itemCodes[0], item_quality);
                    }
                    else if (chance < 0.70)
                    {
                        SelItem = GetByQuality(ItemCodes.Junk, item_quality);
                    }
                    else SelItem = ItemCodes.None;
                }
                break;
        }
        return SelItem;
    }
    public ItemCodes GetByQuality(ItemCodes code, ItemQuality quality)
    {
        ItemCodes item = code;
        switch (code)
        {
            case ItemCodes.None:
                item = ItemCodes.None;
                break;
            case ItemCodes.Stone:
                item = quality > ItemQuality.Special? ItemCodes.Flint: ItemCodes.Stone;
                break;
            case ItemCodes.Bone:
                item = quality > ItemQuality.Special ? ItemCodes.Shell : ItemCodes.Bone;
                break;
            default:
                item = quality > ItemQuality.Special ? (ItemCodes)(((int)code) + 20) : code;
                break;
        }
        return item;
    }
    private float GetSpeedByLevel(float level, RoomType type, EnemySize size)
    {
        float BaseSpeed = 5, MultiplySpeed = 1;
        switch (size)
        {
            case EnemySize.Tiny:
                MultiplySpeed = 0.5f;
                break;
            case EnemySize.Small:
                MultiplySpeed = 0.4f;
                break;
            case EnemySize.Medium:
                MultiplySpeed = 0.3f;
                break;
            case EnemySize.Large:
                MultiplySpeed = 0.2f;
                break;
            case EnemySize.Boss:
                MultiplySpeed = 0.1f;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                BaseSpeed = 3;
                break;
            case RoomType.None:
                BaseSpeed = 1;
                break;
            case RoomType.Cave:
                BaseSpeed = 3.25f;
                break;
            case RoomType.Praire:
                BaseSpeed = 3.25f;
                break;
            case RoomType.Mountains:
                BaseSpeed = 3.55f;
                break;
            case RoomType.Sky:
                BaseSpeed = 4f;
                break;
            case RoomType.Beach:
                BaseSpeed = 3.25f;
                break;
            case RoomType.Hellish:
                BaseSpeed = 3.8f;
                break;
            case RoomType.Lava:
                BaseSpeed = 4f;
                break;
            case RoomType.Underwater:
                BaseSpeed = 3.25f;
                break;
            case RoomType.Void:
                BaseSpeed = 4.2f;
                break;
            case RoomType.Light:
                BaseSpeed = 4.8f;
                break;
        }

        float Speed = BaseSpeed + (BaseSpeed * MultiplySpeed) + (level / 30f);

        if (Speed > 9) Speed = 9;
        return Speed;
    }

    private Texture GetIconByLevel(RoomType type, EnemySize size)
    {
        string Folder = "default", Filename = "default";
        switch (size)
        {
            case EnemySize.Tiny:
                Filename = "tiny";
                break;
            case EnemySize.Small:
                Filename = "small";
                break;
            case EnemySize.Medium:
                Filename = "medium";
                break;
            case EnemySize.Large:
                Filename = "large";
                break;
            case EnemySize.Boss:
                Filename = "boss";
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                Folder = "initial";
                break;
            case RoomType.None:
                Folder = "default";
                break;
            case RoomType.Cave:
                Folder = "cave";
                break;
            case RoomType.Praire:
                Folder = "praire";
                break;
            case RoomType.Mountains:
                Folder = "mountain";
                break;
            case RoomType.Sky:
                Folder = "sky";
                break;
            case RoomType.Beach:
                Folder = "beach";
                break;
            case RoomType.Hellish:
                Folder = "hell";
                break;
            case RoomType.Lava:
                Folder = "lava";
                break;
            case RoomType.Underwater:
                Folder = "water";
                break;
            case RoomType.Void:
                Folder = "void";
                break;
            case RoomType.Light:
                Folder = "light";
                break;
        }

        return LoadPNG(@".\Assets\Sprites\" + Folder +"\\" + Filename + ".png");
    }

    private float GetPow1Bylevel(float level, RoomType type, EnemySize size, float Speed, out AttackType type1, out float Accuracy)
    {
        float multiplier = 1, baseDmg = 4;
        Accuracy = 0.8f;
        switch (size)
        {
            case EnemySize.Tiny:
                multiplier = 0.5f;
                break;
            case EnemySize.Small:
                multiplier = 1f;
                break;
            case EnemySize.Medium:
                multiplier = 1.5f;
                break;
            case EnemySize.Large:
                multiplier = 2f;
                break;
            case EnemySize.Boss:
                multiplier = 4f;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                baseDmg = 1;
                Accuracy = 0.9f;
                break;
            case RoomType.None:
                baseDmg = 0;
                Accuracy = 0.5f;
                break;
            case RoomType.Cave:
                baseDmg = 2;
                Accuracy = 0.9f;
                break;
            case RoomType.Praire:
                baseDmg = 3;
                Accuracy = 0.92f;
                break;
            case RoomType.Mountains:
                baseDmg = 3;
                Accuracy = 0.91f;
                break;
            case RoomType.Sky:
                baseDmg = 4;
                Accuracy = 0.88f;
                break;
            case RoomType.Beach:
                baseDmg = 6;
                Accuracy = 0.9f;
                break;
            case RoomType.Hellish:
                baseDmg = 12;
                Accuracy = 0.95f;
                break;
            case RoomType.Lava:
                baseDmg = 15;
                Accuracy = 0.85f;
                break;
            case RoomType.Underwater:
                baseDmg = 12;
                Accuracy = 0.7f;
                break;
            case RoomType.Void:
                baseDmg = 20;
                Accuracy = 0.97f;
                break;
            case RoomType.Light:
                baseDmg = 35;
                Accuracy = 0.95f;
                break;
        }
        float dmg = (level * 0.5f * baseDmg * multiplier);
        type1 = AttackType.Mele;
        switch (type1)
        {
            case AttackType.Mele:
                Accuracy *= (Speed / 10);
                break;
            case AttackType.Ranged:
                Accuracy *= (Speed / 5);
                break;
            case AttackType.Magic:
                Accuracy *= (Speed / 10);
                break;
        }
        if (Accuracy > 0.99f) Accuracy = 0.99f;
        return Mathf.RoundToInt(dmg);
    }
    private float GetPow2Bylevel(float level, RoomType type, EnemySize size, float Speed, out AttackType type1, out float Accuracy)
    {
        float multiplier = 1, baseDmg = 4;
        bool bSizeRanged = false, bTypeRanged = false;
        bool bIsSizeMagic = false, bIsTypeMagic = false;
        Accuracy = 0.8f;
        switch (size)
        {
            case EnemySize.Tiny:
                multiplier = 0.5f;
                bSizeRanged = true;
                bIsSizeMagic = true;
                break;
            case EnemySize.Small:
                multiplier = 1f;
                bSizeRanged = false;
                break;
            case EnemySize.Medium:
                multiplier = 1.5f;
                bSizeRanged = true;
                break;
            case EnemySize.Large:
                multiplier = 2f;
                bSizeRanged = false;
                bIsSizeMagic = true;
                break;
            case EnemySize.Boss:
                multiplier = 4f;
                bSizeRanged = true;
                bIsSizeMagic = true;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                baseDmg = 2;
                bTypeRanged = true;
                bIsTypeMagic = false;
                Accuracy = 0.9f;
                break;
            case RoomType.None:
                baseDmg = 0;
                bTypeRanged = false;
                bIsTypeMagic = false;
                Accuracy = 0.1f;
                break;
            case RoomType.Cave:
                baseDmg = 4;
                bTypeRanged = false;
                Accuracy = 0.9f;
                break;
            case RoomType.Praire:
                baseDmg = 6;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.92f;
                break;
            case RoomType.Mountains:
                baseDmg = 6;
                bTypeRanged = false;
                Accuracy = 0.91f;
                break;
            case RoomType.Sky:
                baseDmg = 8;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.88f;
                break;
            case RoomType.Beach:
                baseDmg = 12;
                bTypeRanged = true;
                Accuracy = 0.9f;
                break;
            case RoomType.Hellish:
                baseDmg = 20;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.95f;
                break;
            case RoomType.Lava:
                baseDmg = 18;
                bTypeRanged = false;
                Accuracy = 0.85f;
                break;
            case RoomType.Underwater:
                baseDmg = 24;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.7f;
                break;
            case RoomType.Void:
                baseDmg = 30;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.97f;
                break;
            case RoomType.Light:
                baseDmg = 50;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.95f;
                break;
        }
        float dmg = (level * 0.5f * baseDmg * multiplier);
        if(bSizeRanged && bTypeRanged)
        {
            type1 = bIsSizeMagic && bIsTypeMagic ? AttackType.Magic : AttackType.Ranged;
        }
        else
        {
            type1 = AttackType.Mele;
        }
        switch (type1)
        {
            case AttackType.Mele:
                Accuracy *= (Speed / 10);
                break;
            case AttackType.Ranged:
                Accuracy *= (Speed / 5);
                break;
            case AttackType.Magic:
                Accuracy *= (Speed / 10);
                break;
        }
        if (Accuracy > 0.99f) Accuracy = 0.99f;
        return Mathf.RoundToInt(dmg);
    }
    private float GetSpecialBylevel(float level, RoomType type, EnemySize size, float Speed, out AttackType type1, out float Accuracy)
    {
        float multiplier = 1, baseDmg = 4;
        bool bSizeRanged = false, bTypeRanged = false;
        bool bIsSizeMagic = false, bIsTypeMagic = false;
        Accuracy = 0.8f;
        switch (size)
        {
            case EnemySize.Tiny:
                multiplier = 1f;
                bSizeRanged = true;
                bIsSizeMagic = true;
                break;
            case EnemySize.Small:
                multiplier = 2f;
                bSizeRanged = false;
                break;
            case EnemySize.Medium:
                multiplier = 3f;
                bSizeRanged = true;
                break;
            case EnemySize.Large:
                multiplier = 4f;
                bSizeRanged = false;
                bIsSizeMagic = true;
                break;
            case EnemySize.Boss:
                multiplier = 6f;
                bSizeRanged = true;
                bIsSizeMagic = true;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                baseDmg = 2;
                bTypeRanged = true;
                bIsTypeMagic = false;
                Accuracy = 0.6f;
                break;
            case RoomType.None:
                baseDmg = 0;
                bTypeRanged = false;
                bIsTypeMagic = false;
                Accuracy = 0.5f;
                break;
            case RoomType.Cave:
                baseDmg = 5;
                bTypeRanged = false;
                Accuracy = 0.6f;
                break;
            case RoomType.Praire:
                baseDmg = 8;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.62f;
                break;
            case RoomType.Mountains:
                baseDmg = 8;
                bTypeRanged = false;
                Accuracy = 0.61f;
                break;
            case RoomType.Sky:
                baseDmg = 10;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.58f;
                break;
            case RoomType.Beach:
                baseDmg = 15;
                bTypeRanged = true;
                Accuracy = 0.6f;
                break;
            case RoomType.Hellish:
                baseDmg = 22;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.65f;
                break;
            case RoomType.Lava:
                baseDmg = 22;
                bTypeRanged = false;
                Accuracy = 0.55f;
                break;
            case RoomType.Underwater:
                baseDmg = 28;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.5f;
                break;
            case RoomType.Void:
                baseDmg = 35;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.67f;
                break;
            case RoomType.Light:
                baseDmg = 60;
                bTypeRanged = true;
                bIsTypeMagic = true;
                Accuracy = 0.7f;
                break;
        }
        float dmg = (level * 0.5f * baseDmg * multiplier);
        if (bSizeRanged && bTypeRanged)
        {
            type1 = bIsSizeMagic && bIsTypeMagic ? AttackType.Magic : AttackType.Ranged;
        }
        else
        {
            type1 = AttackType.Mele;
        }
        switch (type1)
        {
            case AttackType.Mele:
                Accuracy *= (Speed / 10);
                break;
            case AttackType.Ranged:
                Accuracy *= (Speed / 5);
                break;
            case AttackType.Magic:
                Accuracy *= (Speed / 10);
                break;
        }
        if (Accuracy > 0.99f) Accuracy = 0.99f;
        return Mathf.RoundToInt(dmg);
    }

    private int GetExperience(float level, RoomType type, EnemySize size)
    {
        float multiplier = 1, baseDmg = 4;
        switch (size)
        {
            case EnemySize.Tiny:
                multiplier = 0.5f;
                break;
            case EnemySize.Small:
                multiplier = 1f;
                break;
            case EnemySize.Medium:
                multiplier = 1.5f;
                break;
            case EnemySize.Large:
                multiplier = 2f;
                break;
            case EnemySize.Boss:
                multiplier = 4f;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                baseDmg = 4;
                break;
            case RoomType.None:
                baseDmg = 2;
                break;
            case RoomType.Cave:
                baseDmg = 8;
                break;
            case RoomType.Praire:
                baseDmg = 8;
                break;
            case RoomType.Mountains:
                baseDmg = 12;
                break;
            case RoomType.Sky:
                baseDmg = 18;
                break;
            case RoomType.Beach:
                baseDmg = 22;
                break;
            case RoomType.Hellish:
                baseDmg = 30;
                break;
            case RoomType.Lava:
                baseDmg = 35;
                break;
            case RoomType.Underwater:
                baseDmg = 28;
                break;
            case RoomType.Void:
                baseDmg = 50;
                break;
            case RoomType.Light:
                baseDmg = 75;
                break;
        }
        float exp = (level * 0.5f * baseDmg * multiplier);

        return Mathf.RoundToInt(exp);
    }

    private int GetHealthByLevel(float level, RoomInstance.RoomType type, EnemySize size)
    {
        float multiplier = 1, basehealth = 40;

        switch (size)
        {
            case EnemySize.Tiny:
                multiplier = 0.5f;
                break;
            case EnemySize.Small:
                multiplier = 1f;
                break;
            case EnemySize.Medium:
                multiplier = 1.5f;
                break;
            case EnemySize.Large:
                multiplier = 2f;
                break;
            case EnemySize.Boss:
                multiplier = 4f;
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                basehealth = 40;
                break;
            case RoomType.None:
                basehealth = 20;
                break;
            case RoomType.Cave:
                basehealth = 80;
                break;
            case RoomType.Praire:
                basehealth = 80;
                break;
            case RoomType.Mountains:
                basehealth = 120;
                break;
            case RoomType.Sky:
                basehealth = 180;
                break;
            case RoomType.Beach:
                basehealth = 220;
                break;
            case RoomType.Hellish:
                basehealth = 300;
                break;
            case RoomType.Lava:
                basehealth = 350;
                break;
            case RoomType.Underwater:
                basehealth = 280;
                break;
            case RoomType.Void:
                basehealth = 500;
                break;
            case RoomType.Light:
                basehealth = 750;
                break;

        }

        float health = (level * 0.5f * basehealth * multiplier);

        return Mathf.RoundToInt(health);
    }
    private string GetEnemyNameByLevel(float level, RoomInstance.RoomType type, EnemySize size)
    {
        string FinalName = "";
        string Sufix = "Bug", Prefix = "Rock";
        switch (size)
        {
            case EnemySize.Tiny:
                Sufix = "Bug";
                break;
            case EnemySize.Small:
                Sufix = "Critter";
                break;
            case EnemySize.Medium:
                Sufix = "Beast";
                break;
            case EnemySize.Large:
                Sufix = "Spawn";
                break;
            case EnemySize.Boss:
                Sufix = "Alpha";
                break;
        }
        switch (type)
        {
            case RoomType.Initial:
                Prefix = "Rock ";
                break;
            case RoomType.None:
                Prefix = "";
                break;
            case RoomType.Cave:
                Prefix = "Cave ";
                break;
            case RoomType.Praire:
                Prefix = "Wind ";
                break;
            case RoomType.Mountains:
                Prefix = "Mountain ";
                break;
            case RoomType.Sky:
                Prefix = "Flying ";
                break;
            case RoomType.Beach:
                Prefix = "Coast ";
                break;
            case RoomType.Hellish:
                Prefix = "Hell ";
                break;
            case RoomType.Lava:
                Prefix = "Fire ";
                break;
            case RoomType.Underwater:
                Prefix = "Water ";
                break;
            case RoomType.Void:
                Prefix = "Void ";
                break;
            case RoomType.Light:
                Prefix = "Shining ";
                break;

        }

        FinalName = Prefix + Sufix;

        return FinalName;
    }
    private GameObject GetEnemyModel(string filename, RoomInstance.RoomType type)
    {
        var loadedObject = Resources.Load(@"Battle\Enemies\" + filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        var instance = (GameObject)Instantiate(loadedObject, Vector3.zero, Quaternion.identity);

        return instance;
    }
    private GameObject GetPlayerModel(string filename)
    {
        var loadedObject = Resources.Load(@"PlayerModels\" + filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        var instance = (GameObject)Instantiate(loadedObject, Vector3.zero, Quaternion.identity);

        return instance;
    }
    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
    IEnumerator CloseAfter(float time)
    {
        yield return new WaitForSeconds(time);

        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(PlayerData.parentScene);

    }
    private enum EnemySize
    {
        Tiny, Small, Medium, Large, Boss
    }
}
