using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Revitalize.Framework.Objects.InformationFiles;
using Revitalize.Framework.Objects.Resources.OreVeins;
using Revitalize.Framework.Utilities;
using StardewValley;
using StardustCore.Animations;
using StardustCore.UIUtilities;

namespace Revitalize.Framework.Objects
{
    public class ResourceManager
    {
        /// <summary>
        /// A static reference to the resource manager for quicker access.
        /// </summary>
        public static ResourceManager self;

        /// <summary>
        /// A list of all of the ores held by the resource manager.
        /// </summary>
        public Dictionary<string, OreVeinObj> ores;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceManager()
        {
            self = this;
            this.ores = new Dictionary<string, OreVeinObj>();
            this.loadOreVeins();
        }

        /// <summary>
        /// Loads in all of the ore veins for the game.
        /// </summary>
        private void loadOreVeins()
        {
            //The pancake ore.


            OreVeinObj testOre = new OreVeinObj(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null));
            testOre.addComponent(new Vector2(0, 0), new OreVeinTile(PyTKHelper.CreateOBJData("Omegasis.Revitalize.Resources.Ore.Test", TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), typeof(OreVeinTile), Color.White), new BasicItemInformation("Test Ore Vein", "Omegasis.Revitalize.Resources.Ore.Test", "A ore vein that is used for testing purposes.", "Revitalize.Ore", Color.Black, -300, 0, false, 350, Vector2.Zero, true, true, TextureManager.GetTexture(ModCore.Manifest, "Resources.Ore", "Test"), new AnimationManager(TextureManager.GetExtendedTexture(ModCore.Manifest, "Resources.Ore", "Test"), new Animation(0, 0, 16, 16)), Color.White, false, null, null), new InformationFiles.OreResourceInformation(new StardewValley.Object(211, 1),false,false,true,false,new List<IntRange>()
            {
                new IntRange(1,9)
            },new List<IntRange>(),1,5,1,10,1d,1d,0,0,0,0),new List<ResourceInformaton>()));
            this.ores.Add("Omegasis.Revitalize.Resources.Ore.Test", testOre);
        }

        /// <summary>
        /// Spawns an ore vein at the given location if possible.
        /// </summary>
        /// <param name="name"></param>
        public bool spawnOreVein(string name,GameLocation Location, Vector2 TilePosition)
        {
            if (this.ores.ContainsKey(name))
            {
                OreVeinObj spawn;
                this.ores.TryGetValue(name, out spawn);
                if (spawn != null)
                {
                    spawn = (OreVeinObj)spawn.getOne();
                    bool spawnable = this.canResourceBeSpawnedHere(spawn, Location, TilePosition);
                    if (spawnable)
                    {
                        //ModCore.log("Location is: " + Location.Name);
                        spawn.placementAction(Location, (int)TilePosition.X*Game1.tileSize, (int)TilePosition.Y*Game1.tileSize,Game1.player);
                    }
                    else
                    {
                        ModCore.log("Can't spawn ore: " + name + "at tile location: " + TilePosition);
                    }
                    return spawnable;
                }
                ModCore.log("Key doesn't exist. Weird.");
                return false;
            }
            else
            {
                throw new Exception("The ore dictionary doesn't contain they key for resource: " + name);
            }
        }
        /// <summary>
        /// Spawns an orevein at the tile position at the same location as the player.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool spawnOreVein(string name, Vector2 TilePosition)
        {
            return this.spawnOreVein(name, Game1.player.currentLocation, TilePosition);
        }

        /// <summary>
        /// Checks to see if a resource can be spawned here.
        /// </summary>
        /// <param name="OBJ"></param>
        /// <param name="Location"></param>
        /// <param name="TilePosition"></param>
        /// <returns></returns>
        public bool canResourceBeSpawnedHere(MultiTiledObject OBJ,GameLocation Location, Vector2 TilePosition)
        {
            return OBJ.canBePlacedHere(Location, TilePosition) && Location.isTileLocationTotallyClearAndPlaceable(TilePosition);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~//
        //  Mine ore spawn code  //
        //~~~~~~~~~~~~~~~~~~~~~~~//

        #region
        public void spawnOreInMine()
        {
            int floorLevel = LocationUtilities.CurrentMineLevel();

            List<OreVeinObj> spawnableOreVeins = new List<OreVeinObj>();
            //Get a list of all of the ores that can spawn on this mine level.
            foreach(KeyValuePair<string,OreVeinObj> pair in this.ores)
            {
                if (pair.Value.resourceInfo.canSpawnAtLocation() && (pair.Value.resourceInfo as OreResourceInformation).canSpawnOnCurrentMineLevel())
                {
                    spawnableOreVeins.Add(pair.Value);
                }
            }

            foreach(OreVeinObj ore in spawnableOreVeins)
            {
                int amount = ore.resourceInfo.getNumberOfNodesToSpawn();
                List<Vector2> openTiles = LocationUtilities.GetOpenObjectTiles(Game1.player.currentLocation, (OreVeinObj)ore.getOne());
                for (int i = 0; i < amount; i++)
                {
                    int position = Game1.random.Next(openTiles.Count);
                    this.spawnOreVein(ore.info.id, openTiles[position]);
                }
                ModCore.log("Spawned :" + amount + " pancake test ores!");
            }

            /*
            if(floorLevel>=1 && floorLevel <= 9)
            {
                int amount = Game1.random.Next(1, 10); //Change this to be a frequency table or something.
                List<Vector2> openTiles = LocationUtilities.GetOpenObjectTiles(Game1.player.currentLocation, (OreVeinObj)this.ores["Omegasis.Revitalize.Resources.Ore.Test"].getOne());

                for(int i = 0; i <= amount; i++)
                {
                    int position = Game1.random.Next(openTiles.Count);
                    this.spawnOreVein("Omegasis.Revitalize.Resources.Ore.Test", openTiles[position]);
                }
               
            }
            */
        }

        public void OnPlayerLocationChanged(object o,EventArgs playerWarped)
        {
            this.spawnOreInMine();
        }

        #endregion
    }
}