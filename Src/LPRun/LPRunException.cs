﻿using System;
using System.Diagnostics.CodeAnalysis;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif
using System.Threading.Tasks;

namespace LPRun;

/// <summary>Represents errors that occur during LPRun execution.</summary>
[Serializable]
public class LPRunException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LPRunException" /> class.
    /// </summary>
    public LPRunException()
    {
    }

#if !NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of the <see cref="LPRunException" /> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
    /// <exception cref="SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
    protected LPRunException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="LPRunException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public LPRunException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LPRunException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public LPRunException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Rethrows the exception thrown by <paramref name="action"/> as <see cref="LPRunException"/>. Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.
    /// </summary>
    /// <param name="action">The <see cref="Action"/> to execute.</param>
    /// <exception cref="LPRunException">Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.</exception>
    internal static void Wrap(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            throw Wrap(e);
        }
    }

    /// <summary>
    /// Rethrows the exception thrown by <paramref name="func"/> as <see cref="LPRunException"/>. Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.
    /// </summary>
    /// <typeparam name="TResult">The <paramref name="func"/> result type.</typeparam>
    /// <param name="func">The <see cref="Func{T}"/> to execute.</param>
    /// <returns>The result of <see cref="Func{T}"/> call.</returns>
    /// <exception cref="LPRunException">Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.</exception>
    internal static TResult Wrap<TResult>(Func<TResult> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            throw Wrap(e);
        }
    }

    /// <summary>
    /// Rethrows the exception thrown by asynchronous <paramref name="func"/> as <see cref="LPRunException"/>. Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.
    /// </summary>
    /// <typeparam name="TResult">The <paramref name="func"/> result type.</typeparam>
    /// <param name="func">The asynchronous <see cref="Func{T}"/> where TResult is a <see cref="Task{T}"/> to execute.</param>
    /// <returns>A task that represents the asynchronous result of <see cref="Func{T}"/> call.</returns>
    /// <exception cref="LPRunException">Keeps the original exception as <see cref="P:System.Exception.InnerException"/>.</exception>
    internal static async Task<TResult> WrapAsync<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            throw Wrap(e);
        }
    }

    [DoesNotReturn]
    private static LPRunException Wrap(Exception e) =>
        throw new LPRunException(e.Message, e);
}
