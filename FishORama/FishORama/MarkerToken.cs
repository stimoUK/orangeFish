using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;      // Required to use XNA features.
using XNAMachinationisRatio;        // Required to use the XNA Machinationis Ratio Engine.
namespace FishORama
{
    class MarkerToken : X2DToken
    {
        #region Data Members

        #endregion

        #region Properties
        // No custom properties yet.
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the Signal.
        /// Uses base class to initialize the token name, and adds code to
        /// initialize custom members.
        /// </summary
        /// <param name="pTokenName">Name of the token.</param>
        /// <param name="pAquarium">Reference to the aquarium in which the token lives.</param>
        public MarkerToken(String pTokenName)
            : base(pTokenName)
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Setup default values for this token's porperties.
        /// </summary>
        protected override void DefaultProperties()
        {
            this.GraphicProperties.AssetID = "MarkerVisuals1";
        }

        #endregion
    }
}