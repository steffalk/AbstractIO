using System;

namespace AbstractIO
{
    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IDoubleOutput"/> objects.
    /// </summary>
    public class DoubleOutputDistributor : IDoubleOutput
    {
        private readonly IDoubleOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IDoubleOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public DoubleOutputDistributor(params IDoubleOutput[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                throw new ArgumentNullException(nameof(targets));
            }
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] == null)
                    {
                        throw new ArgumentException("targets must not contain empty elements.");
                    }
                }
                _targets = new IDoubleOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public Double Value
        {
            get
            {
                return _targets[0].Value;
            }
            set
            {
                for (int i = 0; i < _targets.Length; i++)
                {
                    _targets[i].Value = value;
                }
            }
        }
    }
}
