using Discord;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Constants = StardewModdingAPI.Constants;
using LogLevel = StardewModdingAPI.LogLevel;
using Utility = StardewValley.Utility;

namespace StardewPresence
{
    public static class SPFunctions
    {

        public static long StartTime { get; set; }

        public static void JoinGame(string inviteCode)
        {
            object lobby = Program.sdk.Networking.GetLobbyFromInviteCode(inviteCode);
            if (lobby == null) return;
            Game1.ExitToTitle(() => { TitleMenu.subMenu = new FarmhandMenu(Program.sdk.Networking.CreateClient(lobby)); });
        }

        public static string GetLocationText()
        {
            if (!Context.IsWorldReady) return "On the main menu";
            switch (Game1.player.currentLocation.Name)
            {
                case "FarmHouse": return "Relaxing in their house";
                case "Farm": return "At their farm";
                case "FarmCave": return "In the cave";
                case "Town": return "Visiting the town";
                case "JoshHouse": return "At Josh's house";
                case "HaleyHouse": return "At Haley's house";
                case "SamHouse": return "At Sam's house";
                case "Blacksmith": return "Processing some geodes";
                case "ManorHouse": return "At Mayor Lewis' house";
                case "SeedShop": return "Shopping at Pierre's";
                case "Saloon": return "Having a drink with the townsfolk";
                case "Trailer": return "Checking in with Penny";
                case "Hospital": return "Getting a check-up";
                case "HarveyRoom": return "Hanging with Harvey";
                case "Beach": return "Relaxing at the beach";
                case "ElliottHouse": return "At Elliott's house";
                case "Mountain": return "Climbing the mountain";
                case "ScienceHouse": return "Learning some science";
                case "SebastianRoom": return "Hanging out with Sebastian";
                case "Tent": return "Going camping with Linus";
                case "Forest": return "Exploring the forest";
                case "WizardHouse": return "In the Wizard's tower";
                case "AnimalShop": return "At Marnie's ranch";
                case "LeahHouse": return "At Leah's house";
                case "BusStop": return "Waiting for the bus";
                case "Mine": return "Spelunking in the mine";
                case "Sewer": return "Exploring the sewer";
                case "BugLand": return "In the land of the bugs";
                case "Desert": return "Exploring the desert";
                case "Club": return "Partying in the club";
                case "SandyHouse": return "At Sandy's shop";
                case "ArchaeologyHouse": return "Donating to the museum";
                case "WizardHouseBasement": return "In the Wizard's basement";
                case "AdventureGuild": return "In the adventurer's guild";
                case "Woods": return "Exploring the secret woods";
                case "Railroad": return "At the railroad";
                case "WitchSwamp": return "In the Witch's swamp";
                case "WitchHut": return "In the Witch's hut";
                case "WitchWarpCave": return "In the Witch's cave";
                case "Summit": return "At the summit";
                case "FishShop": return "Chatting with Willy";
                case "BathHouse_Entry": return "Visiting the BathHouse";
                case "BathHouse_MensLocker": case "BathHouse_WomensLocker": return "Getting changed in the locker room";
                case "BathHouse_Pool": return "Relaxing in the BathHouse pool";
                case "CommunityCenter": return "Rebuilding the Community Center";
                case "JojaMart": return "Shopping at JojaMart";
                case "Greenhouse": return "Gardening in the Greenhouse";
                case "SkullCave": return "Exploring the skull cave";
                case "Backwoods": return "Exploring the woods";
                case "Tunnel": return "Exploring the tunnel";
                case "Trailer_Big": return "In the trailer";
                case "Cellar": return "In the cellar";
                case "BeachNightMarket": return "Exploring the night market";
                case "MermaidHouse": return "Watching the mermaids";
                case "Submarine": return "In a submarine";
                default: return "Exploring";
            }
        }

        public static string GetGameType()
        { return $"Playing " + (Game1.server.connected() ? "Online" : "Solo"); }

        public static int GetCurrentPlayerCount()
        {
            if (!Context.IsWorldReady) return 0;
            return Game1.numberOfPlayers();
        }

        public static int GetMaxPlayerCount()
        {
            if (!Context.IsWorldReady) return 0;
            return Game1.getFarm().getNumberBuildingsConstructed("Cabin") + 1;
        }

        public static string GetMPInviteCode()
        {
            if (!Context.IsWorldReady) return "";
            return Game1.server.getInviteCode();
        }

        public static Activity GetActivity()
        {
            Activity act = new Activity()
            {
                Details = GetLocationText(),
                State = GetGameType(),
                Assets =
                {
                    LargeImage = "logo"
                },
                Timestamps =
                {
                    Start = StartTime
                },
                Party =
                {
                    Id = Game1.MasterPlayer.UniqueMultiplayerID.ToString(),
                    Size =
                    {
                        CurrentSize = GetCurrentPlayerCount(),
                        MaxSize = GetMaxPlayerCount()
                    }
                },
                Secrets =
                {
                    Join = GetMPInviteCode()
                }
            };

            return act;
        }
    }
}
