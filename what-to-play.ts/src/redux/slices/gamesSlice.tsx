import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";
import {
    IGame, 
    IPayload_updateGameAchievementDetailsByAppId, 
    IGamesState 
} from "../../interfaces/Interfaces";


const initialState: IGamesState = {
    games: []
}

// REDUCERS / BASE STATE
export const gamesSlice = createSlice({
    name: 'games',
    initialState,
    reducers: {
        setGames: (state, action: PayloadAction<IGame[]>) => {
            state.games = action.payload;
        },
        updateGameAchievementDetailsByAppId: (state, action: PayloadAction<IPayload_updateGameAchievementDetailsByAppId>) => {
            const game = state.games.find(g => g.appId === action.payload.appId);
            if(game != undefined) {
                game.achievementDetails = action.payload.achievementDetails;
            }
        },
        setSelectedGameByAppId: (state, action: PayloadAction<string>) => {
            state.games.forEach(game => {
                if(game.appId == action.payload) {
                    game.isSelected = true;
                }
                else {
                    game.isSelected = false;
                }
            });
        }
    }
});

export const {
    setGames, 
    updateGameAchievementDetailsByAppId, 
    setSelectedGameByAppId 
} = gamesSlice.actions;

// SELECTORS
export const selectAllGames = (state: RootState) => state.games.games;
export const getSelectedGame = (state: RootState) => state.games.games.find(g => g.isSelected === true);

export default gamesSlice.reducer;