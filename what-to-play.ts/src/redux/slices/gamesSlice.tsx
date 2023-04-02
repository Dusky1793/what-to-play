import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../store";

interface Achievement {
    name?: string;
    apiName?: string;
    achieved?: boolean;
    achieved_OldApi?: boolean;
    description?: string;
    iconClosed?: string;
    iconOpen?: string;
    unlocktime?: string;
}

interface AchievementDetails {
    achievementPercentage?: string;
    achievement?: Achievement[];
}

interface Game {
    appId: string;
    name?: string;
    img_Icon_Url?: string;
    achievementDetails?: AchievementDetails;
    playtime_Forever_Hours?: string;
    playtime_Forever?: string;
    rtime_Last_Played_DateTime?: Date;
    isSelected?: boolean;
}

interface Payload_updateGameAchievementDetailsByAppId {
    appId: string;
    achievementDetails: any
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
        updateGameAchievementDetailsByAppId: (state, action: PayloadAction<Payload_updateGameAchievementDetailsByAppId>) => {
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