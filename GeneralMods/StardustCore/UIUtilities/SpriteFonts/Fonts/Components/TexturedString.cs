﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustCore.UIUtilities.SpriteFonts.Components
{
    public class TexturedString
    {
        List<TexturedCharacter> characters;
        Vector2 position;

        public TexturedString(Vector2 Position,List<TexturedCharacter> Characters)
        {
            this.characters = Characters;
            this.position = Position;
            setCharacterPositions();
        }

        /// <summary>
        /// Sets the character positions relative to the string's position on screen.
        /// </summary>
        public void setCharacterPositions()
        {
            int index = 0;
            TexturedCharacter lastSeenChar=new TexturedCharacter();
            foreach(var c in characters)
            {
                if (index == 0)
                {
                    c.position = new Vector2(this.position.X + c.spacing.LeftPadding,this.position.Y);
                }
                else
                {
                    c.position = new Vector2(this.position.X + c.spacing.LeftPadding + lastSeenChar.spacing.RightPadding+lastSeenChar.texture.Width*index, this.position.Y);    
                }
                //StardustCore.ModCore.ModMonitor.Log(c.character.ToString());
                //StardustCore.ModCore.ModMonitor.Log(c.position.ToString());
                lastSeenChar = c;
                index++;
            }
        }

        /// <summary>
        /// Adds a textured character to a textured string.
        /// </summary>
        /// <param name="ch"></param>
        public void addCharacterToEnd(TexturedCharacter ch)
        {
            this.characters.Add(ch);
            this.setCharacterPositions();
        }

        /// <summary>
        /// Adds a list of textured characters to a textured string.
        /// </summary>
        /// <param name="chList"></param>
        public void addCharactersToEnd(List<TexturedCharacter> chList)
        {
            foreach(var ch in chList)
            {
                this.characters.Add(ch);
            }
            this.setCharacterPositions();
        }

        /// <summary>
        /// Adds the strings together and allows the position to be set.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="NewPosition"></param>
        /// <returns></returns>
        public TexturedString addStrings(TexturedString first,TexturedString second,Vector2 NewPosition)
        {
            var newString = first + second;
            newString.position = NewPosition;
            newString.setCharacterPositions();
            return newString;
        }

        /// <summary>
        /// Operator overload of +. Adds the two strings together and sets a new 0,0 position.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static TexturedString operator+(TexturedString first, TexturedString second)
        {
            List<TexturedCharacter> characterList = new List<TexturedCharacter>();
            foreach(var v in first.characters)
            {
                characterList.Add(v);
            }
            foreach (var v in second.characters)
            {
                characterList.Add(v);
            }
            TexturedString newString = new TexturedString(new Vector2(0, 0), characterList);
            return newString;
        }


        /// <summary>
        /// Removes the characters from the textured word.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="howMany"></param>
        public void removeCharactersFromEnd(int index,int howMany)
        {
            this.characters.RemoveRange(index, howMany);
        }

        /// <summary>
        /// Draw the textured string.
        /// </summary>
        /// <param name="b"></param>
        public void draw(SpriteBatch b)
        {
            foreach(var v in this.characters)
            {
                v.draw(b);
            }
        }
    }
}
