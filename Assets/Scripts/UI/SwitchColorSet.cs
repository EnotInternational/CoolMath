using UnityEngine;

[CreateAssetMenu(fileName = "SwitchColorSet", menuName = "ColorSets/SwitchColorSet", order = 1)]

public class SwitchColorSet : ScriptableObject
{
    public Color enabledColor;
    public Color pointedColor;
}