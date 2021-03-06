﻿using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using static ChessWithTDD.BoardConstants;
using static ChessWithTDD.Tests.TestHelpers.CommonTestMethods;
using static ChessWithTDD.Tests.TestHelpers.RhinoMockExtensions;

namespace ChessWithTDD.Tests
{
    [TestFixture]
    public class BoardCacheTests
    {
        #region Update cache tests

        [Test]
        public void BlackKingIsSavedToBlackKingSquareIfInPendingUpdatesOfBoardAndUpdatesCleared()
        {
            IKing theKing = MockKingWithColour(Colour.Black);
            ISquare kingSquare = MockSquareWithPiece(4, 4, theKing);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { kingSquare };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(kingSquare.Row, kingSquare.Col)).Return(kingSquare);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.UpdateBoardCache();

            Assert.AreEqual(boardCache.BlackKingSquare, kingSquare);
            Assert.AreEqual(pendingUpdates.Count, 0);
            Assert.True(boardCache.BlackPieceSquares.Contains(kingSquare));
        }

        [Test]
        public void BlackKingSquareInPendingUpdatesThatPreviouslyWasInWhitePieceCacheIsRemoved()
        {
            IKing theKing = MockKingWithColour(Colour.Black);
            ISquare kingSquare = MockSquareWithPiece(4, 4, theKing);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { kingSquare };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(kingSquare.Row, kingSquare.Col)).Return(kingSquare);
            BoardCache boardCache = new BoardCache();
            boardCache.WhitePieceSquares.Add(kingSquare);
            boardCache.InitialiseBoardCache(board);

            boardCache.UpdateBoardCache();

            Assert.False(boardCache.WhitePieceSquares.Contains(kingSquare));
        }

        [Test]
        public void WhiteKingIsSavedToWhiteKingSquareAndWhitePiecesIfInPendingUpdatesOfBoardAndUpdatesCleared()
        {
            IKing theKing = MockKingWithColour(Colour.White);
            ISquare kingSquare = MockSquareWithPiece(4, 4, theKing);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { kingSquare };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(kingSquare.Row, kingSquare.Col)).Return(kingSquare);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.UpdateBoardCache();

            Assert.AreEqual(boardCache.WhiteKingSquare, kingSquare);
            Assert.AreEqual(pendingUpdates.Count, 0);
            Assert.True(boardCache.WhitePieceSquares.Contains(kingSquare));
        }

        [Test]
        public void WhiteKingSquareInPendingUpdatesThatPreviouslyWasInBlackPieceCacheIsRemoved()
        {
            IKing theKing = MockKingWithColour(Colour.White);
            ISquare kingSquare = MockSquareWithPiece(4, 4, theKing);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { kingSquare };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(kingSquare.Row, kingSquare.Col)).Return(kingSquare);
            BoardCache boardCache = new BoardCache();
            boardCache.BlackPieceSquares.Add(kingSquare);
            boardCache.InitialiseBoardCache(board);

            boardCache.UpdateBoardCache();

            Assert.False(boardCache.BlackPieceSquares.Contains(kingSquare));
        }

        [Test]
        public void NothingIsSavedForSquaresWithNoPieceButItIsRemoved()
        {
            ISquare square = MockSquare();
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.UpdateBoardCache();

            Assert.IsNull(boardCache.WhiteKingSquare);
            Assert.IsNull(boardCache.BlackKingSquare);
            Assert.False(board.PendingUpdates.Contains(square));
        }

        [Test]
        public void SquareInPendingUpdatesWithNoPieceIsRemovedFromBoardCacheAndPendingUpdates()
        {
            ISquare square = MockSquare();
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.WhitePieceSquares.Add(square);
            boardCache.UpdateBoardCache();

            Assert.False(boardCache.WhitePieceSquares.Contains(square));
            Assert.False(pendingUpdates.Contains(square));
        }

        [Test]
        public void SquareInPendingUpdatesWithWhitePieceIsAddedToWhitePieceCacheAndRemovedFromPendingUpdates()
        {
            IPiece piece = MockPieceWithColour(Colour.White);
            ISquare square = MockSquareWithPiece(piece);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(square.Row, square.Col)).Return(square);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.UpdateBoardCache();

            Assert.True(boardCache.WhitePieceSquares.Contains(square));
            Assert.False(pendingUpdates.Contains(square));
        }


        [Test]
        public void SquareInPendingUpdatesWithWhitePieceThatWasInBlackPieceCacheIsRemoved()
        {
            IPiece piece = MockPieceWithColour(Colour.White);
            ISquare square = MockSquareWithPiece(piece);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(square.Row, square.Col)).Return(square);
            BoardCache boardCache = new BoardCache();
            boardCache.BlackPieceSquares.Add(square);
            boardCache.InitialiseBoardCache(board);

            boardCache.UpdateBoardCache();

            Assert.False(boardCache.BlackPieceSquares.Contains(square));
        }

        [Test]
        public void SquareInPendingUpdatesWithBlackPieceIsAddedToBlackPieceCacheAndRemovedFromPendingUpdates()
        {
            IPiece piece = MockPieceWithColour(Colour.Black);
            ISquare square = MockSquareWithPiece(piece);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(square.Row, square.Col)).Return(square);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);
            boardCache.UpdateBoardCache();

            Assert.True(boardCache.BlackPieceSquares.Contains(square));
            Assert.False(pendingUpdates.Contains(square));
        }

        [Test]
        public void SquareInPendingUpdatesWithBlackPieceThatWasInWhitePieceCacheIsRemoved()
        {
            IPiece piece = MockPieceWithColour(Colour.Black);
            ISquare square = MockSquareWithPiece(piece);
            IBoard board = MockBoard();
            List<ISquare> pendingUpdates = new List<ISquare> { square };
            board.Stub(b => b.PendingUpdates).Return(pendingUpdates);
            board.Stub(b => b.GetSquare(square.Row, square.Col)).Return(square);
            BoardCache boardCache = new BoardCache();
            boardCache.WhitePieceSquares.Add(square);
            boardCache.InitialiseBoardCache(board);

            boardCache.UpdateBoardCache();

            Assert.False(boardCache.WhitePieceSquares.Contains(square));
        }

        #endregion Update cache tests


        [Test]
        public void BoardCacheInitialisedWithCorrectBoard()
        {
            IBoard board = MockBoard();

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            Assert.AreEqual(boardCache.TheBoard, board);
        }

        [Test]
        public void BoardCacheInitialisedWithWhiteKingCorrectly()
        {
            IKing king = MockKingWithColour(Colour.White);
            ISquare whiteKingSquare = MockSquareWithPiece(WHITE_BACK_ROW, KING_COLUMN, king);
            IBoard board = MockBoard();
            board.Stub(b => b.GetSquare(WHITE_BACK_ROW, KING_COLUMN)).Return(whiteKingSquare);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            Assert.AreEqual(boardCache.WhiteKingSquare, whiteKingSquare);
            Assert.True(boardCache.WhitePieceSquares.Contains(whiteKingSquare));
        }

        [Test]
        public void BoardCacheInitialisedWithBlackKingCorrectly()
        {
            IKing king = MockKingWithColour(Colour.Black);
            ISquare blackKingSquare = MockSquareWithPiece(BLACK_BACK_ROW, KING_COLUMN, king);
            IBoard board = MockBoard();
            board.Stub(b => b.GetSquare(BLACK_BACK_ROW, KING_COLUMN)).Return(blackKingSquare);

            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            Assert.AreEqual(boardCache.BlackKingSquare, blackKingSquare);
            Assert.True(boardCache.BlackPieceSquares.Contains(blackKingSquare));
        }

        #region Board cache system level initialisation

        /// <summary>
        /// System level test for board cache initialisation
        /// </summary>
        [Test]
        public void BoardCacheInitialisedWithWhitePawnsCorrectly()
        {
            //Need a real initialised board
            BoardInitialiser boardInitialiser = new BoardInitialiser(MockCastlingMoveValidator());
            IStrictServiceLocator serviceLocator = MockServiceLocator();
            serviceLocator.Stub(s => s.GetServiceBoardInitialiser()).Return(boardInitialiser).OverridePrevious();
            Board board = new Board(serviceLocator);

            //System under test is a different board cache as we want to initialise here
            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_PAWN_INITAL_ROW, i)));
            }
        }

        [Test]
        public void BoardCacheInitialisedWithBlackPawnsCorrectly()
        {
            //Need a real initialised board
            BoardInitialiser boardInitialiser = new BoardInitialiser(MockCastlingMoveValidator());
            IStrictServiceLocator serviceLocator = MockServiceLocator();
            serviceLocator.Stub(s => s.GetServiceBoardInitialiser()).Return(boardInitialiser).OverridePrevious();
            Board board = new Board(serviceLocator);

            //System under test is a different board cache as we want to initialise here
            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            for (int i = 0; i < BOARD_DIMENSION; i++)
            {
                Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_PAWN_INITAL_ROW, i)));
            }
        }

        [Test]
        public void BoardCacheInitialisedWithWhiteBackRowCorrectly()
        {
            //Need a real initialised board
            BoardInitialiser boardInitialiser = new BoardInitialiser(MockCastlingMoveValidator());
            IStrictServiceLocator serviceLocator = MockServiceLocator();
            serviceLocator.Stub(s => s.GetServiceBoardInitialiser()).Return(boardInitialiser).OverridePrevious();
            Board board = new Board(serviceLocator);

            //System under test is a different board cache as we want to initialise here
            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, LEFT_ROOK_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, RIGHT_ROOK_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, LEFT_BISHOP_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, RIGHT_BISHOP_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, LEFT_KNIGHT_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, RIGHT_KNIGHT_COL)));
            Assert.True(boardCache.WhitePieceSquares.Contains(board.GetSquare(WHITE_BACK_ROW, QUEEN_COLUMN)));
        }

        [Test]
        public void BoardCacheInitialisedWithBlackBackRowCorrectly()
        {
            //Need a real initialised board
            BoardInitialiser boardInitialiser = new BoardInitialiser(MockCastlingMoveValidator());
            IStrictServiceLocator serviceLocator = MockServiceLocator();
            serviceLocator.Stub(s => s.GetServiceBoardInitialiser()).Return(boardInitialiser).OverridePrevious();
            Board board = new Board(serviceLocator);

            //System under test is a different board cache as we want to initialise here
            BoardCache boardCache = new BoardCache();
            boardCache.InitialiseBoardCache(board);

            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, LEFT_ROOK_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, RIGHT_ROOK_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, LEFT_BISHOP_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, RIGHT_BISHOP_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, LEFT_KNIGHT_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, RIGHT_KNIGHT_COL)));
            Assert.True(boardCache.BlackPieceSquares.Contains(board.GetSquare(BLACK_BACK_ROW, QUEEN_COLUMN)));
        }

        #endregion Board cache system level initialisation

    }
}
