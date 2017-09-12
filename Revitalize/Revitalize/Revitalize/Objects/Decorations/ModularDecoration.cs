﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Revitalize.Resources;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Revitalize.Objects
{
    /// <summary>
    /// Original Stardew Furniture Class but rewritten to be placed anywhere.
    /// </summary>
    public class ModularDecoration : CoreObject
    {
        public const int chair = 0;

        public const int bench = 1;

        public const int couch = 2;

        public const int armchair = 3;

        public const int dresser = 4;

        public const int longTable = 5;

        public const int painting = 6;

        public const int lamp = 7;

        public const int decor = 8;

        public const int other = 9;

        public const int bookcase = 10;

        public const int table = 11;

        public const int rug = 12;

        public const int window = 13;

        public new int price;

        public int Decoration_type;

        public int rotations;

        public int currentRotation;

        private int sourceIndexOffset;

        public Vector2 drawPosition;

        public Rectangle sourceRect;

        public Rectangle defaultSourceRect;

        public Rectangle defaultBoundingBox;

        public string description;

        public Texture2D TextureSheet;
        public string SourceFilePathAndName;

        public new bool flipped;

        [XmlIgnore]
        public bool flaggedForPickUp;

        private bool lightGlowAdded;

        public string texturePath;


        public int xTileOffset;
        public int yTileOffset;

        public int xBoundBoxTileOffset;
        public int yBoundBoxTileOffset;

        public int BBBottom;

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public ModularDecoration()
        {
            this.updateDrawPosition();
        }

        public ModularDecoration(bool f)
        {
            //does nothng
        }

        public ModularDecoration(int which, Vector2 tile,string filePathAndName,bool isRemovable = true, int XTileOffset=0, int YTileOffset=0)
        {
            this.xTileOffset = XTileOffset;
            this.yTileOffset = YTileOffset;
            this.SourceFilePathAndName = filePathAndName;
            
            removable = isRemovable;
            // this.thisType = GetType();
            this.tileLocation = tile;
            this.InitializeBasics(0, tile);
            if (TextureSheet == null)
            {
                TextureSheet = Game1.content.Load<Texture2D>(Path.Combine("Revitalize","Furniture","Graphics",filePathAndName));
                texturePath = Path.Combine("Revitalize", "Furniture", "Graphics", filePathAndName);
            }
            Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>(Path.Combine("Revitalize", "Furniture", "Data", filePathAndName));
            string[] array = dictionary[which].Split(new char[]
            {
                '/'
            });
            this.name = array[0];
            this.description = "Can be placed inside your house.";
            this.defaultSourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, 1, 1);
            if (array[2].Equals("-1"))
            {
                //this.sourceRect = this.getDefaultSourceRectForType(which, this.Decoration_type);
                this.defaultSourceRect = this.sourceRect;
            }
            else
            {
                this.defaultSourceRect.Width = Convert.ToInt32(array[2].Split(new char[]
                {
                    ' '
                })[0]);
                this.defaultSourceRect.Height = Convert.ToInt32(array[2].Split(new char[]
                {
                    ' '
                })[1]);
                this.sourceRect = new Rectangle(which * 16 % TextureSheet.Width, which * 16 / TextureSheet.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
                this.defaultSourceRect = this.sourceRect;
            }
            this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);
            if (array[3].Equals("-1"))
            {
               // this.boundingBox = this.getDefaultBoundingBoxForType(this.Decoration_type);
                this.defaultBoundingBox = this.boundingBox;
            }
            else
            {
                this.defaultBoundingBox.Width = Convert.ToInt32(array[3].Split(new char[]
                {
                    ' '
                })[0]);
                this.defaultBoundingBox.Height = Convert.ToInt32(array[3].Split(new char[]
                {
                    ' '
                })[1]);
                xBoundBoxTileOffset = Convert.ToInt32(array[3].Split(new char[]
                {
                    ' '
                })[2]);
                yBoundBoxTileOffset = Convert.ToInt32(array[3].Split(new char[]
               {
                    ' '
               })[3]);
               int xQuarterOffset= Convert.ToInt32(array[3].Split(new char[]
             {
                    ' '
             })[4]);
                int yQuarterOffset = Convert.ToInt32(array[3].Split(new char[]
             {
                    ' '
             })[5]);
                Rectangle r = new Rectangle();
                if (xTileOffset < xBoundBoxTileOffset)
                {
                    r.X = ((int)this.tileLocation.X * Game1.tileSize) + (int)((xBoundBoxTileOffset + xTileOffset) * Game1.tileSize);
                }
                else r.X = ((int)this.tileLocation.X * Game1.tileSize);
                if (yTileOffset < yBoundBoxTileOffset)
                {
                    r.Y = ((int)this.tileLocation.Y * Game1.tileSize) + (int)(yBoundBoxTileOffset * Game1.tileSize);
                }
                else r.Y = ((int)this.tileLocation.Y * Game1.tileSize);
                r.Height = (this.defaultBoundingBox.Height * Game1.tileSize);
                r.Width = (this.defaultBoundingBox.Width * Game1.tileSize);
                if (yTileOffset < yBoundBoxTileOffset)
                {
                   // r.Y = r.Y+(yQuarterOffset*16);
                    r.Height = 0;
                    r.Width = 0;
                }
                if (xTileOffset < xBoundBoxTileOffset)
                {
                   // r.X = r.X + (xQuarterOffset*16);
                    r.Width = 0;
                    r.Height = 0;
                }
                if (r.Height == 0) Log.Async("yus Y");
                if (r.Width == 0) Log.AsyncC("yus X");
                Log.AsyncC("MEH");

                Log.AsyncG(new Vector2(xTileOffset, yTileOffset));
                Log.AsyncY(new Vector2(xBoundBoxTileOffset, yBoundBoxTileOffset));
                this.boundingBox = r;
                //else this.boundingBox = new Rectangle(((int)this.tileLocation.X * Game1.tileSize), ((int)this.tileLocation.Y * Game1.tileSize)-Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
                this.defaultBoundingBox = this.boundingBox;
            }
            this.updateDrawPosition();
            this.rotations = Convert.ToInt32(array[4]);
            this.price = Convert.ToInt32(array[5]);
            this.parentSheetIndex = which;
            
            /*
            for(int x = 0; x <= this.boundingBox.Width/Game1.tileSize; x++)
            {
                for (int y = 1; y <= this.boundingBox.Height/Game1.tileSize; y++)
                {
                    //Log.AsyncC(x);
                    //Log.AsyncM(y);
                    BlankObject ehh = new BlankObject(Vector2.Zero,x, y,false);
                    //blankSpace.Add(ehh);
                }
            }
            */
        }

        public override string getDescription()
        {
            return this.description;
        }

        public override bool performDropDownAction(StardewValley.Farmer who)
        {
            this.resetOnPlayerEntry((who == null) ? Game1.currentLocation : who.currentLocation);
            return false;
        }

        public override void hoverAction()
        {
            base.hoverAction();
            if (!Game1.player.isInventoryFull())
            {
                Game1.mouseCursor = 2;
            }
        }

        public override bool checkForAction(StardewValley.Farmer who, bool justCheckingForActivity = false)
        {
            var mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // Game1.showRedMessage("YOOO");
                //do some stuff when the right button is down
                rotate();
                Game1.playSound("coin");
                return true;
            }
            else
            {
                //Game1.showRedMessage("CRY");
            }

            if (justCheckingForActivity)
            {
                return true;
            }
            if (this.parentSheetIndex == 1402)
            {
                Game1.activeClickableMenu = new Billboard(false);
            }
            return this.clicked(who);
        }

        public override bool clicked(StardewValley.Farmer who)
        {

            if (removable == false) return false;
            //   Game1.showRedMessage("THIS IS CLICKED!!!");
            Game1.haltAfterCheck = false;
            if (this.Decoration_type == 11 && who.ActiveObject != null && who.ActiveObject != null && this.heldObject == null)
            {
                //  Game1.showRedMessage("Why1?");
                return false;
            }
            if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is ModularDecoration)))
            {
                if (Game1.player.currentLocation is FarmHouse)
                {
                    //   Game1.showRedMessage("Why2?");
                    // this.heldObject = new Decoration(parentSheetIndex, Vector2.Zero);
                    Util.addItemToInventoryAndCleanTrackedList(this);
                    this.flaggedForPickUp = true;
                    this.thisLocation = null;
                    return true;
                }
                else
                {
                    // return true;

                    this.flaggedForPickUp = true;
                    if (this is TV)
                    {
                        this.heldObject = new TV(parentSheetIndex, Vector2.Zero);
                    }
                    else
                    {
                        //  this.heldObject = new Decoration(parentSheetIndex, Vector2.Zero);
                        Util.addItemToInventoryAndCleanTrackedList(this);
                        //  this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                        //   this.heldObject = null;
                        Game1.playSound("coin");
                        this.thisLocation = null;
                        return true;
                    }

                }
            }
            if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
            {
                // Game1.showRedMessage("Why3?");
                this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
                this.heldObject = null;
                Util.addItemToInventoryAndCleanTrackedList(this);
                Game1.playSound("coin");
                this.thisLocation = null;
                return true;
            }



            return false;
        }

        public override void DayUpdate(GameLocation location)
        {
            base.DayUpdate(location);
            this.lightGlowAdded = false;
            if (!Game1.isDarkOut() || (Game1.newDay && !Game1.isRaining))
            {
                this.removeLights(location);
                return;
            }
            this.addLights(location);
        }

        public void resetOnPlayerEntry(GameLocation environment)
        {
            this.removeLights(environment);
            if (Game1.isDarkOut())
            {
                this.addLights(environment);
            }
        }

        public override bool performObjectDropInAction(StardewValley.Object dropIn, bool probe, StardewValley.Farmer who)
        {
            if ((this.Decoration_type == 11 || this.Decoration_type == 5) && this.heldObject == null && !dropIn.bigCraftable && (!(dropIn is ModularDecoration) || ((dropIn as ModularDecoration).getTilesWide() == 1 && (dropIn as ModularDecoration).getTilesHigh() == 1)))
            {
                this.heldObject = (StardewValley.Object)dropIn.getOne();
                this.heldObject.tileLocation = this.tileLocation;
                this.heldObject.boundingBox.X = this.boundingBox.X;
                this.heldObject.boundingBox.Y = this.boundingBox.Y;
                //  Log.AsyncO(getDefaultBoundingBoxForType((dropIn as Decoration).Decoration_type));
                this.heldObject.performDropDownAction(who);
                if (!probe)
                {
                    Game1.playSound("woodyStep");
                    //   Log.AsyncC("HUH?");
                    if (who != null)
                    {
                        who.reduceActiveItemByOne();
                    }
                }
                return true;
            }
            return false;
        }

        private void addLights(GameLocation environment)
        {
            // this.lightSource.lightTexture = Game1.content.Load<Texture2D>("LooseSprites\\Lighting\\lantern");

            if (this.Decoration_type == 7)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightSource == null)
                {
                    Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, Color.Black, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                    Game1.currentLightSources.Add(this.lightSource);
                    return;
                }
            }
            else if (this.Decoration_type == 13)
            {
                if (this.sourceIndexOffset == 0)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                }
                this.sourceIndexOffset = 1;
                if (this.lightGlowAdded)
                {
                    environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                    this.lightGlowAdded = false;
                }
            }
        }

        private void removeLights(GameLocation environment)
        {
            if (this.Decoration_type == 7)
            {
                if (this.sourceIndexOffset == 1)
                {
                    this.sourceRect = this.defaultSourceRect;
                }
                this.sourceIndexOffset = 0;
                Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
                this.lightSource = null;
                return;
            }
            if (this.Decoration_type == 13)
            {
                if (this.sourceIndexOffset == 1)
                {
                    this.sourceRect = this.defaultSourceRect;
                }
                this.sourceIndexOffset = 0;
                if (Game1.isRaining)
                {
                    this.sourceRect = this.defaultSourceRect;
                    this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
                    this.sourceIndexOffset = 1;
                    return;
                }
                if (!this.lightGlowAdded && !environment.lightGlows.Contains(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize))))
                {
                    environment.lightGlows.Add(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                }
                this.lightGlowAdded = true;
            }
        }

        public override bool minutesElapsed(int minutes, GameLocation environment)
        {
            if (Game1.isDarkOut())
            {
                this.addLights(environment);
            }
            else
            {
                this.removeLights(environment);
            }
            return false;
        }

        public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
        {
            this.removeLights(environment);
            if (this.Decoration_type == 13 && this.lightGlowAdded)
            {
                environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
                this.lightGlowAdded = false;
            }
           
            base.performRemoveAction(tileLocation, environment);
        }

        public void rotate()
        {
            if (this.rotations < 2)
            {
                return;
            }
            int num = (this.rotations == 4) ? 1 : 2;
            this.currentRotation += num;
            this.currentRotation %= 4;
            this.flipped = false;
            Point point = default(Point);
            int num2 = this.Decoration_type;
            switch (num2)
            {
                case 2:
                    point.Y = 1;
                    point.X = -1;
                    break;
                case 3:
                    point.X = -1;
                    point.Y = 1;
                    break;
                case 4:
                    break;
                case 5:
                    point.Y = 0;
                    point.X = -1;
                    break;
                default:
                    if (num2 == 12)
                    {
                        point.X = 0;
                        point.Y = 0;
                    }
                    break;
            }
            bool flag = this.Decoration_type == 5 || this.Decoration_type == 12 || this.parentSheetIndex == 724 || this.parentSheetIndex == 727;
            bool flag2 = this.defaultBoundingBox.Width != this.defaultBoundingBox.Height;
            if (flag && this.currentRotation == 2)
            {
                this.currentRotation = 1;
            }
            if (flag2)
            {
                int height = this.boundingBox.Height;
                switch (this.currentRotation)
                {
                    case 0:
                    case 2:
                        this.boundingBox.Height = this.defaultBoundingBox.Height;
                        this.boundingBox.Width = this.defaultBoundingBox.Width;
                        break;
                    case 1:
                    case 3:
                        this.boundingBox.Height = this.boundingBox.Width + point.X * Game1.tileSize;
                        this.boundingBox.Width = height + point.Y * Game1.tileSize;
                        break;
                }
            }
            Point point2 = default(Point);
            int num3 = this.Decoration_type;
            if (num3 == 12)
            {
                point2.X = 1;
                point2.Y = -1;
            }
            if (flag2)
            {
                switch (this.currentRotation)
                {
                    case 0:
                        this.sourceRect = this.defaultSourceRect;
                        break;
                    case 1:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
                        break;
                    case 2:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width + this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                        break;
                    case 3:
                        this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + point.Y * 16 + point2.X * 16, this.defaultSourceRect.Width + 16 + point.X * 16 + point2.Y * 16);
                        this.flipped = true;
                        break;
                }
            }
            else
            {
                this.flipped = (this.currentRotation == 3);
                if (this.rotations == 2)
                {
                    this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 2) ? 1 : 0) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                }
                else
                {
                    this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 3) ? 1 : this.currentRotation) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
                }
            }
            if (flag && this.currentRotation == 1)
            {
                this.currentRotation = 2;
            }
            this.updateDrawPosition();
        }

        public bool isGroundFurniture()
        {
            return this.Decoration_type != 13 && this.Decoration_type != 6 && this.Decoration_type != 13;
        }

        public override bool canBeGivenAsGift()
        {
            return false;
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            if ((l is FarmHouse))
            {
                for (int i = 0; i < this.boundingBox.Width / Game1.tileSize; i++)
                {
                    for (int j = 0; j < this.boundingBox.Height / Game1.tileSize; j++)
                    {
                        Vector2 vector = tile * (float)Game1.tileSize + new Vector2((float)i, (float)j) * (float)Game1.tileSize;
                        vector.X += (float)(Game1.tileSize / 2);
                        vector.Y += (float)(Game1.tileSize / 2);
                        foreach (KeyValuePair<Vector2, StardewValley.Object> something in l.objects)
                        {
                            StardewValley.Object obj = something.Value;
                            if ((obj.GetType()).ToString().Contains("ModularDecoration"))
                            {
                                ModularDecoration current = (ModularDecoration)obj;
                                if (current.Decoration_type == 11 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y) && current.heldObject == null && this.getTilesWide() == 1)
                                {
                                    bool result = true;
                                    return result;
                                }
                                if ((current.Decoration_type != 12 || this.Decoration_type == 12) && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
                                {
                                    bool result = false;
                                    return result;
                                }
                            }
                        }
                    }
                }
                return base.canBePlacedHere(l, tile);
            }
            else
            {
                // Game1.showRedMessage("NOT FARMHOUSE");
                for (int i = 0; i < this.boundingBox.Width / Game1.tileSize; i++)
                {
                    for (int j = 0; j < this.boundingBox.Height / Game1.tileSize; j++)
                    {
                        Vector2 vector = tile * (float)Game1.tileSize + new Vector2((float)i, (float)j) * (float)Game1.tileSize;
                        vector.X += (float)(Game1.tileSize / 2);
                        vector.Y += (float)(Game1.tileSize / 2);
                        /*
                        foreach (Decoration current in (l as FarmHouse).Decoration)
                        {
                            if (current.Decoration_type == 11 && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y) && current.heldObject == null && this.getTilesWide() == 1)
                            {
                                bool result = true;
                                return result;
                            }
                            if ((current.Decoration_type != 12 || this.Decoration_type == 12) && current.getBoundingBox(current.tileLocation).Contains((int)vector.X, (int)vector.Y))
                            {
                                bool result = false;
                                return result;
                            }
                        }
                        */
                    }
                }
                return base.canBePlacedHere(l, tile);
            }
        }

        public void updateDrawPosition()
        {
            this.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
        }

        public int getTilesWide()
        {
            return this.boundingBox.Width / Game1.tileSize;
        }

        public int getTilesHigh()
        {
            return this.boundingBox.Height / Game1.tileSize;
        }

        public override bool placementAction(GameLocation location, int x, int y, StardewValley.Farmer who = null)
        {



            Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);


            this.tileLocation = new Vector2((float)point.X, (float)point.Y);
            bool flag = false;
            if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
            {
                int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                bool flag2 = false;

                if (!flag2)
                {
                    Game1.showRedMessage("Must be placed on wall");
                    return false;
                }
                flag = true;
            }
            for (int i = point.X; i < point.X + this.getTilesWide(); i++)
            {
                for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                {
                    if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                    {
                        Game1.showRedMessage("Furniture can't be placed here");
                        return false;
                    }

                    if (location.getTileIndexAt(i, j, "Buildings") != -1)
                    {
                        return false;
                    }
                }
            }
            if(this.boundingBox.Width==0 && this.boundingBox.Height==0) this.boundingBox = new Rectangle(Int32.MinValue, y / Game1.tileSize * Game1.tileSize, 0, 0);
            
           else this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
            /*
            foreach (Furniture current2 in (location as DecoratableLocation).furniture)
                {
                    if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                    {
                        current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                        bool result = true;
                        return result;
                    }
                }
                */
            using (List<StardewValley.Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
            }
            this.updateDrawPosition();
            bool f=Util.placementAction(this,location,x,y,who);
            Log.AsyncM(this.sourceRect);
            int fff = 1;
            for (int X=0; X<(this.sourceRect.Width/16)*Game1.tileSize; X+=Game1.tileSize)
            {
                for (int Y = 0; Y < (this.sourceRect.Height/16)*Game1.tileSize; Y+=Game1.tileSize)
                {
                    if (X == 0 && Y == 0) continue;
                    
                    ModularDecoration newThing = new ModularDecoration(this.parentSheetIndex, Vector2.Zero, this.SourceFilePathAndName, false, (X/Game1.tileSize), Y/Game1.tileSize);
                    newThing.placementActionNonRecursive(location, x + X, y + Y, who);
                    fff++;
                    Log.AsyncO("Placed an object: " + fff + " times.");
                    // f = Util.placementAction(newThing, location, x+X, y+Y, who);
                    //Log.AsyncG("X: "+X+" Y: "+Y);
                }
            }
           /*
            if (f == true)
            {
                foreach(var v in blankSpace)
                {
                    v.placementAction(Game1.player.currentLocation, x+(v.xTileOffset), y+(v.yTileOffset), who);
                }
            }
            */
            return f;
            //  Game1.showRedMessage("Can only be placed in House");
            //  return false;
        }

        public bool placementActionNonRecursive(GameLocation location, int x, int y, StardewValley.Farmer who = null)
        {



            Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);


            this.tileLocation = new Vector2((float)point.X, (float)point.Y);
            bool flag = false;
            if (this.Decoration_type == 6 || this.Decoration_type == 13 || this.parentSheetIndex == 1293)
            {
                int num = (this.parentSheetIndex == 1293) ? 3 : 0;
                bool flag2 = false;

                if (!flag2)
                {
                    Game1.showRedMessage("Must be placed on wall");
                    return false;
                }
                flag = true;
            }
            for (int i = point.X; i < point.X + this.getTilesWide(); i++)
            {
                for (int j = point.Y; j < point.Y + this.getTilesHigh(); j++)
                {
                    if (location.doesTileHaveProperty(i, j, "NoFurniture", "Back") != null)
                    {
                        Game1.showRedMessage("Furniture can't be placed here");
                        return false;
                    }

                    if (location.getTileIndexAt(i, j, "Buildings") != -1)
                    {
                        return false;
                    }
                }
            }
            //if(this.xTileOffset!=0 || this.yTileOffset!=0) this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, Game1.tileSize/2);
            if (this.boundingBox.Width == 0 && this.boundingBox.Height == 0) this.boundingBox = new Rectangle(Int32.MinValue, y / Game1.tileSize * Game1.tileSize, 0, 0);

            else this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
            Log.AsyncC(this.boundingBox);
            /*
            foreach (Furniture current2 in (location as DecoratableLocation).furniture)
                {
                    if (current2.furniture_type == 11 && current2.heldObject == null && current2.getBoundingBox(current2.tileLocation).Intersects(this.boundingBox))
                    {
                        current2.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
                        bool result = true;
                        return result;
                    }
                }
                */
            using (List<StardewValley.Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox))
                    {
                        Game1.showRedMessage("Can't place on top of a person.");
                        bool result = false;
                        return result;
                    }
                }
            }
            this.updateDrawPosition();
            bool f = Util.placementAction(this, location, x, y, who);
            /*
             if (f == true)
             {
                 foreach(var v in blankSpace)
                 {
                     v.placementAction(Game1.player.currentLocation, x+(v.xTileOffset), y+(v.yTileOffset), who);
                 }
             }
             */
            if (f == true) Lists.DecorationsToDraw.Add(this);
            return f;
            //  Game1.showRedMessage("Can only be placed in House");
            //  return false;
        }

        public override bool isPlaceable()
        {
            return true;
        }

        public override Rectangle getBoundingBox(Vector2 tileLocation)
        {
            return this.boundingBox;
        }

        public override int salePrice()
        {
            return this.price;
        }

        public override int maximumStackSize()
        {
            return 1;
        }

        public override int getStack()
        {
            return this.stack;
        }

        public override int addToStack(int amount)
        {
            return 1;
        }

        private float getScaleSize()
        {
            int num = this.sourceRect.Width / 16;
            int num2 = this.sourceRect.Height / 16;

            return 1f;

            if (num >= 5)
            {
                return 0.75f;
            }
            if (num2 >= 3)
            {
                return 1f;
            }
            if (num <= 2)
            {
                return 2f;
            }
            if (num <= 4)
            {
                return 1f;
            }
            return 0.1f;
        }

        /// <summary>
        /// Will override traditional drawn when held logic to get all of the furniture at once, not just the pieces.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="objectPosition"></param>
        /// <param name="f"></param>
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, StardewValley.Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        /// <summary>
        /// Will override traditional
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location"></param>
        /// <param name="scaleSize"></param>
        /// <param name="transparency"></param>
        /// <param name="layerDepth"></param>
        /// <param name="drawStackNumber"></param>
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            spriteBatch.Draw(TextureSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * this.getScaleSize() * scaleSize, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Will only draw scaled offset tile instead of entire thing.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alpha"></param>
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            if (this.boundingBox.Width == 0 && this.boundingBox.Height == 0) return;
            //Draw non held items.
            if (x == -1)
            {
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }
            else
            {
                //Log.AsyncM("SOURCE:" + new Vector2(xTileOffset, yTileOffset));
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y*Game1.tileSize)),(new Rectangle(this.sourceRect.X+(this.xTileOffset*16), (this.sourceRect.Y)+(this.yTileOffset*16), (1)* 16, (1) * 16)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }

            //Draw held items.
            if (this.heldObject != null)
            {
                if (this.heldObject is ModularDecoration)
                {
                    (this.heldObject as ModularDecoration).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as ModularDecoration).sourceRect.Height * Game1.pixelZoom - Game1.tileSize / 4))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
                    return;
                }
                spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
            }
        }

        public void drawInfront(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            //Draw non held items.
            if (x == -1)
            {
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }
            else
            {
                //Log.AsyncM("SOURCE:" + new Vector2(xTileOffset, yTileOffset));
                spriteBatch.Draw(TextureSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), y * Game1.tileSize)), (new Rectangle(this.sourceRect.X + (this.xTileOffset * 16), (this.sourceRect.Y) + (this.yTileOffset * 16), (1) * 16, (1) * 16)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.Decoration_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
            }

            //Draw held items.
            if (this.heldObject != null)
            {
                if (this.heldObject is ModularDecoration)
                {
                    (this.heldObject as ModularDecoration).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as ModularDecoration).sourceRect.Height * Game1.pixelZoom - Game1.tileSize / 4))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
                    return;
                }
                spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
                spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - Game1.tileSize * 4 / 3))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
            }
        }

        /// <summary>
        /// Draw this elsewhere?
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location"></param>
        /// <param name="layerDepth"></param>
        /// <param name="alpha"></param>
        public void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
        {
            spriteBatch.Draw(TextureSheet, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
        }

        public override Item getOne()
        {
            ModularDecoration ModularDecoration = new ModularDecoration(this.parentSheetIndex, this.tileLocation,this.SourceFilePathAndName);

            /*
            drawPosition = this.drawPosition;
            defaultBoundingBox = this.defaultBoundingBox;
            boundingBox = this.boundingBox;
            currentRotation = this.currentRotation - 1;
            rotations = this.rotations;
            rotate();
            */
            return ModularDecoration;
        }

        public override string getCategoryName()
        {
            return "ModularDecoration";
            //  return base.getCategoryName();
        }

        public override Color getCategoryColor()
        {
            return Util.invertColor(LightColors.DodgerBlue);
        }
    }
}