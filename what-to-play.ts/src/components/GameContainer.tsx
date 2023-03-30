import React from "react";
import { useState, useEffect } from "react";
import '../style/GameContainer.css';
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import {
    updateGameAchievementDetailsByAppId
} from '../redux/slices/gamesSlice';

interface GameDetails {
    appId: string;
}

interface GameContainerProps {
    gameDetails: GameDetails;
}

function GameContainer(props:GameContainerProps) {
    const appId = props.gameDetails.appId;
    const getImageUrl = (imgIconUrl?: string) =>{
        const imgUrl = `http://media.steampowered.com/steamcommunity/public/images/apps/${appId}/${imgIconUrl}.jpg`;
        return imgUrl;
    };

    const dispatch = useAppDispatch();
    const game = useAppSelector(state => state.games.games.find(g => g.appId === props.gameDetails.appId));

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
        <div className="gameItem">
            {/* <div>
                    <img src={gameDetails.gameLogo} />
                </div> */}
            <div className="gameItemImgContainer">
                <img className="gameItemImg" src={getImageUrl(game?.img_Icon_Url)} />
            </div>
            <div className="gameDetails">
                <div className="gameTitle">{game?.name}</div>
                <div>{game?.playtime_Forever_Hours} Hours Played</div>
                <div>
                    {game === undefined || game.achievementDetails === undefined || 
                     game.achievementDetails?.achievementPercentage === undefined || 
                     game.achievementDetails?.achievementPercentage === "-1" ? "---" : `${game.achievementDetails?.achievementPercentage}% Achieved`}
                </div>
            </div>
        </div>
    );
}

export default GameContainer;