import React, { useEffect } from "react";
import { IAchievementDetails, IGame } from "../interfaces/Interfaces";
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
            return <div className="gameDetailsContainerBody">
            {
                [...game.achievementDetails?.achievements].map(ac => {
                    return (
                        <div className="gameDetailsContainerAchievements">
                            <div className="gameDetailsAchievementImgContainer">
                                <img className="gameDetailsAchievementImg" src={ac.achieved ? ac.iconClosed : ac.iconOpen} />
                            </div>
                            <div className="gameDetailsAchievement">
                                <h4>{ac.name}</h4>
                                <div>{ac.description}</div>
                                {/* {ac.achieved && ac.unlocktime_DateTime ? <div>{new Intl.DateTimeFormat("en-GB", { dateStyle: 'medium', timeStyle: 'short' }).format(new Date(ac.unlocktime_DateTime))}</div> : ""} */}
                                {ac.achieved && ac.unlocktime_DateTime ? <div>{new Date(ac.unlocktime_DateTime).toLocaleString()}</div> : ""}
                            </div>
                        </div>
                    );
                })
            }
            </div>;
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
                achievementDetails : result as IAchievementDetails }));
        });
    };

    return (
        <div className="gameDetailsContainer">
            <div className="gameDetailsContainerHeader">
                <div>
                    <input type="button" value={"Refresh"} onClick={refreshGameDetails} />
                </div>
                <h2>{game.name}</h2>
                <h3>{game.playtime_Forever_Hours} Hours</h3>
                <h3>{game.achievementDetails?.achievementPercentage} %</h3>
            </div>
            {renderAchievements()}
        </div>
    );
}

export default GameDetailsContainer;