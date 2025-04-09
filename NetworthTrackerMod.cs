using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Il2CppScheduleOne.Money;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace NetworthTracker
{
    public class NetworthTrackerMod : MelonMod
    {
        public static Transform UI;
        public static Transform HUD;
        public static GameObject networthHolder;
        public static Text networthDisplay;
        public static bool mainSceneLoaded = false;
        public static object networthCoroutineObject;

        public static float networth;

        public float Networth
        {
            set 
            {
                networth = value;
                networthDisplay.text = $"Networth: ${value:N2}";
            }
            get => networth;
        }

        // Version Info
        private const string versionCurrent = "1.0.0";
        private const string versionMostUpToDateURL = "https://raw.githubusercontent.com/Voidane/NetworthTracker/refs/heads/master/Version.txt";
        private string versionUpdate = null;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg($"===========================================");
            MelonLogger.Msg($"Initializing, Created by Voidane.");
            MelonLogger.Msg($"Discord: discord.gg/XB7ruKtJje");
            new ConfigData();
            CheckForUpdates();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                mainSceneLoaded = true;
                MelonCoroutines.Start(WaitOnSceneLoad(null, "UI", 20.0F, (_UI) =>
                {
                    UI = _UI;
                    MelonCoroutines.Start(WaitOnSceneLoad(UI, "HUD", 5.0F, (_HUD) =>
                    {
                        HUD = _HUD;
                        CreateNetworthUI();
                        networthCoroutineObject = MelonCoroutines.Start(UpdateNetworth());
                    }));
                    
                }));
            }
            else
            {
                if (networthCoroutineObject != null)
                {
                    MelonCoroutines.Stop(networthCoroutineObject);
                }

                mainSceneLoaded = false;
            }
        }

        private async void CheckForUpdates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string content = await client.GetStringAsync(versionMostUpToDateURL);
                    versionUpdate = content.Trim();
                }
            }
            catch (Exception e)
            {
                MelonLogger.Msg($"Could not fetch most up to date version {e.Message}");
            }

            if (versionCurrent != versionUpdate)
            {
                MelonLogger.Msg($"New Update for no trash mod! https://www.nexusmods.com/schedule1/mods/221?tab=files, Current: {versionCurrent}, Update: {versionUpdate}");
            }

            MelonLogger.Msg($"Has been initialized...");
            MelonLogger.Msg($"===========================================");
        }

        private IEnumerator UpdateNetworth()
        {
            while (true)
            {
                MoneyManager money = MoneyManager.Instance;
                Networth = (float) Math.Round(money.GetNetWorth(), 2);
                yield return new WaitForSeconds(1.0F);
            }
        }

        private IEnumerator WaitOnSceneLoad(Transform parent, string name, float timeoutLimit, Action<Transform> onComplete)
        {
            Transform target = null;
            float timeOutCounter = 0F;
            int attempt = 0;

            while (target == null && timeOutCounter < timeoutLimit)
            {
                target = (parent == null) ? GameObject.Find(name).transform : parent.Find(name);
                if (target == null)
                {
                    timeOutCounter += 0.5F;
                    yield return new WaitForSeconds(0.5F);
                }
            }

            if (target != null)
            {
                onComplete?.Invoke(target);
            }
            else
            {
                MelonLogger.Error("Failed to find target object within timeout period!");
                onComplete?.Invoke(null);
            }

            yield return target;
        }

        private void CreateNetworthUI()
        {
            networthHolder = new GameObject("NetworthHolder");
            networthHolder.transform.SetParent(HUD, false);
            
            RectTransform holderRect = networthHolder.AddComponent<RectTransform>();

            if (ConfigData.Instance == null)
            {
                new ConfigData();
            }

            if (ConfigData.useCustomPosition.Value)
            {
                holderRect.anchorMin = new Vector2(ConfigData.anchorMinX.Value, ConfigData.anchorMinY.Value);
                holderRect.anchorMax = new Vector2(ConfigData.anchorMaxX.Value, ConfigData.anchorMaxY.Value);
                holderRect.offsetMin = new Vector2(ConfigData.offsetMinX.Value, ConfigData.offsetMinY.Value);
                holderRect.offsetMax = new Vector2(ConfigData.offsetMaxX.Value, ConfigData.offsetMaxY.Value);
            }
            else if (ConfigData.topRight.Value)
            {
                holderRect.anchorMin = new Vector2(0.8f, 0.9f);
                holderRect.anchorMax = new Vector2(1.0f, 1.0f);
                holderRect.offsetMin = new Vector2(10f, 10f);
                holderRect.offsetMax = new Vector2(-10f, -10f);
            }
            else if (ConfigData.topLeft.Value)
            {
                holderRect.anchorMin = new Vector2(0.0f, 0.9f);
                holderRect.anchorMax = new Vector2(0.2f, 1.0f);
                holderRect.offsetMin = new Vector2(10f, 10f);
                holderRect.offsetMax = new Vector2(-10f, -10f);
            }
            else if (ConfigData.bottomLeft.Value)
            {
                holderRect.anchorMin = new Vector2(0.0f, 0.0f);
                holderRect.anchorMax = new Vector2(0.2f, 0.1f);
                holderRect.offsetMin = new Vector2(10f, 10f);
                holderRect.offsetMax = new Vector2(-10f, -10f);
            }
            else if (ConfigData.bottomRight.Value)
            {
                holderRect.anchorMin = new Vector2(0.8f, 0.0f);
                holderRect.anchorMax = new Vector2(1.0f, 0.1f);
                holderRect.offsetMin = new Vector2(10f, 10f);
                holderRect.offsetMax = new Vector2(-10f, -10f);
            }

            GameObject networthDisplayGameobject = new GameObject("NetworthText");
            networthDisplayGameobject.transform.SetParent(networthHolder.transform, false);

            networthDisplay = networthDisplayGameobject.AddComponent<Text>();
            networthDisplay.text = "Networth: SYNCING";
            networthDisplay.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            networthDisplay.fontSize = 18;
            networthDisplay.alignment = TextAnchor.MiddleCenter;
            networthDisplay.color = Color.green;

            RectTransform textRect = networthDisplayGameobject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
    }
}
