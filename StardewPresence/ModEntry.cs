using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

using Discord;

namespace StardewPresence
{
    public class ModEntry : Mod
    {

        #region Variables and Properties

        public static ModEntry Instance { get; private set; }

        private long discordClientId = 610210810723303465;
        private uint steamId = 413150;

        private Discord.Discord discord;
        private ActivityManager activityManager;

        #endregion

        #region Entry point

        public override void Entry(IModHelper helper)
        {
            if (Instance == null) Instance = this;
            else return;

            try { discord = new Discord.Discord(discordClientId, (ulong)CreateFlags.NoRequireDiscord); }
            catch (Discord.ResultException ex)
            {
                Monitor.Log($"Failed to init Discord SDK: {ex.Message}", StardewModdingAPI.LogLevel.Error);
                Monitor.Log("Please try restarting the game. If this keeps happening, please update StardewPresence.", StardewModdingAPI.LogLevel.Error);
            }

            activityManager = discord.GetActivityManager();
            activityManager.RegisterSteam(steamId);

            SPFunctions.StartTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            RegisterEvents(helper);
        }

        #endregion

        #region Functions

        private void RegisterEvents(IModHelper helper)
        {
            // Register Discord events
            activityManager.OnActivityJoin += (string secret) =>
            {
                Monitor.Log("Attempting to join game");
                SPFunctions.JoinGame(secret);
            };

            activityManager.OnActivityJoinRequest += (ref User user) =>
            {
                string discordUser = $"{user.Username}#{user.Discriminator}";
                Monitor.Log($"{discordUser} is requesting to join your game. Respond in the Discord overlay!", StardewModdingAPI.LogLevel.Alert);
                Game1.chatBox.addInfoMessage($"{discordUser} is requesting to join your game. Respond in the Discord overlay!");
            };

            // Register game events
            helper.Events.GameLoop.UpdateTicked += UpdateEvent;
            helper.Events.GameLoop.SaveLoaded += SaveLoadedEvent;
        }

        

        #endregion

        #region Events

        private void UpdateEvent(object sender, UpdateTickedEventArgs e)
        {
            try
            {
                discord.RunCallbacks();
                if (e.IsMultipleOf(30))
                {
                    activityManager.UpdateActivity(SPFunctions.GetActivity(), (result) =>
                    {
                        if (result != Result.Ok) { Monitor.Log($"Discord update activity: {result.ToString()}"); }
                    });
                }
            }
            catch (Exception ex) { /*Monitor.Log($"Exception caught: {ex.Message}", StardewModdingAPI.LogLevel.Error);*/ }
        }

        private void SaveLoadedEvent(object sender, SaveLoadedEventArgs e)
        {
            
        }

        #endregion
    }
}
