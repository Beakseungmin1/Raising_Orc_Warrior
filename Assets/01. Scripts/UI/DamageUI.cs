using UnityEngine;
using System.Numerics;
using TMPro;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float displayDuration = 0.5f;

    private float displayTimer;
    private bool isDisplaying;

    private void Start()
    {
        damageText.gameObject.SetActive(false);

        PlayerObjManager.Instance.Player.PlayerBattle.OnPlayerAttack += OnPlayerAttackHandler;
    }

    private void Update()
    {
        if (isDisplaying)
        {
            displayTimer -= Time.deltaTime;

            if (displayTimer <= 0f)
            {
                isDisplaying = false;
                damageText.gameObject.SetActive(false);
            }
        }
    }

    private void OnPlayerAttackHandler()
    {
        BigInteger damage = PlayerObjManager.Instance.Player.DamageCalculator.GetTotalDamage();
        ShowDamage(damage);
    }

    private void ShowDamage(BigInteger damage)
    {
        damageText.text = damage.ToString();
        damageText.gameObject.SetActive(true);

        displayTimer = displayDuration;
        isDisplaying = true;
    }

    private void OnDestroy()
    {
        if (PlayerObjManager.Instance.Player != null)
        {
            PlayerObjManager.Instance.Player.PlayerBattle.OnPlayerAttack -= OnPlayerAttackHandler;
        }
    }
}