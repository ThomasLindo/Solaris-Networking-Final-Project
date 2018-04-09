//Based off of a youtube video
//https://www.youtube.com/watch?v=e8GmfoaOB4Y
//

using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class Stat {
    public enum StatType { None, Damage, Range, ClipSize, AmountCount, FireRate, ReloadTime, ImpactForce, Armour };
    
    [HorizontalGroup("Stats"), LabelWidth(40)]
    public StatType stat = StatType.None;
    [HorizontalGroup("Stats"), LabelWidth(80)]
    public float baseValue;
    [HorizontalGroup("Stats"), LabelWidth(1)]
    private List<float> modifiers = new List<float>(1);

    public Stat(StatType type) { stat = type; }

    //Get all modifiers plus the base value as one value
    public float GetValue() {
        float finalValue = baseValue;
        if (modifiers != null) { modifiers.ForEach(x => finalValue += x); }
        return finalValue;
    }

    //Get all modifiers as one value
    public float GetModifierValue() {
        float finalValue = 0.0f;
        if (modifiers != null) { modifiers.ForEach(x => finalValue += x); }
        return finalValue;
    }

    //Add all modifiers into one value
    public void SquishModifierValue() {
        float finalValue = 0.0f;
        if (modifiers != null) {
            modifiers.ForEach(x => finalValue += x);
            modifiers.ForEach(x => RemoveModifier(x));
        }
        AddModifier(finalValue);
    }

    //Add all modifiers to the base value
    public void SquishValueToBase() {
        float finalValue = baseValue;
        if (modifiers != null) {
            modifiers.ForEach(x => finalValue += x);
            modifiers.ForEach(x => RemoveModifier(x));
        }
        baseValue = finalValue;
    }

    //Add a modifier to an object
    public void AddModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Add(modifier); }
    }

    //Remove a modifier of an object
    public void RemoveModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Remove(modifier); }
    }
    //Remove a modifier of an object
    public void RemoveAllModifiers() {
        if (modifiers != null) { modifiers.Clear(); }
    }

}
