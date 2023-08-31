using System.Linq;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data.MapSets;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.Map;
using TheLongestRoad.MonoBehaviors;
using UnityEngine;

namespace TheLongestRoad;

public partial class Main
{

    [HarmonyPatch(typeof(MapLoader), nameof(MapLoader.LoadScene))]
    [HarmonyPostfix]
    static void MapLoader_LoadScene(MapLoader __instance)
    {
        if (__instance.currentMapName == "The Longest Road")
        {
            __instance.currentMapName = "MuddyPuddles";
            _isMap = true;
        }
        else
        {
            _isMap = false;
        }
    }
    
    [HarmonyPatch(typeof(UnityToSimulation), nameof(UnityToSimulation.InitMap))]
    [HarmonyPrefix]
    internal static void UnityToSimulation_InitMap(UnityToSimulation __instance, ref MapModel map)
    {
        if (!_isMap)
            return;

        GameObject.Find("Seasonal").Destroy();
        GameObject.Find("Particles").Destroy();
        GameObject.Find("Trees").Destroy();


        var original = GameObject.Find("MuddyPuddlesTerrain");

        foreach (var asset in MapItems.AllAssetNames())
        {
            Object.Instantiate(MapItems.LoadAsset(asset).Cast<GameObject>(), original.transform.parent);
        }

        GameObject.Find("map(Clone)").transform.localPosition = new Vector3(-20, 0, 19);

        var leaves = GameObject.Find("BlowingLeaves");
        leaves.transform.localPosition = new Vector3(17, 0, -5);
        leaves.transform.localScale = new Vector3(35, 1, 40);
        var particles = leaves.GetComponent<ParticleSystem>();
        particles.startLifetime = 4.1f;
        particles.emissionRate = 40;
        original.transform.parent.FindChild("GameObject").gameObject.Destroy();
        original.Destroy();


        GameObject.Find("MainWindMillBlades").AddComponent<WindmillSpin>();

        foreach (var childObject in GameObject.Find("Map/Environment/map(Clone)/Town/Trees").transform)
        {
            var child = childObject.Cast<Transform>();
            child.gameObject.AddComponent<TreeBlow>();
        }
        
        GameObject.Find("map(Clone)").name = "TheLongestRoadTerrain";

        map.mapName = "The Longest Road";
        map.mapDifficulty = (int)MapDifficulty.Beginner;
        map.mapWideBloonSpeed = TowerScale;

        map.areas = DataAnalyzer.GetAreas();
        map.paths = DataAnalyzer.GetPaths();
        map.spawner = MapHelper.CreateSpawner(DataAnalyzer.GetPaths());
    }
}