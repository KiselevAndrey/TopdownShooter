using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Стартовые параметры")]
    [SerializeField, Min(0)] int startingHealth;

    [Header("Видимость UI")]
    [SerializeField] bool hideUI;
    [SerializeField, Tooltip("Через сколько спрятать UI")] float visibleUISec;
    [SerializeField] GameObject UIHealth;

    [Header("UI elements")]
    [SerializeField] Slider slider;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image fillImage;

    [Header("Цветовая дифференцияция")]
    [SerializeField] Color fullHealthColor = Color.green;
    [SerializeField] Color zeroHealthColor = Color.red;         
    
    Animator _anim;

    bool _isDead;
    float _currentHealth;

    #region Awake Start
    private void Awake()
    {
        _anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        _currentHealth = startingHealth;
        slider.maxValue = startingHealth;

        SetHealthUI();
    }
    #endregion

    #region Hit
    public void Hit(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
            _anim.SetBool(AnimParam.Dead, true);
        }

        SetHealthUI();
    }

    public bool IsDead() => _isDead;
    #endregion

    private void SetHealthUI()
    {
        slider.value = _currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, _currentHealth / startingHealth);
    }
}
