using System;
using System.Collections.Generic;
using UnityEngine;

public static class Recipes
{

    public static Item[] items;
    public static Dictionary<String, String> recipes;
    static Recipes()
    {
        items = Resources.LoadAll<Item>("Items");
        recipes = new Dictionary<string, string>
        {
            { "StickPebble", "Trowel" },
            { "PebbleStick", "Trowel" },
            {"StickStick", "Plank"},
            {"PebblePebble", "Stone"},
            {"PlankStone", "Hatchet"},
            {"StonePlank", "Hatchet"},
            {"PebbleDirt", "Flint"},
            {"DirtPebble", "Flint"},
            {"FlintStick", "Mattock"},
            {"StickFlint", "Mattock"}
        };
    }

    public static Item fetchItem(String key) {
        if (!recipes.TryGetValue(key, out string value))
        {
            value = "Garbage";
        }
        for (int i = 0; i < items.Length; i++) {
            if (items[i].itemName == value) {
                return items[i];
            }
        }
        return null;
    }
}
