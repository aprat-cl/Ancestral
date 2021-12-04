using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Health, Mana, Damage;
    public float AttackPow1, AttackPow2, Special, AttSpeed;
    public AttackType AttackType1, AttackType2, AttackTypeS;
    public float AccAtt1, AccAtt2, AccAttS;
    public int Experience;
    public float rotationSpeed, Accuracy;
    public string MonsterName;
    public ItemCodes ItemHeld = ItemCodes.None;
    public Texture Icon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
    public enum AttackType
    {
        Mele, Ranged, Magic
    }
}
