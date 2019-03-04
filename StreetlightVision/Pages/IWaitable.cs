namespace StreetlightVision.Pages
{
    interface IWaitable
    {
        /// <summary>
        /// Wait until an action completely loaded
        /// </summary>
        void WaitForCompletelyLoaded();

        /// <summary>
        /// Wait until an action completely loaded
        /// </summary>
        void WaitForPreviousActionComplete();
    }
}
