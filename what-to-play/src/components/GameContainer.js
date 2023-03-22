import React from "react";
import { useState, useEffect } from "react";
import '../style/GameContainer.css';

function GameContainer(props) {
    const getImageUrl = (imgIconUrl) =>{
        const appId = props.gameDetails.appId;
        const imgUrl = `http://media.steampowered.com/steamcommunity/public/images/apps/${appId}/${imgIconUrl}.jpg`;
        return imgUrl;
    };

    const[gameDetails, setGameDetails] = useState({});

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");
        const appId = props.gameDetails.appId;

        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetAchievementDetailsByAppId?encryptedSteamId=${steamId}&appId=${appId}`)
        .then(res => res.json())
        .then((result) => {
            setGameDetails(result);
        });
    }, []);

    return (
        <div class="gameItem">
            {/* <div>
                    <img src={gameDetails.gameLogo} />
                </div> */}
            <div class="gameItemImgContainer">
                <img class="gameItemImg" src={getImageUrl(props.gameDetails.img_Icon_Url)} />
            </div>
            <div class="gameDetails">
                <div class="gameTitle">{props.gameDetails.name}</div>
                <div>{props.gameDetails.playtime_Forever_Hours} Hours Played</div>
                <div>{gameDetails === undefined || gameDetails.achievementPercentage === undefined || gameDetails.achievementPercentage === -1 ? "---" : `${gameDetails.achievementPercentage}% Achieved`}</div>
            </div>
        </div>
    );
}

export default GameContainer;