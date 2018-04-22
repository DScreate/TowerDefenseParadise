using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerType", menuName = "New Tower")]
public class Tower : ScriptableObject {

    // Tower Delegates
    public delegate bool Attack();

    // Build Properties
    public int buildCost;
    public int upgradeCost;
    public int maxUpgrade;
    public int currentLevel = 1;

    // Attack Properities
    public int health; // ?
    public int damage;
    public float attackSpeed;
    public float range;
    public float bulletVelocity;
    public GameObject bulletPrefab;

    // Tower Personality
    public float temper;
    public float jealousy;
    [EnumFlags]
    public Personality personality; // ?

    // Tower Dialogs
    public string[,] dialogTexts;
    public AudioClip[,] dialogSounds;

}
