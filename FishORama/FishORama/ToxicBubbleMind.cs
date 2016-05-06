using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.



namespace FishORama
{
    class ToxicBubbleMind : AIPlayer
    {
        #region Data Members

        private AquariumToken mAquarium;        // Reference to the aquarium in which the creature lives.
        private float mFacingDirection = 0;
        Random speed = new Random();
        private int mSpeed;
        private float yPos;
        private ToxicBarrelToken mToxicBarrel;




        #endregion

        #region Properties
        public AquariumToken Aquarium
        {
            set { mAquarium = value; }
        }

        public ToxicBarrelToken ToxicBarrel
        {
            set { mToxicBarrel = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="pToken">Token to be associated with the mind.</param>
        public ToxicBubbleMind(X2DToken pToken)
        {
            this.Possess(pToken);       // Possess token.
            Vector3 tokenPosition = this.PossessedToken.Position;
            yPos = tokenPosition.Y;
            mSpeed = speed.Next(3, 6);
        }

        #endregion

        #region Methods
      
        public override void Update(ref GameTime pGameTime)
        {
            Vector3 tokenPosition = this.PossessedToken.Position;

            tokenPosition.Y += mSpeed;
            tokenPosition.X += (float)Math.Sin(tokenPosition.Y / 30);
            this.PossessedToken.Position = tokenPosition;

                if (this.PossessedToken.Position.Y >= mToxicBarrel.Position.Y + 300)
                {
                    this.PossessedToken.Position = mToxicBarrel.Position;
                    
                }
          
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                                  this.PossessedToken.Orientation.X,
                                                                  this.PossessedToken.Orientation.Z);

        }
        #endregion
    }
}

