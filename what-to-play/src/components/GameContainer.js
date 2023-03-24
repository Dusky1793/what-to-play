import React from "react";
import { useState, useEffect } from "react";
import '../style/GameContainer.css';
import { useSelector, useDispatch } from "react-redux";
import {
    updateGameAchievementDetailsByAppId
} from '../redux/slices/gamesSlice';

function GameContainer(props) {
    const appId = props.gameDetails.appId;
    const getImageUrl = (imgIconUrl) =>{
        const imgUrl = `http://media.steampowered.com/steamcommunity/public/images/apps/${appId}/${imgIconUrl}.jpg`;
        return imgUrl;
    };

    const dispatch = useDispatch();
    const game = useSelector(state => state.games.games.find(g => g.appId === props.gameDetails.appId));

    useEffect(() => {
        const steamId = localStorage.getItem("steamId");

        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetAchievementDetailsByAppId?encryptedSteamId=${steamId}&appId=${appId}`)
        .then(res => res.json())
        .then((result) => {
            dispatch(updateGameAchievementDetailsByAppId({ 
                appId: appId,
                achievementDetails : result }));
        });
    }, []);

    return (
        <div class="gameItem">
            {/* <div>
                    <img src={gameDetails.gameLogo} />
                </div> */}
            <div class="gameItemImgContainer">
                <img class="gameItemImg" src={getImageUrl(game.img_Icon_Url)} />
            </div>
            <div class="gameDetails">
                <div class="gameTitle">{game.name}</div>
                <div>{game.playtime_Forever_Hours} Hours Played</div>
                <div>
                    {game === undefined || game.achievementDetails === undefined || 
                     game.achievementDetails.achievementPercentage === undefined || 
                     game.achievementDetails.achievementPercentage === -1 ? "---" : `${game.achievementDetails.achievementPercentage}% Achieved`}
                </div>
            </div>
        </div>
    );
}

export default GameContainer;