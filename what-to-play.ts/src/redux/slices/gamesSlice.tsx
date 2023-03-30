import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface AchievementDetails {
    achievementPercentage?: string;
}

interface Game {
    appId: string;
    name?: string;
    img_Icon_Url?: string;
    achievementDetails?: AchievementDetails;
    playtime_Forever_Hours?: string;
}

interface GamesState {
    games: Game[];
}

const initialState: GamesState = {
    games: []
}

// REDUCERS / BASE STATE
export const gamesSlice = createSlice({
    name: 'games',
    initialState,
    reducers: {
        setGames: (state, action: PayloadAction<Game[]>) => {
            state.games = action.payload;
        },
        updateGameAchievementDetailsByAppId: (state, action) => {
            const game = state.games.find(g => g.appId === action.payload.appId);
            if(game != undefined) {
                game.achievementDetails = action.payload.achievementDetails;
            }
        }
    }
});

export const { setGames, updateGameAchievementDetailsByAppId } = gamesSlice.actions;

// SELECTORS
export const selectAllGames = (state: RootState) => state.games.games;

export default gamesSlice.reducer;