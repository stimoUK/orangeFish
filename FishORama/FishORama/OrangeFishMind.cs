using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine general features.
using XNAMachinationisRatio.AI;             // Required to use the XNA Machinationis Ratio general AI features.


/* LERNING PILL: XNAMachinationisRatio Engine
 * XNAMachinationisRatio is an engine that allows implementing
 * simulations and games based on XNA, simplifying the use of XNA
 * and adding features not directly available in XNA.
 * XNAMachinationisRatio is a work in progress.
 * The engine works "under the hood", taking care of many features
 * of an interactive simulation automatically, thus minimizing
 * the amount of code that developers have to write.
 * 
 * In order to use the engine, the application main class (Kernel, in the
 * case of FishO'Rama) creates, initializes and stores
 * an instance of class Engine in one of its data members.
 * 
 * The classes comprised in the  XNA Machinationis Ratio engine and the
 * related functionalities can be accessed from any of your XNA project
 * source code files by adding appropriate 'using' statements at the beginning of
 * the file. 
 * 
 */

namespace FishORama
{
    /* LEARNING PILL: Token behaviors in the XNA Machinationis Ratio engine
     * Some simulation tokens may need to enact specific behaviors in order to
     * participate in the simulation. The XNA Machinationis Ratio engine
     * allows a token to enact a behavior by associating an artificial intelligence
     * mind to it. Mind objects are created from subclasses of the class AIPlayer
     * included in the engine. In order to associate a mind to a token, a new
     * mind object must be created, passing to the constructor of the mind a reference
     * of the object that must be associated with the mind. This must be done in
     * the DefaultProperties method of the token.
     * 
     * Hence, every time a new tipe of AI mind is required, a new class derived from
     * AIPlayer must be created, and an instance of it must be associated to the
     * token classes that need it.
     * 
     * Mind objects enact behaviors through the method Update (see below for further details). 
     */
    class OrangeFishMind : AIPlayer
    {
        #region Data Members

        // This mind needs to interact with the token which it possesses, 
        // since it needs to know where are the aquarium's boundaries.
        // Hence, the mind needs a "link" to the aquarium, which is why it stores in
        // an instance variable a reference to its aquarium.
        private AquariumToken mAquarium;        // Reference to the aquarium in which the creature lives.

        private int mFacingDirection;         // Direction the fish is facing (1: right; -1: left).
        private int mSpeed = 2;
        //double to be more precise for time
        private double Time;
        private double AccelerationTime;
        private double DecellerationTime;
        private double HungryTime;
        private double StartTime;
        private double StopTime;
        private float i;
        Random rNum = new Random();
        private int ranSwimBehave;
        private int ranSink;
        private float dashDistance;
        private float hungryDistance;
        private float sinkDistance;
        private bool Reset = true;



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
        public OrangeFishMind(X2DToken pToken)
        {
            /* LEARNING PILL: associating a mind with a token
             * In order for a mind to control a token, it must be associated with the token.
             * This is done when the mind is constructed, using the method Possess inherited
             * from class AIPlayer.
             */
            this.Possess(pToken);       // Possess token.
            mFacingDirection = 1;       // Current direction the fish is facing.
        }

        #endregion

        #region Methods

        /* LEARNING PILL: The AI update method.
         * Mind objects enact behaviors through the method Update. This method is
         * automatically invoked by the engine, periodically, 'under the hood'. This can be
         * be better understood that the engine asks to all the available AI-based tokens:
         * "Would you like to do anything at all?" And this 'asking' is done through invoking
         * the Update method of each mind available in the system. The response is the execution
         * of the Update method of each mind , and all the methods possibly triggered by Update.
         * 
         * Although the Update method could invoke other methods if needed, EVERY
         * BEHAVIOR STARTS from Update. If a behavior is not directly coded in Updated, or in
         * a method invoked by Update, then it is IGNORED.
         * 
         */

        #region RandomSwimBehaviour

        public void RandomSwimBehaviour()
        {
            int ranSwimBehaviour = rNum.Next(1, 6);
            ranSwimBehave = ranSwimBehaviour;
        }

        #endregion
            
        #region HorizontalSwimBehaviour

        public void HorizontalSwimBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            tokenPosition.X = tokenPosition.X + (mSpeed * mFacingDirection);
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);

            // Reached Horizontal Boundary is defined in the Aquarium Token.
            if (this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {
                // an alternative way of writing mFacingDirection * -1;
                mFacingDirection = -mFacingDirection;
            }

        }
        #endregion

        #region VerticalSwimBehaviour

        public void VerticalSwimBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            tokenPosition.Y ++;// ++ for one increment per update
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);

        }

        #endregion

        #region DashBehaviour

        public void DashBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            mSpeed = mSpeed + 10;
            HorizontalSwimBehaviour(); 
            if (tokenPosition.X >= dashDistance + 250 || tokenPosition.X <= dashDistance - 250)
            {
                ranSwimBehave = 5;
            }
            mSpeed = mSpeed - 10;// had to -10 to get his speed back down otherwise it stayed at +10.
        }

        #endregion
 
        #region AccellerationSwimBehaviour

        public void AccellerationSwimBehaviour()
        {
            /* My Notes - accelleration has to use total time and not start time
            otherwise it always adds 15 on and never ends.*/
            // beacuse XNA uses 60 updates per second the speed increases 1 every 1 second and vice versa.
            // http://stackoverflow.com/questions/24970450/xna-update-function-update-rate-varies-greatly-on-different-machines
            HorizontalSwimBehaviour();
            i++;// i++ = 60 every 1 second
            if (i == 60)
            {
                if (Time < AccelerationTime)
                {
                    i = 0;// i = 0 so it resets.
                    mSpeed++;
                }
                if (Time > AccelerationTime && Time <= DecellerationTime)
                {
                    i = 0;
                    mSpeed--;// after the 15 sec/ 15 i resets the speed decreases.       
                }
            }
            // once he has increased for 15 sec and then decreased for 15 sec he goes back to default.
            if (Time > DecellerationTime)
            {
                ranSwimBehave = 5;
            }
        }

        #endregion

        #region HungrySwimBehaviour

        public void HungrySwimBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            tokenPosition.X = tokenPosition.X + (mSpeed * mFacingDirection);
            this.PossessedToken.Position = tokenPosition;
            this.PossessedToken.Orientation = new Vector3(mFacingDirection,
                                                        this.PossessedToken.Orientation.Y,
                                                        this.PossessedToken.Orientation.Z);

            if (tokenPosition.X >= hungryDistance + 75 || tokenPosition.X <= hungryDistance - 75 || this.mAquarium.ReachedHorizontalBoundary(this.PossessedToken))
            {                                                                                 // Reached Horizontal Boundary is defined in the Aquarium Token.
                // an alternative way of writing mFacingDirection * -1;
                mFacingDirection = -mFacingDirection;
            }
            if (tokenPosition.Y <= 300)// stops rising vertical if he goes over 300.
            {
                VerticalSwimBehaviour();
            }
        }

        #endregion

        #region SinkBehaviour

        public void SinkBehaviour()
        {
            Vector3 tokenPosition = this.PossessedToken.Position;
            int rSink = rNum.Next(50, 150);
            ranSink = rSink;

            // as long as he is over -250 he will sink 2px at a time
            if (tokenPosition.Y >= -250)
            {
                tokenPosition.Y = tokenPosition.Y -2;
            }
            this.PossessedToken.Position = tokenPosition;

            //sinkDistance = tokenPosition.Y
            if (tokenPosition.Y <= (sinkDistance - ranSink))
            {
                ranSwimBehave = 5;
            }
            // stops him from sinking below -300
            else if (tokenPosition.Y <= -300)
            {
                ranSwimBehave = 5;
            }
        }

        #endregion

       

        /// <summary>
        /// AI Update method.
        /// </summary>
        /// <param name="pGameTime">Game time</param>
        public override void Update(ref GameTime pGameTime)
        {
            // tried to keep as much as possible in the methods above rather than the update method.
            Vector3 tokenPosition = this.PossessedToken.Position;
            Time = pGameTime.TotalGameTime.TotalSeconds;
            //Console.WriteLine(ranSwimBehave);
            Console.WriteLine(ranSink);

            //resets the all variables when ranSwimBehave = 5.
            if (Reset == true)
            {
                hungryDistance = tokenPosition.X;
                dashDistance = tokenPosition.X;
                sinkDistance = tokenPosition.Y;
                StartTime = pGameTime.TotalGameTime.TotalSeconds;
                AccelerationTime = StartTime + 15;
                DecellerationTime = AccelerationTime + 15;
                StopTime = StartTime + 1;
                HungryTime = StartTime + 5;
                i = 0;
                Reset = false;
            }

            //the starting behaviour
            if (ranSwimBehave == 0)
            {
                HorizontalSwimBehaviour();
                RandomSwimBehaviour();
            }

            //dash behaviour
            else if (ranSwimBehave == 1)
            {
                DashBehaviour();
            }

            //accelleration behaviour
            else if (ranSwimBehave == 2)
            {
                AccellerationSwimBehaviour();
            }
            
            //hungry
            else if (ranSwimBehave == 3)
            {
                HungrySwimBehaviour();
                //time = total, hungrytime = starttime + 5, so when the method starts the hungry time
                // = the method start + 5 and then stops when the total time ticks past it.
                if (Time >= HungryTime)
                {
                    ranSwimBehave = 5;
                }
            }
            
            //sink
            else if (ranSwimBehave == 4)
            {
                SinkBehaviour();
            }

            //reset
            else if (ranSwimBehave == 5)
            {
                HorizontalSwimBehaviour();
                // same principle as hungry behaviour
                if (Time >= StopTime)
                {
                    // when method 5 is called it resets all variables ready to start again.
                    Reset = true;
                    RandomSwimBehaviour();
                }
            }
            
            
        }        
        #endregion
    }
}
