using System;
using System.Threading;

namespace AbstractIO.Samples
{
    public static class Sample11SimpleTrainWithDoors
    {
        /// <summary>
        /// Runs a "Turmbergbahn" train, that is, 2 trains hanging on a single steel wire driven by a motor, running on
        /// the same rails using a "Abt'sche Weiche".
        /// </summary>
        /// <param name="trainMotor">The motor driving both trains at once. +1.0 is output for the direction so that
        /// train 1 drives upwards and train 2 drives downwards, -1.0 vice versa.</param>
        /// <param name="train1ReachedBottomStation">Signals true when train 1 reached the bottom station (and thus
        /// train 2 reached the top station).</param>
        /// <param name="train2ReachedBottomStation">Signals true when train 2 reached the bottom station (and thus
        /// train 1 reached the top station).</param>
        /// <param name="doorMotor">The motor driving all doors on both trains at once. +1.0 is output for opening,
        /// -1.0 for closing.</param>
        /// <param name="redLight">True shall light up a red traffic light when people shall not enter or leave the
        /// train.</param>
        /// <param name="greenLight">True shall light up a green traffic light when people may enter or leave the
        /// train.</param>
        /// <param name="waitForDoorsToMoveInMs">The time, in milliseconds, to wait for the
        /// <paramref name="doorMotor"/> to have operated all doors reliably.</param>
        /// <param name="waitWithOpenDoorsInMs">The time, in milliseconds, that the doors shall remain open.</param>
        /// <param name="waitAroundDoorOperationsInMs">The time, in milliseconds, to wait after the train stopped and
        /// before opening the door, and after the doors were closed again before the train starts.</param>
        public static void Run(ISingleOutput trainMotor,
                               IBooleanInput trainReachedBottomStation,
                               ISingleOutput doorMotor,
                               int waitForDoorsToMoveInMs,
                               int waitWithOpenDoorsInMs,
                               int waitAroundDoorOperationsInMs)
        {
            // Check arguments:

            if (trainMotor == null)
            {
                throw new ArgumentNullException(nameof(trainMotor));
            }
            if (trainReachedBottomStation == null)
            {
                throw new ArgumentNullException(nameof(trainReachedBottomStation));
            }
            if (doorMotor == null)
            {
                throw new ArgumentNullException(nameof(doorMotor));
            }
            if (waitForDoorsToMoveInMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waitForDoorsToMoveInMs));
            }
            if (waitWithOpenDoorsInMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waitWithOpenDoorsInMs));
            }
            if (waitAroundDoorOperationsInMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waitAroundDoorOperationsInMs));
            }

            // Run the train:

            float moveDirection = 1.0f;

            while (true)
            {
                // Move the train in the current direction until one of the end buttons is pressed:
                if (!trainReachedBottomStation.Value)
                {
                    trainMotor.Value = moveDirection;
                    trainReachedBottomStation.WaitFor(value: true, edgeOnly: false);
                    trainMotor.Value = 0.0f;
                }

                // Change direction for the next pass:
                moveDirection = -moveDirection;

                // Wait a bit before opening the doors:
                Thread.Sleep(waitAroundDoorOperationsInMs);

                // Open the door:
                doorMotor.Value = 1.0f;
                Thread.Sleep(waitForDoorsToMoveInMs);
                doorMotor.Value = 0.0f;

                // Let people step in and out, wait a bit:
                Thread.Sleep(waitWithOpenDoorsInMs);

                // Close the door:
                doorMotor.Value = -1.0f;
                Thread.Sleep(waitForDoorsToMoveInMs);
                doorMotor.Value = 0.0f;

                // Wait a bit before the train starts again:
                Thread.Sleep(waitAroundDoorOperationsInMs);
            }
        }
    }
}
