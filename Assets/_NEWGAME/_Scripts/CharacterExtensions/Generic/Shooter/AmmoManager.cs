using UnityEngine;
using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Ammo Manager", iconName = "ammoIcon")]
    public class AmmoManager : ExtendMonoBehaviour
    {
        public AmmoListData ammoListData;
        [@HideInInspector]
        public ItemManager itemManager;

        //[HideInInspector]
        public List<Ammo> ammos = new List<Ammo>();
        public delegate void OnUpdateTotalAmmo();
        public OnUpdateTotalAmmo updateTotalAmmo = delegate { };

        void Start()
        {
            itemManager = GetComponent<ItemManager>();
            if (itemManager)
            {
                itemManager.onAddItem.AddListener(AddAmmo);
                itemManager.onDropItem.AddListener(DropAmmo);
                itemManager.onDestroyItem.AddListener(LeaveAmmo);
                itemManager.onChangeItemAmount.AddListener(ChangeItemAmount);
                itemManager.onLoadItems.AddListener(ReloadAllAmmoItems);
            }

            if (ammoListData)
            {
                ammos.Clear();
                for (int i = 0; i < ammoListData.ammos.Count; i++)
                {
                    var ammo = new Ammo(ammoListData.ammos[i]);
                    ammo.onDestroyAmmoItem = new Ammo.OnDestroyItem(OnDestroyAmmoItem);
                    ammos.Add(ammo);
                }
            }
        }

        public Ammo GetAmmo(int id)
        {
            return ammos.Find(a => a.ammoID == id);
        }

        /// <summary>
        /// Use with AmmoStandalone Solution
        /// </summary>      
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void AddAmmo(int id, int amount)
        {
            var ammo = ammos.Find(a => a.ammoID == id);
            if (ammo != null)
            {
                ammo.AddAmmo(amount);
            }
            UpdateTotalAmmo();
        }

        /// <summary>
        /// Use with AmmoStandalone Solution
        /// </summary>
        /// <param name="ammoName"></param>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void AddAmmo(string ammoName, int id, int amount)
        {
            var ammo = ammos.Find(a => a.ammoID == id);
            if (ammo == null)
            {
                ammo = new Ammo(ammoName, id, amount);
                ammos.Add(ammo);
                ammo.onDestroyAmmoItem = new Ammo.OnDestroyItem(OnDestroyAmmoItem);
            }
            else if (ammo != null)
            {
                ammo.AddAmmo(amount);
            }
            UpdateTotalAmmo();
        }

        /// <summary>
        /// Use with ItemManager
        /// </summary>
        /// <param name="item"></param>
        public void AddAmmo(Item item)
        {
            if (item.type == ItemType.Ammo)
            {
                var ammo = ammos.Find(a => a.ammoID == item.id);
                if (ammo == null)
                {
                    ammo = new Ammo(item.name, item.id, 0);
                    ammos.Add(ammo);
                    ammo.onDestroyAmmoItem = new Ammo.OnDestroyItem(OnDestroyAmmoItem);
                }

                ammo.ammoItems.Add(item);
            }
            else
            {
                var _ammo = item.GetItemAttribute(ItemAttributes.AmmoCount);

                if (_ammo != null)
                {
                    var weaponReference = item.originalObject.GetComponent<_CharacterController._Shooter.ShooterWeapon>();
                    if (weaponReference != null)
                    {
                        if (_ammo != null && _ammo.value > weaponReference.clipSize)
                        {
                            var extraAmmo = _ammo.value - weaponReference.clipSize;
                            _ammo.value -= extraAmmo;
                            ItemReference newAmmoItem = new ItemReference(weaponReference.ammoID);
                            newAmmoItem.amount = extraAmmo;
                            itemManager.CollectItem(newAmmoItem, ignoreItemAnimation: true);
                        }
                    }
                }
            }
            UpdateTotalAmmo();
        }

        protected void ChangeItemAmount(Item item)
        {
            if (item.type == ItemType.Ammo)
            {
                var ammo = ammos.Find(a => a.ammoID == item.id);
                if (ammo == null)
                {
                    ammo = new Ammo(item.name, item.id, item.amount);
                    ammos.Add(ammo);
                    ammo.onDestroyAmmoItem = new Ammo.OnDestroyItem(OnDestroyAmmoItem);
                }
            }
            UpdateTotalAmmo();
        }

        public void LeaveAmmo(Item item, int amount)
        {
            if (item.type == ItemType.Ammo)
            {
                var ammo = ammos.Find(a => a.ammoID == item.id);
                if (ammo != null)
                {
                    if (item.amount - amount <= 0 && ammo.ammoItems.Contains(item))
                        ammo.ammoItems.Remove(item);
                }
            }
            UpdateTotalAmmo();
        }

        public void DropAmmo(Item item, int amount)
        {
            if (item.type == ItemType.Ammo)
            {
                var ammo = ammos.Find(a => a.ammoID == item.id);
                if (ammo != null)
                {
                    if (item.amount - amount <= 0 && ammo.ammoItems.Contains(item))
                        ammo.ammoItems.Remove(item);
                }
            }
            UpdateTotalAmmo();
        }

        public void UpdateTotalAmmo()
        {
            updateTotalAmmo.Invoke();
        }

        public void ReloadAllAmmoItems()
        {
            var ammosInManager = itemManager.items.FindAll(item => item.type == ItemType.Ammo);
            ammos.Clear();
            for (int i = 0; i < ammosInManager.Count; i++)
            {
                AddAmmo(ammosInManager[i]);
            }
        }

        void OnDestroyAmmoItem(Item item)
        {
            if (itemManager) itemManager.DestroyItem(item, item.amount);
        }
    }

    [System.Serializable]
    public class Ammo
    {
        public string ammoName;
        [Tooltip("Ammo ID - if is using ItemManager, make sure your AmmoManager and ItemListData use the same ID")]
        public int ammoID;
        [Tooltip("Don't need to setup if you're using a Inventory System")]
        [SerializeField]
        private int _count;

        public List<Item> ammoItems;
        public delegate void OnDestroyItem(Item item);
        public OnDestroyItem onDestroyAmmoItem = delegate { };

        public Ammo()
        {
            ammoItems = new List<Item>();
        }

        public Ammo(string ammoName, int ammoID, int amount = 0)
        {
            this.ammoName = ammoName;
            this.ammoID = ammoID;
            _count = amount;
            ammoItems = new List<Item>();
        }

        public Ammo(int ammoID, int amount = 0)
        {
            this.ammoID = ammoID;
            _count = amount;
            ammoItems = new List<Item>();
        }

        public Ammo(Ammo ammo)
        {
            ammoName = ammo.ammoName;
            ammoID = ammo.ammoID;
            ammoItems = ammo.ammoItems;
            _count = ammo.count;
            ammoItems = new List<Item>();
        }

        public virtual int count
        {
            get
            {
                var value = 0;
                if (ammoItems != null && ammoItems.Count > 0)
                {
                    for (int i = 0; i < ammoItems.Count; i++)
                        if (ammoItems[i])
                            value += ammoItems[i].amount;
                }
                return _count + value;
            }
        }

        public virtual void Use()
        {
            var ammoItem = ammoItems.Find(a => a.amount > 0);
            if (ammoItem)
            {
                ammoItem.amount--;
                if (ammoItem.amount == 0)
                {
                    ammoItems.Remove(ammoItem);
                    onDestroyAmmoItem(ammoItem);
                }
                return;
            }
            else if (_count > 0) _count--;
        }

        public virtual void Use(int amout)
        {
            for (int i = 0; i < amout; i++) Use();
        }

        public void AddAmmo(int amount)
        {
            _count += amount;
        }
    }
}