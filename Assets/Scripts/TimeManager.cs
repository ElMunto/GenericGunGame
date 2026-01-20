using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float slowDownFactor = 0.05f;
    [SerializeField] private float slowdownLength = 2f;
    [SerializeField] private float refillSpeed = 0.5f;

    private bool isSlowing = false;
    private float currentSlowdownDuration = 0f;
    private float slowdownTimer = 0f;
    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = slowdownLength;
        slider.value = slowdownLength;
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer += Time.unscaledDeltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }

        if (isSlowing)
        {
            slowdownTimer += Time.unscaledDeltaTime;
            slider.value = Mathf.Lerp(slowdownLength, 0, slowdownTimer / currentSlowdownDuration);

            Time.timeScale += (1f / currentSlowdownDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, slowDownFactor, 1f);

            if (slowdownTimer >= currentSlowdownDuration || Time.timeScale >= 0.99f)
            {
                Time.timeScale = 1f;
                isSlowing = false;
                slowdownTimer = 0f;
                isCooldown = true;
                cooldownTimer = 0f;
            }
        }
        else if (!isCooldown)
        {
            // Refill the slider when not slowing and not in cooldown
            if (slider.value < slowdownLength)
                slider.value += refillSpeed * Time.unscaledDeltaTime;
            if (slider.value > slowdownLength)
                slider.value = slowdownLength;
        }
    }

    public void DoSlowmotion()
    {
        // If slider is empty, cooldown is active, or already slowing, do nothing
        if (slider.value <= 0.01f || isCooldown || isSlowing)
            return;

        // Duration is proportional to current slider value
        currentSlowdownDuration = Mathf.Lerp(0.2f, slowdownLength, slider.value / slowdownLength);
        Time.timeScale = slowDownFactor;
        isSlowing = true;
        slowdownTimer = 0f;

        // Immediately reduce the slider value to zero to prevent spamming
        slider.value = 0f;
    }
}
