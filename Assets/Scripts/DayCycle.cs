using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public const float fullDayLength = 60f;
    public Day Day { get; private set; } = Day.Unoday;
    public float DayTime { get; private set; } = 0f;
    [SerializeField] private Day DebugDay = Day.Unoday;
    public static TodayBonus TodayBonus { get; private set; }
    public TodayBonus DayBonusDebug;
    private void Start()
    {
        ChangeTodayBonus();
    }
    private void Update()
    {
        TimePlus();
        RotateSun();
    }
    void RotateSun()
    {
        float maxSunHeight = 55f; // realistic 45f-90f

        // Calculate the current time of day as a percentage (0 to 1)
        float dayFraction = DayTime / fullDayLength;

        // Calculate the sun's angle based on the current time of day
        float sunXAngle = Mathf.Lerp(-maxSunHeight, maxSunHeight, Mathf.Sin(dayFraction * Mathf.PI));

        // Calculate the horizontal movement (east to west)
        float sunYAngle = dayFraction * 360f - 90f; // -90 to start in the east

        // Apply the rotation to the sun
        transform.rotation = Quaternion.Euler(new Vector3(sunXAngle, sunYAngle, 0f));
        GetComponent<Light>().intensity = MathF.Max(0, sunXAngle / maxSunHeight);
    }
    void TimePlus()
    {
        DayTime += Time.deltaTime;
        if (DayTime > fullDayLength)
        {
            DayTime = 0f;
            NextDay();
        }
    }

    void NextDay()
    {
        if (Day != Day.Quidday) Day++;
        else Day = Day.Unoday;
        ChangeTodayBonus();
        DebugDay = Day;

    }
    void ChangeTodayBonus()
    {
        TodayBonus[] bonuses = (TodayBonus[])Enum.GetValues(typeof(TodayBonus));
        int randIndex = UnityEngine.Random.Range(0, bonuses.Length);
        TodayBonus = bonuses[randIndex];
        DayBonusDebug = TodayBonus;
    }
    public static bool IsTodayBonusIngredients()
    {
        return TodayBonus == TodayBonus.Dough || TodayBonus == TodayBonus.Sauce || TodayBonus == TodayBonus.Toppings;
    }
}

public enum Day
{
    Unoday,
    Duoday,
    Tripday,
    Quadday,
    Quidday,
}

public enum TodayBonus
{
    Dough,
    Sauce,
    Toppings,
    SellPrice,
    Nothing
}