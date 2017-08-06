﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Omegasis.SaveAnywhere.Framework.Models;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;

namespace Omegasis.SaveAnywhere.Framework
{
    /// <summary>Provides methods for saving and loading game data.</summary>
    internal class SaveManager
    {
        /*********
        ** Properties
        *********/
        /// <summary>Simplifies access to game code.</summary>
        private readonly IReflectionHelper Reflection;

        /// <summary>A callback invoked when data is loaded.</summary>
        private readonly Action OnLoaded;

        /// <summary>SMAPI's APIs for this mod.</summary>
        private readonly IModHelper Helper;

        /// <summary>The full path to the player data file.</summary>
        private string SavePath => Path.Combine(this.Helper.DirectoryPath, "data", $"{Constants.SaveFolderName}.json");

        /// <summary>Whether we should save at the next opportunity.</summary>
        private bool WaitingToSave;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="helper">SMAPI's APIs for this mod.</param>
        /// <param name="reflection">Simplifies access to game code.</param>
        /// <param name="onLoaded">A callback invoked when data is loaded.</param>
        public SaveManager(IModHelper helper, IReflectionHelper reflection, Action onLoaded)
        {
            this.Helper = helper;
            this.Reflection = reflection;
            this.OnLoaded = onLoaded;
        }

        /// <summary>Perform any required update logic.</summary>
        public void Update()
        {
            // perform passive save
            if (this.WaitingToSave && Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new NewSaveGameMenu();
                this.WaitingToSave = false;
            }
        }

        /// <summary>Clear saved data.</summary>
        public void ClearData()
        {
            Directory.Delete(this.SavePath, recursive: true);
            this.RemoveLegacyDataForThisPlayer();
        }

        /// <summary>Initiate a game save.</summary>
        public void BeginSaveData()
        {
            // save game data
            Farm farm = Game1.getFarm();
            if (farm.shippingBin.Any())
            {
                Game1.activeClickableMenu = new NewShippingMenu(farm.shippingBin, this.Reflection);
                farm.shippingBin.Clear();
                farm.lastItemShipped = null;
                this.WaitingToSave = true;
            }
            else
                Game1.activeClickableMenu = new NewSaveGameMenu();

            // get data
            PlayerData data = new PlayerData
            {
                Time = Game1.timeOfDay,
                Characters = this.GetPositions().ToArray()
            };

            // save to disk
            // ReSharper disable once PossibleNullReferenceException -- not applicable
            Directory.CreateDirectory(new FileInfo(this.SavePath).Directory.FullName);
            this.Helper.WriteJsonFile(this.SavePath, data);

            // clear any legacy data (no longer needed as backup)
            this.RemoveLegacyDataForThisPlayer();
        }

        /// <summary>Load all game data.</summary>
        public void LoadData()
        {
            // get data
            PlayerData data = this.Helper.ReadJsonFile<PlayerData>(this.SavePath);
            if (data == null)
                return;

            // apply
            Game1.timeOfDay = data.Time;
            this.SetPositions(data.Characters);
            this.OnLoaded?.Invoke();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the current character positions.</summary>
        private IEnumerable<CharacterData> GetPositions()
        {
            // player
            {
                var player = Game1.player;
                string name = player.name;
                string map = player.currentLocation.name;
                Point tile = player.getTileLocationPoint();
                int facingDirection = player.facingDirection;

                yield return new CharacterData(CharacterType.Player, name, map, tile, facingDirection);
            }

            // NPCs (including horse and pets)
            foreach (NPC npc in Utility.getAllCharacters())
            {
                CharacterType? type = this.GetCharacterType(npc);
                if (type == null)
                    continue;

                string name = npc.name;
                string map = npc.currentLocation.name;
                Point tile = npc.getTileLocationPoint();
                int facingDirection = npc.facingDirection;

                yield return new CharacterData(type.Value, name, map, tile, facingDirection);
            }
        }

        /// <summary>Reset characters to their saved state.</summary>
        /// <param name="positions">The positions to set.</param>
        /// <returns>Returns whether any NPCs changed position.</returns>
        private void SetPositions(CharacterData[] positions)
        {
            // player
            {
                CharacterData data = positions.FirstOrDefault(p => p.Type == CharacterType.Player && p.Name == Game1.player.name);
                if (data != null)
                {
                    Game1.player.previousLocationName = Game1.player.currentLocation.name;
                    Game1.locationAfterWarp = Game1.getLocationFromName(data.Name);
                    Game1.xLocationAfterWarp = data.X;
                    Game1.yLocationAfterWarp = data.Y;
                    Game1.facingDirectionAfterWarp = data.FacingDirection;
                    Game1.fadeScreenToBlack();
                    Game1.warpFarmer(data.Map, data.X, data.Y, false);
                    Game1.player.faceDirection(data.FacingDirection);
                }
            }

            // NPCs (including horse and pets)
            foreach (NPC npc in Utility.getAllCharacters())
            {
                // get NPC type
                CharacterType? type = this.GetCharacterType(npc);
                if (type == null)
                    continue;

                // get saved data
                CharacterData data = positions.FirstOrDefault(p => p.Type == type && p.Name == npc.name);
                if (data == null)
                    continue;

                // update NPC
                Game1.warpCharacter(npc, data.Map, new Point(data.X, data.Y), false, true);
                npc.faceDirection(data.FacingDirection);
            }
        }

        /// <summary>Get the character type for an NPC.</summary>
        /// <param name="npc">The NPC to check.</param>
        private CharacterType? GetCharacterType(NPC npc)
        {
            if (npc is Monster)
                return null;
            if (npc is Horse)
                return CharacterType.Horse;
            if (npc is Pet)
                return CharacterType.Pet;
            return CharacterType.Villager;
        }

        /// <summary>Remove legacy save data for this player.</summary>
        private void RemoveLegacyDataForThisPlayer()
        {
            DirectoryInfo dataDir = new DirectoryInfo(Path.Combine(this.Helper.DirectoryPath, "Save_Data"));
            DirectoryInfo playerDir = new DirectoryInfo(Path.Combine(dataDir.FullName, Game1.player.name));
            if (playerDir.Exists)
                playerDir.Delete(recursive: true);
            if (dataDir.Exists && !dataDir.EnumerateDirectories().Any())
                dataDir.Delete(recursive: true);
        }
    }
}
