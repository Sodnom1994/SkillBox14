using UnityEngine;
using System.Collections.Generic;

public class ItemsInRHandScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> SpyItems = new List<GameObject>();
    [SerializeField] private Animator animator; // Ссылка на Animator

    [Range(0, 2)]
    [SerializeField] private int CurrentItemIndex = 0;

    // Хэши параметров (для производительности)
    private int currentItemInHandHash = Animator.StringToHash("CurrentItemInHand");
    private int animationStateHash = Animator.StringToHash("AnimationState");

    private void Start()
    {
        UpdateItemsAndAnimation();
    }

    private void OnValidate()
    {
        UpdateItemsAndAnimation();
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Смена предмета → сбрасывает в Idle
        if (scroll > 0f)
        {
            CurrentItemIndex = (CurrentItemIndex + 1) % SpyItems.Count;
            UpdateItemsAndAnimation();
        }
        else if (scroll < 0f)
        {
            CurrentItemIndex = (CurrentItemIndex - 1 + SpyItems.Count) % SpyItems.Count;
            UpdateItemsAndAnimation();
        }

        // Управление анимациями (клавиши 1–5)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetAnimationState(0); // Idle
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetAnimationState(1); // Run
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetAnimationState(2); // Strafe
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetAnimationState(3); // Kneeling
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetAnimationState(4); // Hit Reaction
    }

    /// <summary>
    /// Меняет предмет в руке и сбрасывает анимацию на Idle
    /// Вызывается при прокрутке колеса
    /// </summary>
    private void UpdateItemsAndAnimation()
    {
        if (SpyItems == null || SpyItems.Count == 0 || animator == null)
            return;

        // Ограничиваем индекс
        CurrentItemIndex = Mathf.Clamp(CurrentItemIndex, 0, SpyItems.Count - 1);

        // Выключаем все предметы
        foreach (GameObject item in SpyItems)
        {
            if (item != null)
                item.SetActive(false);
        }

        // Включаем текущий
        if (SpyItems[CurrentItemIndex] != null)
        {
            SpyItems[CurrentItemIndex].SetActive(true);
        }

        // Обновляем анимации:
        // 1. Указываем, какой предмет в руке
        animator.SetInteger(currentItemInHandHash, CurrentItemIndex);

        // 2. Сбрасываем на Idle (0)
        animator.SetInteger(animationStateHash, 0);
    }

    /// <summary>
    /// Только меняет анимацию, не трогая предмет
    /// Вызывается при нажатии клавиш 1–5
    /// </summary>
    /// <param name="state">Номер анимации (0=Idle, 1=Run и т.д.)</param>
    public void SetAnimationState(int state)
    {
        if (animator != null)
        {
            animator.SetInteger(animationStateHash, state);
        }
    }
}