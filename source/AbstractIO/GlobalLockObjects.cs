namespace AbstractIO
{
    /// <summary>
    /// Objects for global locking of system-wide resources.
    /// </summary>
    public static class GlobalLockObjects
    {
        private static object _i2cLockObject = new object();

        /// <summary>
        /// Gets the object used for lock-Statements when I²C data is sent or received in order to not interleave calls
        /// from multiple threads. Lock this object in any I²C operation so that other threads cannot use I²C on the
        /// same pins at the same time.
        /// </summary>
        public static object I2cLockObject
        {
            get
            {
                return _i2cLockObject;
            }
        }
    }

}
