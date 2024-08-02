using System;
using System.Collections;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public const float fullDayLength = 60f;
    [field: SerializeField] public Day Day { get; private set; } = Day.Unoday;
    [field: SerializeField] public float DayTime { get; private set; } = 0f;
    [field: SerializeField] public static TodayBonus TodayBonus { get; private set; }
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
    }
    void ChangeTodayBonus()
    {

        TodayBonus[] bonuses = (TodayBonus[])Enum.GetValues(typeof(TodayBonus));
        int randIndex = UnityEngine.Random.Range(0, bonuses.Length);
        TodayBonus = bonuses[randIndex];
        if (HUDScript.Instance != null)
            HUDScript.Instance.UpdateTodayBonus();
        HUDScript.Instance.UpdateTodayBonus();

        //TodayBonus = TodayBonus.NothingButURBeautifull;
    }
    public static bool IsTodayBonusIngredients()
    {
        return TodayBonus == TodayBonus.Dough || TodayBonus == TodayBonus.Sauce || TodayBonus == TodayBonus.Toppings;
    }
    private void ResetAll()
    {
        foreach (var o in Oven.ovens)
        {
            o.Reset();
        }
        foreach (var g in GoblinTransporter.Goblins)
        {
            g.State.Reset();
        }
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
    Nothing,
    NothingButURBeautifull
}