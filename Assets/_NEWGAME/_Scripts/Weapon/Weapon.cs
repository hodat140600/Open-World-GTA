using System;
using _GAME._Scripts._CharacterController._Shooter;
using _SDK.Money;
using _SDK.Shop;
using Assets._SDK.Entities;
using Assets._SDK.Inventory.Interfaces;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _GAME._Scripts.Inventory
{
    public enum WeaponType
    {
        Handgun      = 0,
        SMG          = 1,
        AssaultRifle = 2,
        Shotgun      = 3,
        RPG          = 4,
        Skin         = 5, 
    }

    [Serializable, HideReferenceObjectPicker]
    public sealed class Weapon : AbstractEntity, IGameItem, IShopItem
    {
        private static string WEAPON_TEMPLATE_DIR = "Weapons/WeaponTemplates";
        private static string MODEL_HOLDER_DIR    = "renderer";
        private static string PREFAB_PATH         = "Assets/_NEWGAME/Prefabs/Weapons/";

        private string ActivatedWeaponKey => "ActivatedWeapon" + Id;

        private string OwnedWeaponKey => "OwnedWeapon" + Id;

        public override int Id => (nameof(Weapon) + Name).GetHashCode();

        [OnValueChanged(nameof(UpdateAnimatorId))]
        [SerializeField]
        [HideLabel]
        [TabGroup("TabGroup", "General")]
        [EnumToggleButtons]
        [GUIColor(0f / 255f, 183f / 255f, 255f / 255f)]
        public WeaponType weaponType;

        [field: SerializeField
                , TabGroup("TabGroup", "General")
                , HideLabel]
        public Price Price { get; private set; }

        [TabGroup("TabGroup", "Visual")]
        [HorizontalGroup("TabGroup/Visual/Inline")]
        [BoxGroup("TabGroup/Visual/Inline/Side (2D)")]
        [HideLabel]
        [PreviewField(150, ObjectFieldAlignment.Center)]
        public Sprite ImageSide;

        [BoxGroup("TabGroup/Visual/Inline/Front (2D)")]
        [HideLabel]
        [PreviewField(150, ObjectFieldAlignment.Center)]
        public Sprite ImageFront;

        [BoxGroup("TabGroup/Visual/Inline/Front Glow (2D)")]
        [HideLabel]
        [PreviewField(150, ObjectFieldAlignment.Center)]
        public Sprite ImageFrontYellowGlow;
        
        [field: SerializeField
                , BoxGroup("TabGroup/Visual/Inline/Model (3D)")
                , HideLabel
                , PreviewField(150, ObjectFieldAlignment.Center)]
        public GameObject Model { get; set; }

        [TabGroup("TabGroup", "Stats"), SuffixLabel("float", true)]
        public float FireRate, ReloadSeconds;

        [TabGroup("TabGroup", "Stats"), SuffixLabel("int", true)]
        public int Damage, ClipSize;

        [field: SerializeField]
        public Material Material { get; set; }
        [field: SerializeField]
        public string Description { get; set; }

#if UNITY_EDITOR
        [Button("RAPID-FIRE", ButtonSizes.Large)]
        [ShowIf("@this.Automatic == false", false)]
        [HorizontalGroup("TabGroup/Stats/Button", .2f)]
        private void AutomaticOn()
        {
            Automatic = true;
        }

        [Button("RAPID-FIRE", ButtonSizes.Large)]
        [ShowIf("@this.Automatic == true", false)]
        [HorizontalGroup("TabGroup/Stats/Button", .2f)]
        [GUIColor(0.27f, 1f, 0f, 1f)]
        private void AutomaticOff()
        {
            Automatic = false;
        }
#endif

        [HideInInspector]
        public bool Automatic;

        [TabGroup("TabGroup", "Animator ID")]
        public short MovesetId;

        [InfoBox("Used for UpperBody, Shoot, Reload, Equip Ids")]
        [TabGroup("TabGroup", "Animator ID"), ReadOnly]
        public short AnimatorId;

        public void UpdateAnimatorId()
        {
            MovesetId = 1;
            AnimatorId = weaponType switch
            {
                WeaponType.Handgun      => 1,
                WeaponType.SMG          => 2,
                WeaponType.AssaultRifle => 2,
                WeaponType.Shotgun      => 3,
                WeaponType.RPG          => 4,
                _                       => throw new ArgumentOutOfRangeException()
            };
        }

#if UNITY_EDITOR
        [TabGroup("TabGroup", "File")]
        [Button("SAVE SETTINGS", ButtonSizes.Large)]
        public void SaveSettings()
        {
            AssetDatabase.SaveAssets();
        }

        private GameObject gameObject;

        [TabGroup("TabGroup", "File")]
        [Button("EXPORT GAMEOBJECT", ButtonSizes.Large)]
        public void ExportGameObject()
        {
            //List<ShooterWeapon> weaponTemplates = new List<ShooterWeapon>();
            //weaponTemplates = Resources.LoadAll<ShooterWeapon>(WEAPON_TEMPLATE_DIR);
            var           strings        = WEAPON_TEMPLATE_DIR + "/" + weaponType.ToString() + "Template";
            var           temp           = Resources.Load(strings);
            var           weaponTemplate = Object.Instantiate(temp) as GameObject;
            ShooterWeapon weapon         = weaponTemplate.GetComponent<ShooterWeapon>();
            gameObject = weapon.gameObject;

            weapon.name = Name;
            //weapon.LeoId = Id;

            Transform modelHolder = weapon.transform.Find(MODEL_HOLDER_DIR);
            Object.Instantiate(Model, modelHolder);

            weapon.minDamage = Damage;
            weapon.maxDamage = Damage;

            weapon.shootFrequency = FireRate;

            weapon.reloadTime      = ReloadSeconds;
            weapon.clipSize        = ClipSize;
            weapon.automaticWeapon = Automatic;

            weapon.moveSetID   = MovesetId;
            weapon.upperBodyID = AnimatorId;
            weapon.shotID      = AnimatorId;
            weapon.reloadID    = AnimatorId;
            weapon.equipID     = AnimatorId;

            weapon.weaponCategory = weaponType.ToString();
        }

        [TabGroup("TabGroup", "File")]
        [Button("CREATE PREFAB", ButtonSizes.Large)]
        public void CreatePrefab()
        {
            if (!gameObject) ExportGameObject();
            PrefabUtility.SaveAsPrefabAsset(gameObject, PREFAB_PATH + gameObject.name + ".prefab");
        }

        [TabGroup("TabGroup", "File")]
        [Button("CLICK THIS IF YOU WANT TO DO BOTH OF THEM\n BUT DO NOT WANT TO CLICK 3 TIMES", ButtonSizes.Gigantic)]
        [GUIColor(0.31f, 0.55f, 0.93f, 1f)]
        public void SaveToPrefab()
        {
            SaveSettings();
            ExportGameObject();
            CreatePrefab();
        }
#endif

        public bool IsOwned => PlayerPrefs.HasKey(OwnedWeaponKey);
        public bool IsActivated => PlayerPrefs.HasKey(ActivatedWeaponKey);
        public bool IsBought => IsOwned;
        public bool IsSelected => IsActivated;

        public void Bought()
        {
            Own();
        }

        public void Selected()
        {
            Activate();
        }

        public void Own()
        {
            PlayerPrefs.SetInt(OwnedWeaponKey, 1);
        }

        public void DisOwn()
        {
            PlayerPrefs.DeleteKey(OwnedWeaponKey);
        }

        public void Activate()
        {
            PlayerPrefs.SetInt(ActivatedWeaponKey, 1);
        }

        public void DeActivate()
        {
            PlayerPrefs.DeleteKey(ActivatedWeaponKey);
        }
    }
}