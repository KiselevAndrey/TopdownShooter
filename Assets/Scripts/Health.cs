using System;
using System.Collections.Generic;
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

    [Header("Звуки")]
    [SerializeField] List<AudioClip> hitClips;
    [SerializeField] List<AudioClip> deathClips;

    [Header("Атрибуты")]
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource;

    public static Action<string, Vector2> ImDeath;

    bool _isDead;
    float _currentHealth;
    float _lastTimeHit;
    int _hitClipIndex;
    #region Переменные для прятания UI
    bool _isVisible;
    float _visibleTime;
    #endregion

    #region Awake Start Update
    private void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out audioSource);
    }

    private void Start()
    {
        startingHealth += UnityEngine.Random.Range(0, possibleAdditionalHealth);

        _currentHealth = startingHealth;
        slider.maxValue = startingHealth;

        SetHealthUI();

        if (hideUI)
        {
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

        if (audioSource && hitClips.Count > 0 && playSound)
        {
            if(Time.realtimeSinceStartup - _lastTimeHit > hitClips[_hitClipIndex].length)
            {
                _hitClipIndex = UnityEngine.Random.Range(0, hitClips.Count);    // новый индекс звука
                audioSource.PlayOneShot(hitClips[_hitClipIndex]);
                _lastTimeHit = Time.realtimeSinceStartup;
            }
        }
        SetActiveUI(true);
        _visibleTime = visibleUISec;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
            Death(playSound);

        SetHealthUI();
    }

    void SetHealthUI()
    {
        slider.value = _currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, _currentHealth / startingHealth);
    }
    #endregion

    #region Death
    public bool IsDead() => _isDead;

    void Death(bool playSound)
    {
        _currentHealth = 0;
        SetActiveUI(false);
        _isDead = true;

        if (audioSource && deathClips.Count > 0 && playSound)
            audioSource.PlayOneShot(deathClips[UnityEngine.Random.Range(0, deathClips.Count)]);

        if (anim)
            anim.SetBool(AnimParam.Dead, true);
        else
            Destroy(gameObject);
    }
    #endregion

    #region HideUI
    void SetActiveUI(bool value)
    {
        if (!UIHealth) return;
        _isVisible = value;
        UIHealth.SetActive(value);
    }
    #endregion
}
