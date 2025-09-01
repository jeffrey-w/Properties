using System.Collections.Immutable;
using Extra.Extensions;
using Extra.Guard;

namespace Properties;

/// <summary>
/// The <c>Value</c> struct represents an immutable piece of data.
/// </summary>
public readonly struct Value : IEquatable<Value>
{
    /// <summary>
    /// Creates a new Boolean <c>Value</c> with the specified
    /// <paramref name="payload" />.
    /// </summary>
    /// <param name="payload">The Boolean held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    public static implicit operator Value(bool payload)
    {
        return new Value(payload);
    }

    /// <summary>
    /// Creates a new integer <c>Value</c> with the specified
    /// <paramref name="payload" />.
    /// </summary>
    /// <param name="payload">The integer held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    public static implicit operator Value(int payload)
    {
        return new Value(payload);
    }

    /// <summary>
    /// Creates a new real <c>Value</c> with the specified <paramref name="payload" />.
    /// </summary>
    /// <param name="payload">The real number held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    public static implicit operator Value(double payload)
    {
        return new Value(payload);
    }

    /// <summary>
    /// Creates a new string <c>Value</c> with the specified
    /// <paramref name="payload" />.
    /// </summary>
    /// <param name="payload">The string held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    public static implicit operator Value(string payload)
    {
        return new Value(payload);
    }

    /// <summary>
    /// Determines whether the <paramref name="first" /> and <paramref name="second" />
    /// <c>Value</c>s hold the same data.
    /// </summary>
    /// <param name="first">One of the <c>Value</c>s to compare.</param>
    /// <param name="second">The other <c>Value</c> to compare.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="first" /> <c>Value</c> is equal to the
    /// <paramref name="second" />.
    /// </returns>
    public static bool operator ==(Value first, Value second)
    {
        return first.Equals(second);
    }

    /// <summary>
    /// Determines whether the <paramref name="first" /> and <paramref name="second" />
    /// <c>Value</c>s hold different data.
    /// </summary>
    /// <param name="first">One of the <c>Value</c>s to compare.</param>
    /// <param name="second">The other <c>Value</c> to compare.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="first" /> <c>Value</c> is not equal to the
    /// <paramref name="second" />.
    /// </returns>
    public static bool operator !=(Value first, Value second)
    {
        return !first.Equals(second);
    }

    /// <summary>The <c>Value</c> that does not hold any data.</summary>
    public static readonly Value Empty = new(null);

    /// <summary>
    /// Creates a new <c>Value</c> with the specified <paramref name="payload" />.
    /// </summary>
    /// <param name="payload">The data held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the specified <paramref name="payload" /> does not belong to
    /// <see cref="bool" />, <see cref="int" />, <see cref="double" />,
    /// <see cref="string" />, or <see cref="IEnumerable{T}" /> over
    /// <see cref="string" />s.
    /// </exception>
    public static Value Make(object? payload)
    {
        return payload switch
        {
            null => Empty,
            bool boolean => boolean,
            int integer => integer,
            double number => number,
            string str => str,
            _ => throw new ArgumentOutOfRangeException(nameof(payload)),
        };
    }

    /// <summary>
    /// Creates a new <c>Value</c> with the specified <paramref name="strings" />.
    /// </summary>
    /// <param name="strings">The data held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value.</c></returns>
    /// <exception cref="ArgumentNullException">
    /// If any of the specified <paramref name="strings" /> are <c>null</c>.
    /// </exception>
    public static Value From(params string[] strings)
    {
        return new Value(
            Against
               .InvalidEnumerable(strings, name: nameof(strings))
               .NullElements()
               .Validated()
               .ToImmutableList());
    }

    /// <summary>
    /// Creates a new <c>Value</c> with the specified <paramref name="strings" />.
    /// </summary>
    /// <param name="strings">The data held by the new <c>Value</c>.</param>
    /// <returns>A new <c>Value</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If any of the specified <paramref name="strings" /> are <c>null</c>.
    /// </exception>
    public static Value From(IEnumerable<string> strings)
    {
        return From(strings.ToArray());
    }

    private static T CastOrThrow<T>(object? payload)
    {
        try
        {
            return (T)payload! ?? throw new InvalidOperationException("This value holds no data.");
        }
        catch (InvalidCastException e)
        {
            throw new InvalidOperationException(GetInvalidCastMessage(typeof(T)), e);
        }
    }

    private static T GetOrDefault<T>(object? payload, T value)
    {
        if (payload is not null)
        {
            if (payload is T type)
            {
                return type;
            }
            throw new InvalidOperationException(GetInvalidCastMessage(typeof(T)));
        }
        return value;
    }

    private static string GetInvalidCastMessage(Type type)
    {
        return $"This value cannot be interpreted as the type {type.Name}.";
    }

    /// <summary>Indicates whether this <c>Value</c> holds no data;</summary>
    public bool IsEmpty => Payload is null;

    /// <summary>The <see cref="bool" /> held by this <c>Value</c>.</summary>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="bool" />, or holds
    /// not data.
    /// </exception>
    public bool Boolean => CastOrThrow<bool>(Payload);

    /// <summary>The <see cref="int" /> held by this <c>Value</c>.</summary>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="int" />, or holds
    /// not data.
    /// </exception>
    public int Integer => CastOrThrow<int>(Payload);

    /// <summary>The <see cref="double" /> held by this <c>Value</c>.</summary>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="double" />, or holds
    /// no data,
    /// </exception>
    public double Number => CastOrThrow<double>(Payload);

    /// <summary>The <see cref="string" /> held by this <c>Value</c>.</summary>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="string" />, or holds
    /// no data.
    /// </exception>
    public string String => CastOrThrow<string>(Payload);

    /// <summary>
    /// The <see cref="IEnumerable{T}" /> of <see cref="string" />s held by this
    /// <c>Value</c>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as an
    /// <see cref="IEnumerable{String}" />, or holds no data.
    /// </exception>
    public IEnumerable<string> Strings => CastOrThrow<IEnumerable<string>>(Payload);

    internal object? Payload { get; }

    private Value(object? payload)
    {
        Payload = payload;
    }

    /// <summary>
    /// Provides the <see cref="bool" /> held by this <c>Value</c> or the specified
    /// <paramref name="defaultValue" /> if it does not hold any data.
    /// </summary>
    /// <param name="defaultValue">
    /// The data to return when this <c>Value</c> does not
    /// hold any.
    /// </param>
    /// <returns>
    /// The <see cref="bool" /> held by this <c>Value</c>, or the specified
    /// <paramref name="defaultValue" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="bool" />.
    /// </exception>
    public bool BooleanOrDefault(bool defaultValue = false)
    {
        return GetOrDefault(Payload, defaultValue);
    }

    /// <summary>
    /// Provides the <see cref="int" /> held by this <c>Value</c> or the specified
    /// <paramref name="defaultValue" /> if it does not hold any data.
    /// </summary>
    /// <param name="defaultValue">
    /// The data to return when this <c>Value</c> does not hold any.
    /// </param>
    /// <returns>
    /// The <see cref="int" /> held by this <c>Value</c>, or the specified
    /// <paramref name="defaultValue" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="int" />.
    /// </exception>
    public int IntegerOrDefault(int defaultValue = 0)
    {
        return GetOrDefault(Payload, defaultValue);
    }

    /// <summary>
    /// Provides the <see cref="double" /> held by this <c>Value</c> or the specified
    /// <paramref name="defaultValue" /> if it does not hold any data.
    /// </summary>
    /// <param name="defaultValue">
    /// The data to return when this <c>Value</c> does not hold any.
    /// </param>
    /// <returns>
    /// The <see cref="double" /> held by this <c>Value</c>, or the specified
    /// <paramref name="defaultValue" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="double" />.
    /// </exception>
    public double NumberOrDefault(double defaultValue = 0)
    {
        return GetOrDefault(Payload, defaultValue);
    }

    /// <summary>
    /// Provides the <see cref="string" /> held by this <c>Value</c> or the specified
    /// <paramref name="defaultValue" /> if it does not hold any data.
    /// </summary>
    /// <param name="defaultValue">
    /// The data to return when this <c>Value</c> does not hold any.
    /// </param>
    /// <returns>
    /// The <see cref="string" /> held by this <c>Value</c>, or the specified
    /// <paramref name="defaultValue" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as a <see cref="string" />.
    /// </exception>
    public string StringOrDefault(string defaultValue = "")
    {
        return GetOrDefault(Payload, defaultValue);
    }

    /// <summary>
    /// Provides the <see cref="IEnumerable{T}" /> of <see cref="string" />s held by
    /// this <c>Value</c> or the specified <paramref name="defaultValue" /> if it does
    /// not hold any data.
    /// </summary>
    /// <param name="defaultValue">
    /// The data to return when this <c>Value</c> does not
    /// hold any.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerable{T}" /> of <see cref="string" />s held by this
    /// <c>Value</c>, or the specified <paramref name="defaultValue" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If this <c>Value</c> cannot be interpreted as an <see cref="IEnumerable{T}" />
    /// of <see cref="string" />s.
    /// </exception>
    public IEnumerable<string> StringsOrDefault(params string[] defaultValue)
    {
        return GetOrDefault<IEnumerable<string>>(Payload, defaultValue);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is Value value)
        {
            return Equals(value);
        }
        return false;
    }

    /// <inheritdoc />
    public bool Equals(Value value)
    {
        if (Payload is null)
        {
            return value.Payload is null;
        }
        if (Payload.GetType() == value.Payload?.GetType())
        {
            if (value.Payload is IEnumerable<string> strings)
            {
                return strings.SequenceEqual(Strings);
            }
            return Payload.Equals(value.Payload);
        }
        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (Payload is IEnumerable<string> strings)
        {
            return strings.SequenceHashCode();
        }
        return HashCode.Combine(Payload);
    }
}
