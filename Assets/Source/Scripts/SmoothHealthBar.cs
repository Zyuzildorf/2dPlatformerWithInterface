using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _smoothSpeed = 0.5f;

    private Coroutine _currentCoroutine;
    private Quaternion _fixedRotation;

    private void Awake()
    {
        _fixedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _fixedRotation;
    }

    private void OnEnable()
    {
        _health.HealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int healthValue)
    {
        if (_healthBar == null) return;
        
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        
        _currentCoroutine = StartCoroutine(SmoothHealthChange(healthValue));
    }

    private IEnumerator SmoothHealthChange(int targetHealth)
    {
        float targetValue = (float)targetHealth / _health.MaxHealth;
        float startValue = _healthBar.value;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * _smoothSpeed;
            _healthBar.value = Mathf.MoveTowards(startValue, targetValue, progress);
            yield return null;
        }
        
        _healthBar.value = targetValue;
        _currentCoroutine = null;
    }
}
