using _GAME._Scripts;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using System.Linq;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

    private Weapon _selectedSkin;

    private WeaponResources _weaponResources => GameManager.Instance.Resources.WeaponResources;

    public SkinnedMeshRenderer SkinnedMeshRenderer { get => _skinnedMeshRenderer; set => _skinnedMeshRenderer = value; }

    private void Start()
    {
        EquipSkin();
    }

    public void EquipSkin(Weapon skin = null)
    {
        _selectedSkin = (skin == null) ? _weaponResources.WeaponSettingsByType[WeaponType.Skin]
            .Select(skinSettings => skinSettings.Entity)
            .First((skin) => skin.IsActivated) : skin;
        SkinnedMeshRenderer.material = _selectedSkin.Material;
    }

}
