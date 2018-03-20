﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardustCore.UIUtilities.MenuComponents.Delegates;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardustCore.UIUtilities.MenuComponents.Delegates.Delegates;

namespace StardustCore.UIUtilities.MenuComponents
{
    public class Button : StardewValley.Menus.ClickableTextureComponent
    {
        public Animations.AnimationManager animationManager;
        public Color textureColor;
        public Color textColor;

        public ButtonFunctionality buttonFunctionality;

        /// <summary>
        /// Basic Button constructor.
        /// </summary>
        /// <param name="Bounds"></param>
        /// <param name="Texture">The texture sheet to be drawn to the screen. Used with the animation manager this allows you to reference different parts of the sheet at any given time.</param>
        /// <param name="sourceRect"></param>
        /// <param name="Scale"></param>
        /// <param name="defaultAnimation"></param>
        /// <param name="AnimationEnabled"></param>
        public Button(string Name,Rectangle Bounds,Texture2DExtended Texture,string displayText,Rectangle sourceRect,float Scale,Animations.Animation defaultAnimation, Color DrawColor,Color TextColor, ButtonFunctionality Functionality, bool AnimationEnabled=true) : base(Bounds,Texture.texture,sourceRect,Scale)
        {
            this.animationManager = new Animations.AnimationManager(Texture, defaultAnimation,AnimationEnabled);
            this.label = displayText;
            this.name = Name;
            this.textureColor = DrawColor;
            if (this.textureColor == null)
            {
                this.textureColor = StardustCore.IlluminateFramework.Colors.getColorFromList("White");
            }
            this.textColor = DrawColor;
            if (this.textColor == null)
            {
                this.textColor = StardustCore.IlluminateFramework.Colors.getColorFromList("White");
            }
            this.buttonFunctionality = Functionality;
        }

        /// <summary>
        /// A more advanced Button constructor that deals with an animation manager.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Bounds"></param>
        /// <param name="Texture"></param>
        /// <param name="displayText"></param>
        /// <param name="sourceRect"></param>
        /// <param name="Scale"></param>
        /// <param name="defaultAnimation"></param>
        /// <param name="animationsToPlay"></param>
        /// <param name="startingAnimationKey"></param>
        /// <param name="startingAnimationFrame"></param>
        /// <param name="AnimationEnabled"></param>
        public Button(string Name,Rectangle Bounds,Texture2DExtended Texture, string displayText, Rectangle sourceRect,float Scale, Animations.Animation defaultAnimation,Dictionary<string, List<Animations.Animation>> animationsToPlay,string startingAnimationKey,Color DrawColor,Color TextColor, ButtonFunctionality Functionality,int startingAnimationFrame=0,bool AnimationEnabled=true) : base(Bounds, Texture.texture, sourceRect, Scale)
        {
            this.animationManager = new Animations.AnimationManager(Texture, defaultAnimation, animationsToPlay, startingAnimationKey, startingAnimationFrame, AnimationEnabled);
            this.label = displayText;
            this.name = Name;
            this.textureColor = DrawColor;
            if (this.textureColor == null)
            {
                this.textureColor = StardustCore.IlluminateFramework.Colors.getColorFromList("White");
            }
            this.textColor = DrawColor;
            if (this.textColor == null)
            {
                this.textColor = StardustCore.IlluminateFramework.Colors.getColorFromList("White");
            }
            this.buttonFunctionality = Functionality;
        }

        /// <summary>
        /// Draws the button and all of it's components.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="layerDepth"></param>
        public void draw(SpriteBatch b, float layerDepth)
        {
            
            this.animationManager.tickAnimation();
            if (!this.visible)
                return;
            if (this.drawShadow)
                Utility.drawWithShadow(b, this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), this.sourceRect, this.textureColor, 0.0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, false, layerDepth, -1, -1, 0.35f);
            else
                b.Draw(this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), new Rectangle?(this.sourceRect), this.textureColor, 0.0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, layerDepth);
            if (string.IsNullOrEmpty(this.label)) {

            }
            else {
                b.DrawString(Game1.smallFont, this.label, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);
            }

            if (this.hoverText=="")
            {

            }
            else
            {
                StardustCore.ModCore.ModMonitor.Log("HOVER???");
                b.DrawString(Game1.smallFont, this.hoverText, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);
            }

        }

        /// <summary>
        /// Swaps if the button is visible or not. Also toggles the animation manager appropriately.
        /// </summary>
        public void swapVisibility()
        {
            if (this.visible == true)
            {
                this.visible = false;
                this.animationManager.disableAnimation();
            }
        }
    

        public void onRightClick()
        {
            if (this.buttonFunctionality == null) return;
            else
            {
                if (this.buttonFunctionality.rightClick == null) return;
                else this.buttonFunctionality.rightClick.run();
            }
        }

        public void onLeftClick()
        {
            if (this.buttonFunctionality == null) return;
            else
            {
                if (this.buttonFunctionality.leftClick == null) return;
                else this.buttonFunctionality.leftClick.run();
            }
        }

        public void onHover()
        {
            if (this.buttonFunctionality == null) return;
            else
            {
                if (this.buttonFunctionality.hover == null) return;
                else this.buttonFunctionality.hover.run();
            }
        }
    }
}
