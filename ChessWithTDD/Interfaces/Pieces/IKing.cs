﻿namespace ChessWithTDD
{
    public interface IKing : IPiece
    {
        bool InCheckState { get; set; }

        bool HasMoved { get; set; }
    }
}
