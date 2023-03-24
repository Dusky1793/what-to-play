import React from "react";
import { useEffect } from "react";
import GameContainer from "./GameContainer";
import { useSelector, useDispatch } from "react-redux";
import {
    setGames,
    selectAllGames
} from '../redux/slices/gamesSlice';

function Games() {
    const games = useSelector(selectAllGames);
    const dispatch = useDispatch();

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");
        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetAllOwnedGames?encryptedSteamId=${steamId}`)
            .then(res => res.json())
            .then((result) => {
                dispatch(setGames(result.response.games));
            });
    }, []);

    return (
        <div class="gameContainer">
            {[].concat(games)
                .sort((a, b) => {
                    if (a === undefined ||
                        b === undefined ||
                        a.achievementDetails === undefined ||
                        b.achievementDetails === undefined) {
                        return -1;
                    }

                    return a.achievementDetails.achievementPercentage > b.achievementDetails.achievementPercentage ? -1 : 1;
                })
                .map(game => {
                return (
                    <GameContainer gameDetails={game} />
                );
            })}
        </div>
    );
}

export default Games;