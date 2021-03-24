using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Стартовые параметры")]
    [SerializeField, Min(0)] float startingHealth;
    [SerializeField, Min(0)] int maxPlusPercentLife; 

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

    #region Переменные для прятания UI
    bool _isVisible;
    float _visibleTime;
    //float _bgAlpha;
    //float _fillAlpha;
    #endregion

    #region Awake Start Update
    private void Awake()
    {
        _anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        startingHealth += startingHealth * Random.Range(0, maxPlusPercentLife) / 100;
        //startingHealth += startingHealth * Random.value * maxPlusPercentLife;

        _currentHealth = startingHealth;
        slider.maxValue = startingHealth;

        SetHealthUI();

        if (hideUI)
        {
            //_bgAlpha = backgroundImage.color.a;
            //_fillAlpha = fillImage.color.a;
            SetActiveUI(false);
        }
    }

    private void Update()
    {
        if (!_isVisible || _isDead) return;

        if (_visibleTime <= 0)
        {
            SetActiveUI(false);
            _isVisible = false;
        }
        else _visibleTime -= Time.deltaTime;
    }
    #endregion

    #region Hit
    public void Hit(float damage)
    {
        if (_isDead) return;

        SetActiveUI(true);
        _visibleTime = visibleUISec;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            SetActiveUI(false);
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

    #region HideUI
    void SetActiveUI(bool value)
    {
        if (!UIHealth) return;
        _isVisible = value;
        UIHealth.SetActive(value);
        //if (value)
        //{
        //    SetAlpha(backgroundImage, _bgAlpha);
        //    SetAlpha(fillImage, _fillAlpha);
        //}
        //else
        //{
        //    SetAlpha(backgroundImage, 0);
        //    SetAlpha(fillImage, 0);
        //}
    }

    void SetAlpha(Image image, float newAlpha)
    {
        if (!image) return;

        Color temp = image.color;
        temp.a = newAlpha;
        image.color = temp;
    }
    #endregion
}
