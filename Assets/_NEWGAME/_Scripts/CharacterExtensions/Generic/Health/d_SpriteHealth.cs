using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts._CharacterController
{
    [ClassHeader("SpriteHealth", "Assign your canvas object in the 'healthBar' field to hide and only display when receive damage - leave it empty if you want to display the healthbar all the time.", openClose = false)]
    public class d_SpriteHealth : ExtendMonoBehaviour
    {
        [Tooltip("UI to show on receive damage")]
        [SerializeField]
        protected GameObject healthBar;
        public bool lookToCamera = true;
        [Header("UI properties")]
        [SerializeField] protected Slider _healthSlider;
        [SerializeField] protected Slider _damageDelay;
        [SerializeField] protected float _smoothDamageDelay;
        [SerializeField] protected Text _damageCounter;
        [SerializeField] protected float _damageCounterTimer = 1.5f;
        [SerializeField] protected bool _showDamageType = true;

        private HealthController healthControl;
        private bool inDelay;
        private float damage;
        private float currentSmoothDamage;
        internal Transform cameraMain;

        void Start()
        {
            cameraMain = Camera.main ? Camera.main.transform : null;
            healthControl = transform.GetComponentInParent<HealthController>();
            if (healthControl == null)
            {
                Debug.LogWarning("The character must have a ICharacter Interface");
                Destroy(gameObject);
            }
            healthControl.onReceiveDamage.AddListener(Damage);
            _healthSlider.maxValue = healthControl.maxHealth;
            _healthSlider.value = _healthSlider.maxValue;
            _damageDelay.maxValue = healthControl.maxHealth;
            _damageDelay.value = _healthSlider.maxValue;
            _damageCounter.text = string.Empty;
            if (healthBar) healthBar.SetActive(false);
        }

        void SpriteBehaviour()
        {
            if (lookToCamera && cameraMain != null) transform.LookAt(cameraMain.position, Vector3.up);

            if (healthControl == null || healthControl.CurrentHealth <= 0)
                Destroy(gameObject);

            _healthSlider.value = healthControl.CurrentHealth;
        }

        void Update()
        {
            if (!healthBar)
            {
                SpriteBehaviour();
            }
        }

        public void Damage(Damage damage)
        {
            try
            {
                this.damage += damage.damageValue;
                _damageCounter.text = this.damage.ToString("00") + (_showDamageType && !string.IsNullOrEmpty(damage.damageType) ? " : by " + damage.damageType : "");
                _healthSlider.value -= damage.damageValue;

                if (!inDelay && healthControl && healthControl.gameObject.activeInHierarchy)
                    StartCoroutine(DamageDelay());
            }
            catch
            {
                Destroy(this);
            }
        }

        IEnumerator DamageDelay()
        {
            inDelay = true;
            if (healthBar) SpriteBehaviour();
            if (healthBar) healthBar.SetActive(true);

            while (_damageDelay.value > _healthSlider.value)
            {
                if (healthBar) SpriteBehaviour();
                _damageDelay.value -= _smoothDamageDelay;

                yield return null;
            }
            inDelay = false;

            yield return new WaitForSeconds(_damageCounterTimer);
            damage = 0;
            _damageCounter.text = string.Empty;
            if (healthBar) healthBar.SetActive(false);
        }
    }
}