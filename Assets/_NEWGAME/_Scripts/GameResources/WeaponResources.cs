using System;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts;
using _GAME._Scripts._CharacterController._Shooter;
using _GAME._Scripts._ItemManager;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using _NEWGAME._Scripts.Utils;
using Assets._SDK.Game;
using MyBox;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

public class WeaponResources : AbstractGameResources
{
    private const string WEAPON_SETTINGS_FOLDER = "Assets/_NEWGAME/_Settings/Weapon/Data";
    private const string WEAPON_PREFABS_DIR = "Assets/_NEWGAME/Prefabs/Weapons";

    // private const string WEAPON_IMPACT_VFX_FOLDER = "Assets/_NEWGAME/_Settings/Weapon/ImpactVFX";

    [OdinSerialize, HideInInspector]
    private Dictionary<int, WeaponSettings> AllWeaponSettings;

    public const int DEFAULT_WEAPON_INDEX = 0;

    // Owned each default weapon by default
    private void Awake()
    {
        foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
        {
            GetDefaultWeapon(type).Bought();
            if (WeaponSettingsByType[type].None(weapon => weapon.Entity.IsSelected))
                GetDefaultWeapon(type).Selected();
        }
    }

    [OdinSerialize]
    public Dictionary<WeaponType, List<WeaponSettings>> WeaponSettingsByType { get; private set; }

    public const int RifleAmmoCount = 40;
    public Dictionary<WeaponType, AmmoData> WeaponTypeToAmmoData = new()
    {
        [WeaponType.AssaultRifle] = new("Assault Rifle", RifleAmmoCount, 10000),
        [WeaponType.Handgun] = new("Pistol", RifleAmmoCount * .75f, 5000),
        [WeaponType.Shotgun] = new("Shotgun", RifleAmmoCount * .25f, 10000),
        [WeaponType.SMG] = new("SMG", RifleAmmoCount, 15000),
        [WeaponType.RPG] = new("Rocket", RifleAmmoCount * .25f, 20000),
    };

    public Weapon GetWeapon(int id) => AllWeaponSettings[id].Entity;
    public WeaponSettings GetWeaponSettings(int id) => AllWeaponSettings[id];

    public List<Weapon> UnownedWeapons => AllWeaponSettings.Values.Select(ws => ws.Entity).Where(w => !w.IsOwned && w.weaponType != WeaponType.Skin).ToList();

    public ItemListData ItemListData;


#if UNITY_EDITOR
    public SerializedObject serializedObject;

    public SerializedProperty itemReferenceList;
#endif

    public Weapon GetDefaultWeapon(WeaponType type)
    {
        return WeaponSettingsByType[type][DEFAULT_WEAPON_INDEX].Entity;
    }

#if UNITY_EDITOR
    private void LoadWeapons()
    {
        LoadSettingsToDictionary();

        EditorUtility.SetDirty(this);
    }

    private void LoadSettingsToDictionary()
    {
        AllWeaponSettings ??= new Dictionary<int, WeaponSettings>();
        AllWeaponSettings = LoadEntitySettings<WeaponSettings, Weapon>(WEAPON_SETTINGS_FOLDER /*"Assets/_NEWGAME/_Scripts/_SHAME/CharacterExtensions/_ResourcesForExtension/WeaponsNew/_weaponsPrefabs/Test/2"*/);

        WeaponSettingsByType = new Dictionary<WeaponType, List<WeaponSettings>>();

        foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
            WeaponSettingsByType.Add(type, new List<WeaponSettings>());

        foreach (var weaponSetting in AllWeaponSettings.Values)
            WeaponSettingsByType[weaponSetting.Entity.weaponType].Add(weaponSetting);
    }

    private void LoadItemListData()
    {
        List<Item> items = new();
        List<ShooterWeapon> weapons = LoadPrefab<ShooterWeapon>(WEAPON_PREFABS_DIR /*"Assets/_NEWGAME/_Scripts/_SHAME/CharacterExtensions/_ResourcesForExtension/WeaponsNew/_weaponsPrefabs/Test/1"*/);

        //int i = 0;

        foreach (ShooterWeapon weapon in weapons)
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            item.name = weapon.name;
            int id = (nameof(Weapon) + weapon.name).GetHashCode();
            item.icon = GetWeapon(id).ImageFront;
            item.stackable = false;
            item.originalObject = weapon.gameObject;
            item.isEquiped = false;
            item.isInEquipArea = false;
            item.type = ItemType.ShooterWeapon;
            item.description = string.Empty;
            item.canBeDestroyed = false;
            item.canBeDroped = false;
            item.canBeUsed = false;
            item.destroyAfterUse = false;

            AddItem(item, id);
        }
    }

    [Button("Load StartItems", ButtonSizes.Medium)]
    private void LoadStartItemListData()
    {
        var serializedProperty = new SerializedObject(GameManager.Instance.ItemManager);
        itemReferenceList = serializedProperty.FindProperty("startItems");
        foreach (var item in ItemListData.items)
        {
            AddStartItem(item, serializedProperty);
        }
    }

    private void AddStartItem(Item item, SerializedObject serializedProperty)
    {
        itemReferenceList.arraySize++;
        itemReferenceList.GetArrayElementAtIndex(itemReferenceList.arraySize - 1).FindPropertyRelative("id").intValue = item.id;
        itemReferenceList.GetArrayElementAtIndex(itemReferenceList.arraySize - 1).FindPropertyRelative("amount").intValue = 1;
        EditorUtility.SetDirty(GameManager.Instance.ItemManager);
        serializedProperty.ApplyModifiedProperties();
    }

    private void AddItem(Item item, int id)
    {
        if (item.name.Contains("(Clone)"))
        {
            item.name = item.name.Replace("(Clone)", string.Empty);
        }

        if (item && !ItemListData.items.Find(it => it.name.ToClearUpper().Equals(item.name.ToClearUpper())))
        {
            AssetDatabase.AddObjectToAsset(item, AssetDatabase.GetAssetPath(ItemListData));
            item.hideFlags = HideFlags.HideInHierarchy;

            //if (ItemListData.items.Exists(it => it.id.Equals(item.id)))
            item.id = id; /*GetUniqueID();*/
            ItemListData.items.Add(item);
            OrderByID(ref ItemListData.items);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(ItemListData);
            AssetDatabase.SaveAssets();
        }
    }

    protected virtual void OrderByID(ref List<Item> items)
    {
        if (items != null && items.Count > 0)
        {
            items = items.OrderBy(i => i.id).ToList();
        }
    }

    [Button("Load Resources", ButtonSizes.Medium)]
    public void LoadResources()
    {
        Init();
        LoadWeapons();
        LoadItemListData();
        Debug.Log($"Finished. Retrieved {AllWeaponSettings.Values.Count} weapons, ");
    }

    public void Init()
    {
        serializedObject = new SerializedObject(ItemListData);
    }

    [Button("SERIALIZE FOR APK BUILD")]
    public void SerializeWeaponSettingsByTypeToList()
    {
        serializedWeaponType = new List<WeaponType>();
        serializedWeaponSettingsList = new List<WeaponSettingsListWrapper>();

        foreach (var wt in WeaponSettingsByType)
        {
            serializedWeaponType.Add(wt.Key);
            serializedWeaponSettingsList.Add(new WeaponSettingsListWrapper(wt.Value));
        }
    }
#endif

    public void LoadFromSerializedList()
    {
        WeaponSettingsByType = new Dictionary<WeaponType, List<WeaponSettings>>();

        for (int i = 0; i < serializedWeaponType.Count; i++)
        {
            WeaponSettingsByType.Add(serializedWeaponType[i], serializedWeaponSettingsList[i].list);
        }
    }

    [SerializeField] private List<WeaponType> serializedWeaponType;
    [SerializeField] private List<WeaponSettingsListWrapper> serializedWeaponSettingsList;
}

[System.Serializable]
public class WeaponSettingsListWrapper
{
    public List<WeaponSettings> list;

    public WeaponSettingsListWrapper(List<WeaponSettings> list)
    {
        this.list = list;
    }
}
[System.Serializable]
public class AmmoData
{
    public string Name;
    public int AmmoAmount;
    public int Cash;

    public AmmoData(string name, float ammoAmount, int cash)
    {
        Name = name;
        AmmoAmount = ammoAmount.RoundToInt();
        Cash = cash;
    }
}