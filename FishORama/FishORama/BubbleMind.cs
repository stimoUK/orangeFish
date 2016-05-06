using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.



namespace FishORama
{
    class BubbleMind : AIPlayer
    {
        #region Data Members

        private AquariumToken mAquarium;        // Reference to the aquarium in which the creature lives.
        private float mFacingDirection = 0;
        Random speed = new Random();
        private int mSpeed;
        private float yPos;
        private OrangeFishToken mOrangeFish;




        #endregion

        #region Properties
        public AquariumToken Aquarium
        {
            set { mAquarium = value; }
        }
        public OrangeFishToken orangeFish
        {
            set { mOrangeFish = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pToken">Token to be associated with the mind.</param>
        public BubbleMind(X2DToken pToken)
        {
            this.Possess(pToken);       // Possess token.
            Vector3 tokenPosition = this.PossessedToken.Position;
            yPos = tokenPosition.Y;
            mSpeed = speed.Next(3, 6);// random speed for the bubbles
        }

        #endregion

        #region Methods

        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>
        public override void Update(ref GameTime pGameTime)
        {
            Vector3 tokenPosition = this.PossessedToken.Position;

            tokenPosition.Y += mSpeed;
            tokenPosition.X += (float)Math.Sin(tokenPosition.Y / 30);
                                                            //devided by the radius
            /*this is making the bubble sway by oscillating horizontally
            http://gamedev.stackexchange.com/questions/9198/oscillating-sprite-movement-in-xna
            https://immersivenick.wordpress.com/2011/10/31/xna-short-using-sine-cosine-to-move-an-object-forward/ */
            
            this.PossessedToken.Position = tokenPosition;

            //reset greater than 150 -> then
            if (tokenPosition.Y >= mOrangeFish.Position.Y + 150)
            {   
                //place on the fish location 
                this.PossessedToken.Position = mOrangeFish.Position;                  
            }

            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                                  this.PossessedToken.Orientation.X,
                                                                  this.PossessedToken.Orientation.Z);

        }
        #endregion
    }
}
