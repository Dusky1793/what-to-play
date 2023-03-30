import React from "react";
import { useEffect } from "react";
import GameContainer from "./GameContainer";
import {
    setGames,
    selectAllGames
} from '../redux/slices/gamesSlice';
import { useAppDispatch, useAppSelector } from "../hooks/hooks";

function Games() {
    const games = useAppSelector(selectAllGames);
    const dispatch = useAppDispatch();

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");
        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetAllOwnedGames?encryptedSteamId=${steamId}`)
            .then(res => res.json())
            .then((result) => {
                dispatch(setGames(result.response.games));
            });
    }, []);

    return (
        <div className="gameContainer">
            {[...games].sort((g1, g2) => {
                if (g1 === undefined ||
                    g2 === undefined ||
                    g1.achievementDetails === undefined ||
                    g2.achievementDetails === undefined) {
                    return -1;
                }

                if (!g1.achievementDetails.achievementPercentage) return 1;

                if (!g2.achievementDetails.achievementPercentage) return -1;


                return g1.achievementDetails.achievementPercentage > g2.achievementDetails.achievementPercentage ? -1 : 1;
            }).map(game => <GameContainer gameDetails={game} />)}
        </div>
    );
}

export default Games;