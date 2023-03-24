import { createSlice } from "@reduxjs/toolkit";

// REDUCERS / BASE STATE
export const gamesSlice = createSlice({
    name: 'games',
    initialState: {
        games: []
    },
    reducers: {
        setGames: (state, action) => {
            state.games = action.payload;
        },
        updateGameAchievementDetailsByAppId: (state, action) => {
            const game = state.games.find(g => g.appId === action.payload.appId);
            game.achievementDetails = action.payload.achievementDetails;
        }
    }
});

export const { setGames, updateGameAchievementDetailsByAppId } = gamesSlice.actions;

// SELECTORS
export const selectAllGames = (state) => state.games.games;

export default gamesSlice.reducer;