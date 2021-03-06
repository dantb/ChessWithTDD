﻿using DeepCopyExtensions;
using System.Windows.Forms;

namespace ChessWithTDD
{
    public class MoveIntoCheckValidatorUsingCloning : IMoveIntoCheckValidator
    {
        public bool MoveCausesMovingTeamCheck(IBoard theBoard, ISquare fromSquare, ISquare toSquare)
        {
            try
            {
                if (toSquare.Piece is IKing)
                {
                    //special case, not into check since this would end the game
                    return false;
                }
                else
                {
                    //it doesn't matter whether the moving piece is a king or not, we still need to run the scenario
                    //that this move is applied and then analyse the resulting board
                    return MoveResultsInOurKingBeingInCheck(theBoard, fromSquare, toSquare);
                }
            }
            catch
            {
                //error meant the move screwed up the board - special error handling as this can help find edge cases
                MessageBox.Show("Error in move into check validator using cloning.");
                return false;
            }
        }

        private bool MoveResultsInOurKingBeingInCheck(IBoard theBoard, ISquare fromSquare, ISquare toSquare)
        {
            IBoard newBoard = GetNewBoardWithThisMoveApplied(theBoard, fromSquare, toSquare);

            //can the other team whose turn it is move onto our king after our move?
            ISquare newBoardKingSquare = newBoard.OtherTeamKingSquare;
            foreach (ISquare square in newBoard.MovingTeamPieceSquares)
            {
                if (newBoard.MoveIsValid(square, newBoardKingSquare))
                {
                    return true;
                }
            }
            return false;
        }

        private IBoard GetNewBoardWithThisMoveApplied(IBoard theBoard, ISquare fromSquare, ISquare toSquare)
        {
            // just clone the board to get it into the current state, includes any partially done moves (no check and mate states)
            IBoard newBoard = theBoard.DeepCopyByExpressionTree();

            if (theBoard.MoveWithoutCheckAndMateUpdated != null)
            {
                // we aren't doing a full apply, the board already contains the state with this move. We just have to up the turn counter
                // and add it to the move data
                // TODO - I don't like this, find away to make this more the board's responsibility e.g. by making the post move application updates
                // method public on the board. This may be worth doing now that cloning is a fundamental part of the system
                newBoard.TurnCounter++;
                newBoard.OrderedMoveData.Add(theBoard.MoveWithoutCheckAndMateUpdated);
            }

            ISquare newBoardFromSquare = newBoard.GetSquare(fromSquare.Row, fromSquare.Col);
            ISquare newBoardToSquare = newBoard.GetSquare(toSquare.Row, toSquare.Col);
            newBoard.ApplyWithoutUpdatingCheckAndMate(newBoardFromSquare, newBoardToSquare);
            return newBoard;
        }
    }
}
