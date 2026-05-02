using PvPAdventure.Core.Utilities;
using System.Collections.Generic;
using Terraria.ID;

namespace PvPAdventure.Common.MainMenu.Shop;

public static class ProductCatalog
{
    private static readonly Dictionary<ProductKey, ShopProduct> ByKey = new()
    {
        [new("breaker_blade", "brutalist")] = new(
            Prototype: "breaker_blade",
            Name: "brutalist",
            DisplayName: "Brutalist Breaker Blade",
            Texture: Ass.BrutalistBreakerBlade,
            ItemType: ItemID.BreakerBlade),

        [new("chain_guillotines", "defiled")] = new(
            Prototype: "chain_guillotines",
            Name: "defiled",
            DisplayName: "Defiled Chain Guillotines",
            Texture: Ass.DefiledChainGuillotines,
            ItemType: ItemID.ChainGuillotines),

        [new("cursed_flames", "grotesque")] = new(
            Prototype: "cursed_flames",
            Name: "grotesque",
            DisplayName: "Grotesque Cursed Flames",
            Texture: Ass.GrotesqueCursedFlames,
            ItemType: ItemID.CursedFlames),

        [new("excalibur", "trophy")] = new(
            Prototype: "excalibur",
            Name: "trophy",
            DisplayName: "Trophy Excalibur",
            Texture: Ass.TrophyExcalibur,
            ItemType: ItemID.Excalibur),

        [new("heat_ray", "toxic")] = new(
            Prototype: "heat_ray",
            Name: "toxic",
            DisplayName: "Toxic Heatray",
            Texture: Ass.ToxicHeatray,
            ItemType: ItemID.HeatRay),

        [new("influx_waver", "cyberblade")] = new(
            Prototype: "influx_waver",
            Name: "cyberblade",
            DisplayName: "Influx Waver Cyberblade",
            Texture: Ass.InfluxWaverCyberblade,
            ItemType: ItemID.InfluxWaver),

        [new("light_disc", "light_disc")] = new(
            Prototype: "light_disc",
            Name: "light_disc",
            DisplayName: "Light Disc",
            Texture: Ass.LightDisc,
            ItemType: ItemID.LightDisc),

        [new("meteor_staff", "infernal")] = new(
            Prototype: "meteor_staff",
            Name: "infernal",
            DisplayName: "Infernal Meteor Staff",
            Texture: Ass.InfernalMeteorStaff,
            ItemType: ItemID.MeteorStaff),

        [new("morning_star", "hologram")] = new(
            Prototype: "morning_star",
            Name: "hologram",
            DisplayName: "Hologram Morning Star",
            Texture: Ass.HologramMorningStar,
            ItemType: ItemID.MaceWhip),

        [new("paladins_hammer", "tesla")] = new(
            Prototype: "paladins_hammer",
            Name: "tesla",
            DisplayName: "Tesla Paladin's Hammer",
            Texture: Ass.TeslaPaladinsHammer,
            ItemType: ItemID.PaladinsHammer),

        [new("psycho_knife", "dark_matter")] = new(
            Prototype: "psycho_knife",
            Name: "dark_matter",
            DisplayName: "Dark Matter Psycho Knife",
            Texture: Ass.DarkMatterPsychoKnife,
            ItemType: ItemID.PsychoKnife),

        [new("scourge_of_the_corruptor", "crimera")] = new(
            Prototype: "scourge_of_the_corruptor",
            Name: "crimera",
            DisplayName: "Crimera Scourge of the Corruptor",
            Texture: Ass.CrimeraScourgeOfTheCorruptor,
            ItemType: ItemID.ScourgeoftheCorruptor),

        [new("sniper_rifle", "blue")] = new(
            Prototype: "sniper_rifle",
            Name: "blue",
            DisplayName: "Blue Sniper Rifle",
            Texture: Ass.SniperRifleBlue,
            ItemType: ItemID.SniperRifle),

        [new("sniper_rifle", "green")] = new(
            Prototype: "sniper_rifle",
            Name: "green",
            DisplayName: "Green Sniper Rifle",
            Texture: Ass.SniperRifleGreen,
            ItemType: ItemID.SniperRifle),

        [new("sniper_rifle", "pink")] = new(
            Prototype: "sniper_rifle",
            Name: "pink",
            DisplayName: "Pink Sniper Rifle",
            Texture: Ass.SniperRiflePink,
            ItemType: ItemID.SniperRifle),

        [new("sniper_rifle", "red")] = new(
            Prototype: "sniper_rifle",
            Name: "red",
            DisplayName: "Red Sniper Rifle",
            Texture: Ass.SniperRifleRed,
            ItemType: ItemID.SniperRifle),

        [new("sniper_rifle", "yellow")] = new(
            Prototype: "sniper_rifle",
            Name: "yellow",
            DisplayName: "Yellow Sniper Rifle",
            Texture: Ass.SniperRifleYellow,
            ItemType: ItemID.SniperRifle),

        [new("staff_of_earth", "avalanche_staff")] = new(
            Prototype: "staff_of_earth",
            Name: "avalanche_staff",
            DisplayName: "Staff of Earth Avalanche Staff",
            Texture: Ass.StaffOfEarthAvalancheStaff,
            ItemType: ItemID.StaffofEarth),

        [new("starfury", "fallen")] = new(
            Prototype: "starfury",
            Name: "fallen",
            DisplayName: "Fallen Starfury",
            Texture: Ass.FallenStarfury,
            ItemType: ItemID.Starfury),

        [new("tsunami", "sculk")] = new(
            Prototype: "tsunami",
            Name: "sculk",
            DisplayName: "Sculk Tsunami",
            Texture: Ass.SculkTsunami,
            ItemType: ItemID.Tsunami),

        [new("volcano", "molten")] = new(
            Prototype: "volcano",
            Name: "molten",
            DisplayName: "Molten Volcano",
            Texture: Ass.VolcanoMolten,
            ItemType: ItemID.FieryGreatsword),

        [new("xenopopper", "watergun")] = new(
            Prototype: "xenopopper",
            Name: "watergun",
            DisplayName: "Watergun Xenopopper",
            Texture: Ass.WatergunXenopopper,
            ItemType: ItemID.Xenopopper),
    };

    public static IEnumerable<ShopProduct> All => ByKey.Values;

    public static bool TryGet(ProductKey key, out ShopProduct definition)
    {
        return ByKey.TryGetValue(key, out definition);
    }

    public static bool TryGet(string prototype, string name, out ShopProduct definition)
    {
        return TryGet(new ProductKey(prototype, name), out definition);
    }
}