using System;

namespace AbstractIO
{
    #region Inverter

    /// <summary>
    /// A class inverting an <see cref="IBooleanOuput"/>.
    /// </summary>
    public class BooleanOutputInverter : IBooleanOutput
    {
        private readonly IBooleanOutput _target;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="target">The ouput get the inverted Value of this object.</param>
        public BooleanOutputInverter(IBooleanOutput target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Gets or sets the inverted Value of the target object passed to the constructor, or sets the target object's
        /// Value to the inverted Value of this object.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_target.Value;
            }
            set
            {
                _target.Value = !value;
            }
        }
    }

    #endregion

    #region Output Distribution

    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IBooleanOutput"/> objects.
    /// </summary>
    public class BooleanOutputDistributor : IBooleanOutput
    {
        private readonly IBooleanOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IBooleanOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public BooleanOutputDistributor(params IBooleanOutput[] targets)
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
                _targets = new IBooleanOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public bool Value
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

    /// <summary>
    /// A class distributing the values received to an arbitrary number of other <see cref="IIntegerOutput"/> objects.
    /// </summary>
    public class IntegerOutputDistributor : IIntegerOutput
    {
        private readonly IIntegerOutput[] _targets;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="targets">The target objects whose <see cref="IIntegerOutput.Value"/> properties shall be set to
        /// the same value as the <see cref="Value"/> property of this object gets set.</param>
        public IntegerOutputDistributor(params IIntegerOutput[] targets)
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
                _targets = new IIntegerOutput[targets.Length];
                targets.CopyTo(_targets, 0);
            }
        }

        /// <summary>
        /// Gets or sets the value. Setting it will set the values on all target objects passed to the constructor of
        /// this object.
        /// </summary>
        public int Value
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

    #endregion

    public static class OutputConverterExtensionMethods
    {
        /// <summary>
        /// Creates an <see cref="BooleanOutputInverter"/> object.
        /// </summary>
        /// <param name="target">The output which shall receive the inverted Value of this object.</param>
        /// <returns>The <see cref="BooleanOutputInverter"/> object sending inverted
        /// <see cref="IBooleanOutput">Values</see> to <paramref name="target"/>.</returns>
        public static IBooleanOutput Invert(this IBooleanOutput target)
        {
            return new BooleanOutputInverter(target);
        }
    }
}