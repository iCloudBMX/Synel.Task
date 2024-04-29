﻿namespace Task.Application.Contracts;

public class Result
{
    protected Result() { }

    public string Message { get; protected set; }

    public bool Succeeded { get; protected set; }

    public static Result Fail()
    {
        return new Result { Succeeded = false };
    }

    public static Result Fail(string message)
    {
        return new Result { Succeeded = false, Message = message };
    }

    public static Result Success()
    {
        return new Result { Succeeded = true };
    }

    public static Result Success(string message)
    {
        return new Result { Succeeded = true, Message = message };
    }
}

public class Result<T> : Result
{
    protected Result() { }

    public T Data { get; private init; }

    public new static Result<T> Fail()
    {
        return new Result<T> { Succeeded = false };
    }

    public new static Result<T> Fail(string message)
    {
        return new Result<T> { Succeeded = false, Message = message };
    }

    public new static Result<T> Success()
    {
        return new Result<T> { Succeeded = true };
    }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Message = message };
    }
}
