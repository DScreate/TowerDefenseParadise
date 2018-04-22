using UnityEngine;



public enum Personality {
    Neutral = 1,
    Shy = (1 << 1),
    Outgoing = (1 << 2),
    Jealous = (1 << 3),
    Crazy = (1 << 4),
    Smart = (1 << 5),
    Cat = (1 << 6)
}

public class EnumFlagsAttribute : PropertyAttribute {
    public EnumFlagsAttribute() { }
}