import React, { useEffect } from "react";
import { IGame } from "../interfaces/Interfaces";
import { useAppDispatch } from "../hooks/hooks";
import { updateGameAchievementDetailsByAppId } from "../redux/slices/gamesSlice";

interface PropsGameDetailsContainer {
    game: IGame;
}

function GameDetailsContainer(props:PropsGameDetailsContainer) {
    const game = props.game;
    const dispatch = useAppDispatch();

    const renderAchievements = () => {
        if(game.achievementDetails?.achievements)
        {
            return <>
            {
                [...game.achievementDetails?.achievements].map(ac => {
                    return (
                        <div className="gameDetailsAchievementContainer">
                            <div className="gameDetailsAchievementImgContainer">
                                <img className="gameDetailsAchievementImg" height={50} width={50} src={ac.achieved ? ac.iconClosed : ac.iconOpen} />
                            </div>
                            <div className="gameDetailsAchievement">
                                <h4>{ac.name}</h4>
                                <div>description: {ac.description}</div>
                                <div>unlock time: {ac.unlocktime}</div>
                            </div>
                        </div>
                    );
                })
            }
            </>;
        }

        return "";
    };

    const refreshGameDetails = () => {
        const steamId = localStorage.getItem("steamId");

        fetch(`${process.env.REACT_APP_API_URL}/Steam/GetFullAchievementDetailsByAppId?encryptedSteamId=${steamId}&appId=${game.appId}`)
        .then(res => res.json())
        .then((result) => {
            dispatch(updateGameAchievementDetailsByAppId({ 
                appId: game.appId,
                achievementDetails : result }));
        });
    };

    return (
        <div className="gameDetailsContainer">
            <div className="gameDetailsContainerHeader">
                <div>
                    <input type="button" value={"Refresh"} onClick={refreshGameDetails} />
                </div>
                <h1>{game.name}</h1>
                <h2>{game.playtime_Forever_Hours} Hours</h2>
                <h2>{game.achievementDetails?.achievementPercentage} %</h2>
            </div>
            <div className="gameDetailsContainerAchievements">
                {renderAchievements()}
            </div>
        </div>
    );
}

export default GameDetailsContainer;