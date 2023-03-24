import { configureStore } from "@reduxjs/toolkit";
import gamesReducer from './slices/gamesSlice'

export default configureStore({
    reducer: {
        games: gamesReducer
    }
});