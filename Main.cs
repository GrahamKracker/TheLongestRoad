#region

using System.Reflection;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.MapSets;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using TheLongestRoad;
using TheLongestRoad.MonoBehaviors;
using UnityEngine;
using Main = TheLongestRoad.Main;

#endregion

[assembly: MelonInfo(typeof(Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace TheLongestRoad;


[HarmonyPatch]
public partial class Main : BloonsTD6Mod
{
    /*
    private bool _gameStart = true;
    public override void OnMainMenu()
    {
        if (_gameStart)
        {
            InGameData.Editable.selectedMode = "Sandbox";
            InGameData.Editable.selectedMap = "The Longest Road";
            InGameData.Editable.selectedDifficulty = "Easy";
            UI.instance.LoadGame();
            _gameStart = false;
        }
    }
    //*/
    
    public new static Assembly Assembly => typeof(Main).Assembly;
    
    private static bool _isMap;

    private static AssetBundle? MapItems;

    internal const float TowerScale = 1/3f;
    
    private const bool ShouldScale = true; //testing purposes

    public override void OnTitleScreen()
    {
        MapItems ??= ModContent.GetBundle<Main>("mapitems");
        
        GameData.Instance.mapSet.Maps.items = GameData.Instance.mapSet.Maps.items.AddTo(new MapDetails
        {
            id = "The Longest Road",
            isBrowserOnly = false,
            isDebug = false,
            difficulty = MapDifficulty.Beginner,
            coopMapDivisionType = CoopDivision.DEFAULT,
            mapMusic = "MusicBTD5JazzA",
            mapSprite = ModContent.GetSpriteReference<Main>("TheLongestRoad"),
            hasWater = true
        });
        Game.instance.ScheduleTask(() => Game.instance.GetBtd6Player().UnlockMap("The Longest Road"),
            () => Game.instance && Game.instance.GetBtd6Player() != null);
    }
    

    public override void OnNewGameModel(GameModel result)
    {
        if (!_isMap || !ShouldScale) return;
        
        foreach (var displayModel in result.GetDescendants<DisplayModel>().ToList())
        {
            displayModel.scale *= TowerScale;
            displayModel.positionOffset *= TowerScale;
        }

        /*foreach (var bloon in result.GetDescendants<BloonModel>().ToList())
        {
            //bloon.radius *= TowerScale;
            if (bloon.collisionGroup is not { collisionGroup: not null }) continue;
            foreach(var collidethingy in bloon.collisionGroup.collisionGroup)
            {
                collidethingy.radius *= TowerScale;
            }
        }*/
        
        foreach (var tower in result.GetDescendants<TowerModel>().ToList())
        {
            tower.range *= TowerScale;
            foreach (var am in tower.GetAttackModels())
            {
                am.range *= TowerScale;

                am.offsetX *= TowerScale;
                am.offsetY *= TowerScale;
                am.offsetZ *= TowerScale;

                foreach (var weaponModel in am.weapons)
                {
                    weaponModel.ejectX *= TowerScale;
                    weaponModel.ejectY *= TowerScale;
                    weaponModel.ejectZ *= TowerScale;
                }
            }
            tower.radius *= TowerScale;
            var circleFootprintModel = tower.footprint.TryCast<CircleFootprintModel>();
            var rectangleFootprintModel = tower.footprint.TryCast<RectangleFootprintModel>();
            
            if (circleFootprintModel != null)
            {
                circleFootprintModel.radius *= TowerScale;
            }
            
            if (rectangleFootprintModel != null)
            {
                rectangleFootprintModel.xWidth *= TowerScale;
                rectangleFootprintModel.yWidth *= TowerScale;
            }
        }

        foreach (var projectile in result.GetDescendants<ProjectileModel>().ToList())
        {
            projectile.radius *= TowerScale;
        }

        foreach (var behavior in result.GetDescendants<TravelStraitModel>().ToList())
        {
            behavior.lifespan *= TowerScale;
            behavior.lifespanFrames = (int)(TowerScale * behavior.lifespanFrames);
        }
        
        foreach (var behavior in result.GetDescendants<TravelCurvyModel>().ToList())
        {
            behavior.lifespan *= TowerScale;     
            behavior.lifespanFrames = (int)(TowerScale * behavior.lifespanFrames);
        }
        
        foreach (var behavior in result.GetDescendants<TravelAlongPathModel>().ToList())
        {
            var lifespan = behavior.lifespanFrames * TowerScale;
            behavior.lifespanFrames = (int)lifespan;
            behavior.range *= TowerScale;
        }
        
    }
    

    [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0), nameof(Factory.__c__DisplayClass21_0._CreateAsync_b__0))]
    [HarmonyPrefix]
    private static void Factory_CreateAsync(Factory.__c__DisplayClass21_0 __instance, ref UnityDisplayNode prototype)
    {
        if(!_isMap || !ShouldScale || !prototype) return;
        
        if(!prototype.gameObject.HasComponent<Scaler>())
            prototype.gameObject.AddComponent<Scaler>();
    }
}
