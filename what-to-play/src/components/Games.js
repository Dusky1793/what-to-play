import React from "react";
import { useState, useEffect } from "react";
import GameContainer from "./GameContainer";

function Games() {
    const [games, setGames] = useState([]);

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");
        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetAllOwnedGames?encryptedSteamId=${steamId}`)
            .then(res => res.json())
            .then((result) => {
                setGames(result.response.games);
            });
    }, []);

    return (
        <div class="gameContainer">
            {games.map(game => {
                return (
                    <GameContainer gameDetails={game} />
                );
            })}
        </div>
    );
}

export default Games;