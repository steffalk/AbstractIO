namespace AbstractIO
{
    /// <summary>
    /// An interface for objects being able to move a positive or negative number of steps.
    /// </summary>
    public interface IStepDrive
    {
        /// <summary>
        /// Moves a positive or negative number of steps and returns when the desired position is reached.
        /// </summary>
        /// <param name="steps">The number of steps to move relative to the current position. This may be any integer
        /// number, positive or negative, including 0.</param>
        /// <param name="pauseInMs">The number of milliseconds to pause after each step.</param>
        /// <remarks>This method should be implemented blocking. Moving steps in background is to be implemented in
        /// higher level classes.</remarks>
        void MoveSteps(int steps, int pauseInMs);

        /// <summary>
        /// Cuts power from the outputs.
        /// </summary>
        /// <remarks>Use this method to save power or to have to stepper turn freely using outside mechanical force.
        /// </remarks>
        void ReleaseHoldingTorque();
    }
}
