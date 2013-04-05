﻿// Animation.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype1
{
    public class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteSheet;

        // The scale used to display the sprite strip
        float scale;


        // The time since we last updated the frame
        int elapsedTime;


        // The time we display a frame until the next one
        public int frameTime;

        // The time we display a frame until the next one
        public int minFrameTime;


        // The number of frames that the animation contains
        int frameCount;


        // The index of the current frame we are displaying
        int currentFrame;


        // The color of the frame we will be displaying
        Color color;


        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();


        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();


        // Width of a given frame
        public int FrameWidth;


        // Height of a given frame
        public int FrameHeight;


        // The state of the Animation
        public bool Active;


        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        public bool paused;

        // Width of a given frame
        public Vector2 Position;

        public SpriteEffects myEffect = SpriteEffects.None;

        public Vector2 origin; 
               

        public void Initialize(Texture2D texture, Vector2 position,
int frameWidth, int frameHeight, int frameCount,
int frametime, Color color, float scale, bool looping, Vector2 origin)
        {
            // Keep a local copy of the values passed in
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.minFrameTime = frametime;
            this.scale = scale;
            this.origin = origin;            

            Looping = looping;
            Position = position;
            spriteSheet = texture;            

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            paused = false;

            // Set the Animation to active by default
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false)
                return;


            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                if (paused == false)
                {
                    // Move to the next frame
                    currentFrame++;
                }

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping pause the animation
                    if (Looping == false)
                    {
                        currentFrame = frameCount - 1;

                        paused = true;
                    }
                }


                // Reset the elapsed time to zero
                elapsedTime = 0;
            }


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
            (int)Position.Y - (int)(FrameHeight * scale) / 2,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
        }
        
        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                //spriteBatch.Draw(spriteSheet, destinationRect, sourceRect, color);

                spriteBatch.Draw(spriteSheet, destinationRect, sourceRect, color, 0.0f, origin, myEffect, 0.0f);
            }
        }

    }
}
