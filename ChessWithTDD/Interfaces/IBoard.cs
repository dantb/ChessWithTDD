﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChessWithTDD
{
    public interface IBoard
    {
        event MoveAppliedEventHandler MoveAppliedEvent;

        int RowCount { get; }

        int ColCount { get; }

        int TurnCounter { get; set; }

        bool InCheck { get; set; }

        bool CheckMate { get; set; }

        bool StaleMate { get; set; }

        List<List<ISquare>> Squares { get; }

        /// <summary>
        /// Contains in order the moves applied to get the board into its current state
        /// </summary>
        ObservableCollection<MoveGenerationData> OrderedMoveData { get; }

        /// <summary>
        /// This is to note that there is a move that has been applied to the board, but the check and checkmate
        /// states haven't been updated yet. If everything is up to date this is null.
        /// </summary>
        MoveGenerationData MoveWithoutCheckAndMateUpdated { get; set; }

        /// <summary>
        /// Board updates that have not yet been cached
        /// </summary>
        List<ISquare> PendingUpdates { get; set; }

        /// <summary>
        /// Square of the king belonging to the team whose turn it currently is.
        /// </summary>
        ISquare MovingTeamKingSquare { get; }

        /// <summary>
        /// Square of the king belonging to the team not making a move.
        /// </summary>
        ISquare OtherTeamKingSquare { get; }

        /// <summary>
        /// Squares containing pieces of the team whose turn it currently is.
        /// </summary>
        HashSet<ISquare> MovingTeamPieceSquares { get; }

        /// <summary>
        /// Squares containing pieces of the team not making a move.
        /// </summary>
        HashSet<ISquare> OtherTeamPieceSquares { get; }

        Colour TeamWithTurn { get; }

        /// <summary>
        /// Returns a clone of the square at position (row, col) on the board.
        /// To change the state of the board you should use Apply, after checking move validity with IsValidMove.
        /// </summary>
        ISquare GetSquare(int row, int col);

        /// <summary>
        /// Use to check the validity of a given move. Combines general board validation with specific piece validation.
        /// </summary>
        /// <param name="fromSquare">Square containing the piece you want to check.</param>
        /// <param name="toSquare">Square to check your piece against.</param>
        /// <returns></returns>
        bool MoveIsValid(ISquare fromSquare, ISquare toSquare);

        /// <summary>
        /// This is the only way you can change the state of the board. The move is applied to the piece in the from square,
        /// capturing the piece in the to square if it exists.
        /// </summary>
        /// <param name="fromSquare">Square containing the piece to be moved.</param>
        /// <param name="toSquare">Square the piece will be moved to, capturing pieces as required.</param>
        void Apply(ISquare fromSquare, ISquare toSquare);

        void ApplyWithoutUpdatingCheckAndMate(ISquare fromSquare, ISquare toSquare);


        void SetSquare(ISquare square);

        void UpdateBoardCache();
    }
}
