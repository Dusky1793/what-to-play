import React from "react";
import { useState, useEffect } from "react";
import GameContainer from "./GameContainer";

function Games() {
    const [games, setGames] = useState([]);

    useEffect(() => {
        const steamId = localStorage.getItem("steamId"); // TODO: self reminder to make sure this is encrypted
        fetch(`http://localhost:5220/Steam/GetAllOwnedGames?steamId=${steamId}`)
            .then(res => res.json())
            .then((result) => {
                setGames(result.response.games);
            });
    }, []);
    
    return (
        <div>
            {games.map(game => {
                return (
                    <GameContainer gameDetails={game} />
                );
            })}
        </div>
    );
}

export default Games;