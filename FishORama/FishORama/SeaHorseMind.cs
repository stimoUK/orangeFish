using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.




namespace FishORama
{

    class SeaHorseMind : AIPlayer
    {

        #region Data Members

        private AquariumToken mAquarium;        // Reference to the aquarium in which the creature lives.

        private int mFacingDirection = 1;         // Direction the fish is facing (1: right; -1: left).
        private int xSpeed;
        private int ySpeed;
        private float yPos;
        //static for seperate random speeds
        private static Random seaHorseSpeed = new Random();
        private int rSpeed = seaHorseSpeed.Next(5, 11);
        private bool started = true;
        private bool reset = true;
        private bool up = true;
        private bool down = true;
        
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
        public SeaHorseMind(X2DToken pToken)
        {
            mFacingDirection = 1;
            this.Possess(pToken);
            Vector3 tokenPosition = this.PossessedToken.Position;
            yPos = tokenPosition.Y;
        }

        #endregion

        #region Methods

        #region HorizontalSwimBehaviour

        public void HorizontalSwimBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            tokenPosition.X = tokenPosition.X + (xSpeed * mFacingDirection);
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);
            xSpeed = rSpeed;
            if (this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {
                // an alternative way of writing mFacingDirection * -1;
                mFacingDirection = -mFacingDirection;
            }

        }

        #endregion

        #region VerticalswimBehaviour

        public void VerticalSwimBehaviour()
        {
            ySpeed = seaHorseSpeed.Next(2, 10);
            Vector3 tokenPosition = this.PossessedToken.Position;
            if (up == true)
            {
                tokenPosition.Y += ySpeed;
                this.PossessedToken.Position = tokenPosition;
                if (tokenPosition.Y >= yPos + 100)
                {
                    reset = true;
                    up = false;
                    down = true;
                }
            }
            else if (down == true)
            {
                tokenPosition.Y -= ySpeed;
                this.PossessedToken.Position = tokenPosition;
                if (tokenPosition.Y <= yPos - 100)
                {
                    reset = true;
                    down = false;
                    up = true;
                }
                this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);
            }
            
        }

        #endregion

        #region ChickLegScatter

        public void ChickLegScatter()
        {
            HorizontalSwimBehaviour();
            VerticalSwimBehaviour();
            
            Vector3 tokenPosition = this.PossessedToken.Position;
            this.PossessedToken.Position = tokenPosition;
            
            if (mAquarium.ChickenLeg != null && this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken) == false)
            {
                xSpeed += 5;
                if (tokenPosition.X <= mAquarium.ChickenLeg.Position.X)
                {
                    mFacingDirection = -1;
                }
                else if (tokenPosition.X > mAquarium.ChickenLeg.Position.X)
                {
                    mFacingDirection = 1;
                }   
            }
            if (tokenPosition.X >= 395 && this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {
                mFacingDirection = -1;
                tokenPosition.X = 390;
            }
            if (tokenPosition.X <= -395 && this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {
                mFacingDirection = 1;
                tokenPosition.X = -390;
            }
        }

        #endregion

        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>

        public override void Update(ref GameTime pGameTime)
        {

            Vector3 tokenPosition = this.PossessedToken.Position;
            Console.WriteLine(seaHorseSpeed);

            if (started == true)
            {
                up = true;
                started = false;
            }
            if (reset == true)
            { 
                yPos = tokenPosition.Y;
                reset = false;              
            }
            

            if (mAquarium.ChickenLeg == null)
            {
                HorizontalSwimBehaviour();
                VerticalSwimBehaviour();
            }
            else if (mAquarium.ChickenLeg != null)
            {
                ChickLegScatter();
            }
            
        }

    }

        #endregion
}