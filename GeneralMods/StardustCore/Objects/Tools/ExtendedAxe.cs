﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardustCore.Interfaces;
using StardustCore.Objects.Tools.SerializationInformation;
using StardustCore.UIUtilities;

namespace StardustCore.Objects.Tools
{
    public class ExtendedAxe : StardewValley.Tools.Axe, IItemSerializeable, IToolSerializer
    {
        public Texture2DExtended texture;

        public override string DisplayName { get => this.displayName; set => this.displayName = value; }
        public override string Name { get => this.displayName; set => this.displayName = value; }

        /// <summary>
        /// Generates a default axe. Doens't really do much.
        /// </summary>
        public ExtendedAxe() : base()
        {
            this.texture = new Texture2DExtended(StardustCore.ModCore.ModHelper, Path.Combine("Content","Graphics","Tools","CustomAxe.png"));
        }

        public ExtendedAxe(IModHelper helper,String texturePath) : base()
        {
            this.texture = new Texture2DExtended(helper, texturePath);
        }

        public ExtendedAxe(Texture2DExtended texture) :base() 
        {
            this.texture = texture;   
        }

        public ExtendedAxe(SerializedObjectBase dataBase) : base()
        {
            StardustCore.ModCore.ModMonitor.Log("WTF EVEN " + dataBase.GetType().ToString());
            StardustCore.ModCore.ModMonitor.Log((dataBase as Serialization_ExtendedAxe).Name);
            this.displayName = "Hello";
            this.description = (dataBase as Serialization_ExtendedAxe).Description;
            this.texture = new Texture2DExtended(StardustCore.ModCore.ModHelper, Path.Combine("Content", "Graphics", "Tools", "CustomAxe.png"));
            this.UpgradeLevel = (dataBase as Serialization_ExtendedAxe).UpgradeLevel;
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber, Color color, bool drawShadow)
        {
            spriteBatch.Draw(texture.getTexture(), location + new Vector2(32f, 32f), new Rectangle(0, 0, 16 , 16), color * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
        }

        public Type getCutsomType()
        {
            return this.GetType();
        }

        public string GetSerializationName()
        {
            return this.GetType().ToString();
        }

        public override int maximumStackSize()
        {
            return 1;
        }

        public override void setNewTileIndexForUpgradeLevel()
        {
            //Do nothing.
        }

        /// <summary>
        /// Serializes the said item properly.
        /// </summary>
        /// <param name="I"></param>
        public static void Serialize(Item I)
        {
            SerializationInformation.Serialization_ExtendedAxe sAxe = new SerializationInformation.Serialization_ExtendedAxe((I as ExtendedAxe));
            String savePath = ModCore.SerializationManager.playerInventoryPath;
            String fileName = I.Name + ".json";
            String resultPath = Path.Combine(savePath, fileName);
            int count = 0;
            while (File.Exists(resultPath))
            {
                resultPath=Serialization.SerializationManager.getValidSavePathIfDuplicatesExist(I, savePath,count);
                count++;
            }
            StardustCore.ModCore.ModHelper.WriteJsonFile<SerializedObjectBase>(resultPath, sAxe);
        }

        /// <summary>
        /// Serializes the said item to a chest.
        /// </summary>
        /// <param name="I"></param>
        /// <param name="s"></param>
        public static void SerializeToContainer(Item I, string s)
        {
            SerializationInformation.Serialization_ExtendedAxe sAxe = new SerializationInformation.Serialization_ExtendedAxe((I as ExtendedAxe));
            String savePath = s;
            String fileName = I.Name + ".json";
            String resultPath = Path.Combine(savePath, fileName);
            int count = 0;
            while (File.Exists(resultPath))
            {
                resultPath = Serialization.SerializationManager.getValidSavePathIfDuplicatesExist(I, savePath, count);
                count++;
            }
            StardustCore.ModCore.ModHelper.WriteJsonFile<SerializedObjectBase>(resultPath, sAxe);
        }

        /// <summary>
        /// Deserializes the object from a .json.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ExtendedAxe Deserialize(string data)
        {  
            Serialization_ExtendedAxe axeData= ModCore.ModHelper.ReadJsonFile<Serialization_ExtendedAxe>(data);
            return new ExtendedAxe(axeData);
        }


    }
}
