using System.Collections.Generic;
using TimiShared.Debug;
using TimiSharedUtils;

namespace Game {
    public static class DestructiblePropertiesHelper {

        public enum DestructibleType {
            WOOD_BLOCK,
            ICE_BLOCK,
            STONE_BLOCK,
            WOOD_PILLAR,
            ICE_PILLAR,
            STONE_PILLAR,
            WOOD_TRIANGLE,
            ICE_TRIANGLE,
            WOOD_CIRCLE,
            ICE_CIRCLE,
            STONE_CIRCLE,
            WOOD_PLANK,
            ICE_PLANK,
            STONE_PLANK,
            STONE_HOLLOW_BLOCK,
            MONSTER_LITTLE,
            MONSTER_FAT,
            MONSTER_TOUGH,
        }

        public struct DestructibleProperties {
            public float startingHealth;
            public float mass;
        }

        private static Dictionary<DestructibleType, DestructibleProperties> _propertiesByType = new Dictionary<DestructibleType, DestructibleProperties> {
            {DestructibleType.WOOD_BLOCK, new DestructibleProperties { startingHealth = 10, mass = 10 }},
            {DestructibleType.ICE_BLOCK, new DestructibleProperties { startingHealth = 8, mass = 15 }},
            {DestructibleType.STONE_BLOCK, new DestructibleProperties { startingHealth = 30, mass = 30 }},
            {DestructibleType.WOOD_PILLAR, new DestructibleProperties { startingHealth = 8, mass = 10 }},
            {DestructibleType.ICE_PILLAR, new DestructibleProperties { startingHealth = 5, mass = 15 }},
            {DestructibleType.STONE_PILLAR, new DestructibleProperties { startingHealth = 20, mass = 30 }},
            {DestructibleType.WOOD_TRIANGLE, new DestructibleProperties { startingHealth = 10, mass = 10 }},
            {DestructibleType.ICE_TRIANGLE, new DestructibleProperties { startingHealth = 8, mass = 15 }},
            {DestructibleType.WOOD_CIRCLE, new DestructibleProperties { startingHealth = 10, mass = 10 }},
            {DestructibleType.ICE_CIRCLE, new DestructibleProperties { startingHealth = 8, mass = 15 }},
            {DestructibleType.STONE_CIRCLE, new DestructibleProperties { startingHealth = 30, mass = 30 }},
            {DestructibleType.WOOD_PLANK, new DestructibleProperties { startingHealth = 8, mass = 8 }},
            {DestructibleType.ICE_PLANK, new DestructibleProperties { startingHealth = 5, mass = 10 }},
            {DestructibleType.STONE_PLANK, new DestructibleProperties { startingHealth = 15, mass = 20 }},
            {DestructibleType.STONE_HOLLOW_BLOCK, new DestructibleProperties { startingHealth = 10, mass = 15 }},
            {DestructibleType.MONSTER_LITTLE, new DestructibleProperties { startingHealth = 5, mass = 5 }},
            {DestructibleType.MONSTER_FAT	, new DestructibleProperties { startingHealth = 0.4f, mass = 50 }},
            {DestructibleType.MONSTER_TOUGH	, new DestructibleProperties { startingHealth = 20, mass = 10 }},
        };

        public static float GetStartingHealthForType(DestructibleType destructibleType) {
            DestructibleProperties properties;
            if (_propertiesByType.TryGetValue(destructibleType, out properties)) {
                return properties.startingHealth;
            }
            DebugLog.LogWarningColor("No destructible properties defined for type: " + destructibleType.ToString(), LogColor.yellow);
            return 1;
        }

        public static float GetMassForType(DestructibleType destructibleType) {
            DestructibleProperties properties;
            if (_propertiesByType.TryGetValue(destructibleType, out properties)) {
                return properties.mass;
            }
            DebugLog.LogWarningColor("No destructible properties defined for type: " + destructibleType.ToString(), LogColor.yellow);
            return 1;
        }


    }
}