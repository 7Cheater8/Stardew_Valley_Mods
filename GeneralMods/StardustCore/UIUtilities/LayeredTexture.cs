﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities
{
    public class LayeredTexture
    {
        public List<Texture2DExtended> textureLayers;

        public LayeredTexture(List<Texture2DExtended> textures)
        {
            this.textureLayers = textures;
        }

        /// <summary>
        /// Adds a new texture as the top layer.
        /// </summary>
        /// <param name="texture"></param>
        public void addTexture(Texture2DExtended texture)
        {
            this.textureLayers.Add(texture);
        }

        /// <summary>
        /// Adds a new texture at a specific layer depth.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="index"></param>
        public void addTexture(Texture2DExtended texture, int index)
        {
            this.textureLayers.Insert(index, texture);
        }

        public LayeredTexture Copy()
        {
            return new LayeredTexture(this.textureLayers);
        }

    }
}
