using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Стартовые параметры")]
    [SerializeField, Min(0)] float startingHealth;
    [SerializeField, Min(0)] int possibleAdditionalHealth; 

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

    [Header("Звук получения урона")]
    [SerializeField] AudioClip hitClip;
    
    Animator _anim;
    AudioSource _audioSource;

    bool _isDead;
    float _currentHealth;
    float _lastTimeHit;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        startingHealth += Random.Range(0, possibleAdditionalHealth);

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
    public void Hit(float damage, bool playSound = true)
    {
        if (_isDead) return;

        if (_audioSource && hitClip && playSound)
        {
            if(Time.realtimeSinceStartup - _lastTimeHit > hitClip.length)
            {
                _audioSource.PlayOneShot(hitClip);
                _lastTimeHit = Time.realtimeSinceStartup;
            }
        }
        SetActiveUI(true);
        _visibleTime = visibleUISec;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            SetActiveUI(false);
            _isDead = true;

            if (_anim)
                _anim.SetBool(AnimParam.Dead, true);
            else
                Destroy(gameObject);
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
