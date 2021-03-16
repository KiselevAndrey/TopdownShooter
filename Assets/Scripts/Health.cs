using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField, Min(0)] int startingHealth;
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
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
