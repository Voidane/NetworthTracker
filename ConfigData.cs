using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace NetworthTracker
{
    public class ConfigData
    {
        public static MelonPreferences_Category customNetworthPositioning;
        public static MelonPreferences_Entry<bool> useCustomPosition;
        public static MelonPreferences_Entry<float> anchorMinX;
        public static MelonPreferences_Entry<float> anchorMinY;
        public static MelonPreferences_Entry<float> anchorMaxX;
        public static MelonPreferences_Entry<float> anchorMaxY;
        public static MelonPreferences_Entry<float> offsetMinX;
        public static MelonPreferences_Entry<float> offsetMinY;
        public static MelonPreferences_Entry<float> offsetMaxX;
        public static MelonPreferences_Entry<float> offsetMaxY;

        public static MelonPreferences_Category presetNetworkPositioning;
        public static MelonPreferences_Entry<bool> topLeft;
        public static MelonPreferences_Entry<bool> topRight;
        public static MelonPreferences_Entry<bool> bottomLeft;
        public static MelonPreferences_Entry<bool> bottomRight;

        public static ConfigData Instance;

        public ConfigData()
        {
            Instance = this;

            customNetworthPositioning = MelonPreferences.CreateCategory("UI Custom Positioning");
            useCustomPosition = customNetworthPositioning.CreateEntry<bool>("Use Custom Positioning", false, "Use Custom Positioning",
                "Allows the mod to configure the transform position of where you want the networth UI to be. (more precise positioning)");
            anchorMinX = customNetworthPositioning.CreateEntry<float>("Anchor Min X", 0.8F);
            anchorMinY = customNetworthPositioning.CreateEntry<float>("Anchor Min Y", 0.9F);
            anchorMaxX = customNetworthPositioning.CreateEntry<float>("Anchor Max X", 1.0F);
            anchorMaxY = customNetworthPositioning.CreateEntry<float>("Anchor Max Y", 1.0F);
            offsetMinX = customNetworthPositioning.CreateEntry<float>("Offset Min X", 10.0F);
            offsetMinY = customNetworthPositioning.CreateEntry<float>("Offset Min Y", 10.0F);
            offsetMaxX = customNetworthPositioning.CreateEntry<float>("Offset Max X", -10.0F);
            offsetMaxY = customNetworthPositioning.CreateEntry<float>("Offset Max Y", -10.0F);

            presetNetworkPositioning = MelonPreferences.CreateCategory("UI Preset Positioning");
            topLeft = presetNetworkPositioning.CreateEntry<bool>("Top Left", false);
            topRight = presetNetworkPositioning.CreateEntry<bool>("Top Right", true);
            bottomLeft = presetNetworkPositioning.CreateEntry<bool>("Bottom Left", false);
            bottomRight = presetNetworkPositioning.CreateEntry<bool>("Bottom Right", false);

            customNetworthPositioning.SetFilePath("UserData/NetworthTracker.cfg");
            presetNetworkPositioning.SetFilePath("UserData/NetworthTracker.cfg");
            customNetworthPositioning.SaveToFile();
            presetNetworkPositioning.SaveToFile();
        }
    }
}
