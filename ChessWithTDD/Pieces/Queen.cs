﻿namespace ChessWithTDD
{
    public class Queen : IPiece
    {
        private Colour _colour;

        public Queen(Colour colour)
        {
            _colour = colour;
        }

        public Colour Colour { get { return _colour; } }

        public bool CanMove(ISquare fromSquare, ISquare toSquare)
        {
            if (toSquare.IsDiagonalTo(fromSquare))
            {
                //diagonal
                return true;
            }
            else if (toSquare.IsInSameRowAs(fromSquare) ||
                     toSquare.IsInSameColumnAs(fromSquare))
            {
                //vertical or horizontal
                return true;
            }
            return false;
        }
    }
}