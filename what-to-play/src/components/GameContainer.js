import React from "react";
import { useState, useEffect } from "react";
import '../style/GameContainer.css';

function GameContainer(props) {
    const getImageUrl = () =>{
        const appId = props.gameDetails.appId;
        const imgIconUrl = props.gameDetails.img_Icon_Url;
        const imgUrl = `http://media.steampowered.com/steamcommunity/public/images/apps/${appId}/${imgIconUrl}.jpg`;
        return imgUrl;
    };

    const[gameDetails, setGameDetails] = useState({});

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");
        const appId = props.gameDetails.appId;
        console.log(appId);

        fetch(`http://localhost:5220/Steam/GetAchievementDetailsByAppId?encryptedSteamId=${steamId}&appId=${appId}`)
        .then(res => res.json())
        .then((result) => {
            setGameDetails(result);
        });
    }, []);

    return (
        <div class="gameItem">
            <div class="gameItemImg">
                <img src={getImageUrl()} />
            </div>
            <div class="gameDetails">
                <div class="gameTitle">{props.gameDetails.name}</div>
                <div>{props.gameDetails.playtime_Forever_Hours}</div>
                <div>{gameDetails.achievementPercentage === -1 ? "---" : `${gameDetails.achievementPercentage}% Achieved`}</div>
            </div>
        </div>
    );
}

export default GameContainer;