using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


namespace FishORama
{

    class ToxicBarrelMind : AIPlayer
    {
        #region Data Members

        private AquariumToken mAquarium;    // Reference to the aquarium in which the creature lives.
        private int mFacingDirection;
        private float mSpeed = 2;
        private Vector3 MarkerVector;
        private bool RemoveX = false;
        private bool RemoveY = false;
        private bool exit = false;
        private float rSpeed = 2;
        private float lSpeed = -2;
        private bool Move = false;

        #endregion

        #region Properties

        /// <summary>
        /// Set Aquarium in which the mind's behavior should be enacted.
        /// </summary>
        public AquariumToken Aquarium
        {
            set { mAquarium = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pToken">Token to be associated with the mind.</param>
        public ToxicBarrelMind(X2DToken pToken)
        {
            this.Possess(pToken);       // Possess token.
            mFacingDirection = 1;
        }

        #endregion

        #region Methods

        /* barrel can be moved by pressing left and right on the keyboard but beacuse of the normalize()
         it still goes towards the original vector, once it has reached its target you can then move it 
         left and right freely. Pressing space resets the barrel.*/

        #region Drop

        public void Drop()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            tokenPosition.X += (float)Math.Sin(tokenPosition.Y / 30);
                                                            //devided by the radius
            /*this is making the barrel sway by oscillating horizontally
            http://gamedev.stackexchange.com/questions/9198/oscillating-sprite-movement-in-xna
            https://immersivenick.wordpress.com/2011/10/31/xna-short-using-sine-cosine-to-move-an-object-forward/ */


            /*This is saying that if the marker is displayed to the left
            of the toxic barrel on the screen that the barrel must face left.*/
            if (tokenPosition.X - mAquarium.Marker.Position.X < 0)
            {
                mFacingDirection = 1;
            }
            /*This is saying that if the marker is displayed to the right
            of the toxic barrel on the screen that the barrel must face right.*/
            else if (tokenPosition.X - mAquarium.Marker.Position.X > 0)
            {
                mFacingDirection = -1;
            }
            //This is giving the toxic barrel and the marker a vector3 name of MarkerVector
            MarkerVector = tokenPosition - mAquarium.Marker.Position;
            /*Normalize is changing the vector from a length to a direction which then 
              the toxic Barrel as a point to go to.
             researched http://xnafan.net/2012/12/pointing-and-moving-towards-a-target-in-xna-2d/ */
            MarkerVector.Normalize();
            tokenPosition = tokenPosition - MarkerVector * mSpeed;

            //Deleting the chickenleg

            //if X coor and barrel are within 5px of each other then the X coor is removed.
            if (tokenPosition.X - mAquarium.Marker.Position.X <= 5 && tokenPosition.X - mAquarium.Marker.Position.X >= -5)
            {
                RemoveX = true;
            }

            if (tokenPosition.Y - mAquarium.Marker.Position.Y <= 5 && tokenPosition.Y - mAquarium.Marker.Position.Y >= -5)
            {
                RemoveY = true;
            }

            /*If both the X and Y coordinates are removed then the marker is removed
              and the removes are reset*/
            if (RemoveX && RemoveY == true)
            {
                mAquarium.RemoveMarker();
                RemoveX = false;
                RemoveY = false;
            }

            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                            this.PossessedToken.Orientation.Y,
                                            this.PossessedToken.Orientation.Z);

        }

        #endregion

        #region reset barrel

        public void reset()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            mFacingDirection = 1;
            tokenPosition.Y = 1000;
            tokenPosition.Z = 0;
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                            this.PossessedToken.Orientation.Y,
                                            this.PossessedToken.Orientation.Z);
        }

        #endregion

        #region Move Right

        //moves right with a speed of 2
        private Vector3 Right(Vector3 tokenPosition)
        {
            tokenPosition.X = tokenPosition.X + rSpeed; 

            return tokenPosition;
        }

        #endregion

        #region Move Left

        //moves left with a speed of 2
        private Vector3 Left(Vector3 tokenPosition)
        {
            tokenPosition.X = tokenPosition.X + lSpeed;

            return tokenPosition;
        }

        #endregion
        
        
        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>
        ///
        public override void Update(ref GameTime pGameTime)
        {
            Vector3 tokenPosition = this.PossessedToken.Position;

            //researched keystates at http://snipd.net/detecting-keyboard-input-in-xna
            KeyboardState keystate = Keyboard.GetState(); //keystate

            //if right is pressed.
            if (keystate.IsKeyDown(Keys.Right))
            {
                Move = true;
                if (Move == true)
                {
                    tokenPosition = Right(tokenPosition);
                    Move = false;
                }
            }


            //if left is pressed
            if (keystate.IsKeyDown(Keys.Left))
            {
                Move = true;
                if (Move == true)
                {
                    tokenPosition = Left(tokenPosition);
                    Move = false;
                }
            }
            
            this.PossessedToken.Position = tokenPosition;
            
            
            //when marker is removed and space is pressed the barrel resets.
            if (mAquarium.Marker != null)
            {
                Drop();
            }
            else if (mAquarium.Marker == null && keystate.IsKeyDown(Keys.Space))
            {
                exit = true;
                if (exit == true)
                {
                    reset();
                    exit = false;
                }  
            }
        }

            

        #endregion
    }


}



